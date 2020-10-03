/*
Copyright (c) 2020, Micah Schuster
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Singleton section
    private static GameManager _instance = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static GameManager instance()
    {
        return _instance;
    }


    public Spawner spawner;

    private List<CollectibleSmall> currentlyCombining;

    // Start is called before the first frame update
    void Start()
    {
        currentlyCombining = new List<CollectibleSmall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyCombining.Count != 0)
        {
            if (!currentlyCombining[0].IsCombining() && !currentlyCombining[1].IsCombining())
            {
                spawner.manualSpawn(currentlyCombining[0].uAngleD, currentlyCombining[0].vAngleD, 2);
                currentlyCombining[0].despawn();
                currentlyCombining[1].despawn();
                currentlyCombining.Clear();
            }
        }




        //test initiate combine
        if (Input.GetKeyDown(KeyCode.Space) && currentlyCombining.Count == 0)
        {
            //get current list of spawns
            List<GameObject> spawns = spawner.getCollectibles();
            int tries = 0;
            //only try to do this a few times
            while (tries < 5)
            {
                //pick two random spawns
                int spawn1 = Random.Range(0, spawns.Count);
                int spawn2 = Random.Range(0, spawns.Count);

                if (spawns[spawn1].GetComponent<CollectibleSmall>() != null && spawns[spawn2].GetComponent<CollectibleSmall>() != null)
                {
                    print("Found 2 small collectibles");
                    CollectibleSmall cs1 = spawns[spawn1].GetComponent<CollectibleSmall>();
                    CollectibleSmall cs2 = spawns[spawn2].GetComponent<CollectibleSmall>();
                    //check if they are relatively close, track and angle
                    int track1 = (int)cs1.getTrackAngle() / 36;
                    int track2 = (int)cs2.getTrackAngle() / 36;
                    float trackDiff = Mathf.Abs(track1 - track2);
                    if (trackDiff > 1 && trackDiff % 2 == 0)
                    {
                        print("found 2 within 1 track");
                        if (Mathf.Abs(cs1.getCurrentAngle() - cs2.getCurrentAngle()) <= 20f)
                        {
                            print("Found 2 within angle");
                            //do combine
                            float newAngle = (cs1.getCurrentAngle() + cs2.getCurrentAngle()) / 2;
                            float newTrackAngle = (cs1.getTrackAngle() + cs2.getTrackAngle()) / 2;
                            cs1.initiateCombine(newTrackAngle, newAngle);
                            cs2.initiateCombine(newTrackAngle, newAngle);
                            currentlyCombining.Add(cs1);
                            currentlyCombining.Add(cs2);
                            break;
                        }
                    }
                }
                tries++;
            }
        }
    }
}
