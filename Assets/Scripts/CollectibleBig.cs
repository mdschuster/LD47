using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBig : TorusMover
{

    private Vector3 vec1;
    private Vector3 vec2;

    private float angle;
    private float angle1;
    private float angle2;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        angle = 0;
        angle1 = Random.Range(0f, 180f);
        angle2 = Random.Range(0f, 180f);
    }

    // Update is called once per frame
    new void Update()
    {

        base.Update();
        updateRotation();
    }

    public void updateRotation()
    {
        transform.rotation = Quaternion.Euler(angle1, angle, angle2);
        angle += Time.deltaTime * 500;
    }

}
