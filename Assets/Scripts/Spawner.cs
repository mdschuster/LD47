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

public class Spawner : MonoBehaviour
{

    private List<GameObject> collectibles;

    private IFactory factory;

    public float minSpeed;
    public float maxSpeed;

    public float initialAngle;

    public float torusRadius;
    public float innerRadius;
    public float killAngle;

    private int track;

    //spawner timer stuff
    private float spawnTimer = 0;
    public float timeBetweenSpawns;




    // Start is called before the first frame update
    void Start()
    {
        collectibles = new List<GameObject>();
        factory = GetComponent<CollectibleFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance().dead)
        {
            spawn();
        }
    }

    public void spawn()
    {

        if (spawnTimer <= 0)
        {
            track = Random.Range(0, 10);
            float uAngleD = track * 36;
            float speed = Random.Range(minSpeed, maxSpeed);
            float vAngleD = initialAngle;


            GameObject product = ((TorusMover)factory.produce()).gameObject;


            GameObject go = Instantiate(product, new Vector3(0f, 0f, 0f), Quaternion.identity);
            TorusMover tm = go.GetComponent<TorusMover>();
            tm.uAngleD = uAngleD;
            tm.speed = speed;
            tm.vAngleD = vAngleD;
            tm.torusRadius = torusRadius;
            tm.innerRadius = innerRadius;
            tm.killAngle = killAngle;

            go.GetComponent<TorusMover>().kill += removeCollectible;
            collectibles.Add(go);

            spawnTimer = timeBetweenSpawns;

        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void removeCollectible(GameObject go)
    {
        collectibles.Remove(go);
    }

    public List<GameObject> getCollectibles()
    {
        return collectibles;
    }

    public void manualSpawn(float uAngleD, float vAngleD, int type)
    {
        float speed = Random.Range(minSpeed, maxSpeed);


        GameObject product = ((TorusMover)factory.produceManual(type)).gameObject;


        GameObject go = Instantiate(product, new Vector3(0f, 0f, 0f), Quaternion.identity);
        TorusMover tm = go.GetComponent<TorusMover>();
        tm.uAngleD = uAngleD;
        tm.speed = speed;
        tm.vAngleD = vAngleD;
        tm.torusRadius = torusRadius;
        tm.innerRadius = innerRadius;
        tm.killAngle = killAngle;
        tm.updateOffset();

        go.GetComponent<TorusMover>().kill += removeCollectible;
        collectibles.Add(go);
    }

    public void increaseDiff()
    {
        timeBetweenSpawns /= 2;
        minSpeed += 2;
        maxSpeed += 2;
    }
}
