using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private List<GameObject> collectables;

    public GameObject Collectable;

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
        collectables = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        spawn();
    }

    public void spawn()
    {

        if (spawnTimer <= 0)
        {
            track = Random.Range(0, 10);
            float uAngleD = track * 36;
            float speed = Random.Range(minSpeed, maxSpeed);
            float vAngleD = initialAngle;

            GameObject go = Instantiate(Collectable, new Vector3(0f, 0f, 0f), Quaternion.identity);
            TorusMover tm = go.GetComponent<TorusMover>();
            tm.uAngleD = uAngleD;
            tm.speed = speed;
            tm.vAngleD = vAngleD;
            tm.torusRadius = torusRadius;
            tm.innerRadius = innerRadius;
            tm.killAngle = killAngle;

            go.GetComponent<TorusMover>().kill += removeCollectable;
            collectables.Add(go);

            spawnTimer = timeBetweenSpawns;

        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void removeCollectable(GameObject go)
    {
        collectables.Remove(go);
    }
}
