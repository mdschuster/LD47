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
using TMPro;
using UnityEngine.UI;

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

    private float combineTimer = 5f;
    public float timeBetweenCombine;
    public float decreaseSpeed;
    public float powerIncrease;


    public TextMeshProUGUI FusionText;
    public TextMeshProUGUI FusionValue;

    public TextMeshProUGUI PowerText;
    public TextMeshProUGUI PowerValue;



    public MeshRenderer TorusMesh;

    public GameObject SmallCollectEffect;
    public GameObject BigCollectEffect;
    public Fade blackCanvas;

    public GameObject gameOverText;
    public GameObject finalScoreText;

    public float fusionStart;
    private float currentFusion;
    private float currentPower;
    public float powerStart;

    private bool textBlinking = false;

    public bool dead;



    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        currentlyCombining = new List<CollectibleSmall>();
        currentFusion = fusionStart;
        currentPower = powerStart;
    }

    // Update is called once per frame
    void Update()
    {


        if (currentFusion <= 0f && !dead)
        {
            gameOver();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentFusion += 25;
        }

        if (!dead)
        {
            decreaseFusion();
            updatePower();

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
            if (combineTimer <= 0 && currentlyCombining.Count == 0)
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
                        CollectibleSmall cs1 = spawns[spawn1].GetComponent<CollectibleSmall>();
                        CollectibleSmall cs2 = spawns[spawn2].GetComponent<CollectibleSmall>();
                        //check if they are relatively close, track and angle
                        int track1 = (int)cs1.getTrackAngle() / 36;
                        int track2 = (int)cs2.getTrackAngle() / 36;
                        float trackDiff = Mathf.Abs(track1 - track2);
                        if (trackDiff > 1 && trackDiff % 2 == 0)
                        {
                            if (Mathf.Abs(cs1.getCurrentAngle() - cs2.getCurrentAngle()) <= 20f)
                            {
                                //do combine
                                float newAngle = (cs1.getCurrentAngle() + cs2.getCurrentAngle()) / 2;
                                float newTrackAngle = ((cs1.getTrackAngle() + cs2.getTrackAngle()) / 2) % 180f;
                                cs1.initiateCombine(newTrackAngle, newAngle);
                                cs2.initiateCombine(newTrackAngle, newAngle);
                                currentlyCombining.Add(cs1);
                                currentlyCombining.Add(cs2);
                                combineTimer = timeBetweenCombine;
                                break;
                            }
                        }
                    }
                    tries++;
                }
            }
            else
            {
                combineTimer -= Time.deltaTime;
            }
        }
    }


    public void updateFusion(float value)
    {
        currentFusion += value;
        //FusionValue.text = System.String.Format("{0:0.0}", currentFusion) + "%";
        if (currentFusion <= 25)
        {
            //change text color to red, blink
            FusionValue.color = Color.red;
            if (!textBlinking)
            {
                StartCoroutine("textFusionBlink");
                textBlinking = true;
            }
        }
        else
        {
            FusionValue.color = Color.green;
            if (textBlinking)
            {
                StopCoroutine("textFusionBlink");
                textBlinking = false;
            }
            FusionValue.text = System.String.Format("{0:0.0}", currentFusion) + "%";
        }
        adjustTorusIntensity();
        if (currentFusion <= 0f || dead)
        {
            currentFusion = 0f;
        }

    }

    public void decreaseFusion()
    {
        updateFusion(-Time.deltaTime * decreaseSpeed);
    }

    public void updatePower()
    {
        //increase power scaled by fusion progress
        currentPower += Time.deltaTime * powerIncrease * currentFusion / 100f;
        PowerValue.text = System.String.Format("{0:0.0}", currentPower) + "MW";
        PowerValue.color = Color.green;
    }

    private IEnumerator textFusionBlink()
    {
        while (true)
        {
            FusionValue.text = "";
            yield return new WaitForSeconds(.2f);
            FusionValue.text = System.String.Format("{0:0.0}", currentFusion) + "%";
            yield return new WaitForSeconds(.2f);
        }

    }

    public void killCollectible(GameObject go)
    {
        go.GetComponent<TorusMover>()?.despawn();
    }

    public void adjustTorusIntensity()
    {

        float H, S, V;
        Color.RGBToHSV(TorusMesh.GetComponent<Renderer>().material.GetColor("Color_2C8B9613"), out H, out S, out V);

        int maxIntensity = 100;
        S = currentFusion / maxIntensity;
        V = currentFusion / maxIntensity;
        if (S >= 1)
        {
            S = 1f;
        }
        if (V >= 1f)
        {
            V = 1f;
        }
        if (S <= 0.01)
        {
            S = 0.01f;
        }
        if (V <= 0.01)
        {
            V = 0.01f;
        }

        TorusMesh.GetComponent<Renderer>().material.SetColor("Color_2C8B9613", Color.HSVToRGB(H, S * 10, V * 10, true));
    }


    public void spawnCollectEffect(Transform t)
    {
        if (t.gameObject.GetComponent<CollectibleSmall>() != null)
        {
            GameObject go = Instantiate(SmallCollectEffect, t.position, Quaternion.identity);
            go.GetComponent<ParticleSystem>().Play();
        }
        else if (t.gameObject.GetComponent<CollectibleBig>() != null)
        {
            GameObject go = Instantiate(BigCollectEffect, t.position, Quaternion.identity);
            go.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            print("Not sure why this was called!");
        }
    }


    public void gameOver()
    {
        dead = true;
        finalScoreText.GetComponent<Text>().text = System.String.Format("{0:0.0}", currentPower) + "MW";
        StartCoroutine(blackCanvas.fade(0.99f, 1f));
        StartCoroutine(wait(1f));
        //Time.timeScale = 0f;

    }

    public IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameOverText.SetActive(true);
    }
}
