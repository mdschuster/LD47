using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectibleSmall : TorusMover
{

    private bool isCombining = false;
    private int combineTrack;
    private float combineAngle;

    private Vector3 end = Vector3.zero;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if (isCombining)
        {
            combine();
        }
        else
        {
            base.Update();
        }

    }

    private void combine()
    {
        Vector3 start = this.transform.position;
        rb.MovePosition(Vector3.Lerp(start, end, speed * 5 * Time.deltaTime));
        if (Mathf.Abs((start - end).magnitude) <= 0.1f)
        {
            uAngleD = combineTrack * 36;
            vAngleD = combineAngle;
            speed = 5f;
            isCombining = false;
        }
    }

    public void initiateCombine(float trackAngle, float angle)
    {
        this.combineTrack = (int)trackAngle / 36;
        this.combineAngle = angle;


        //convert from degrees to radians for cos and sin
        float uAngleR = Mathf.Deg2Rad * trackAngle;
        float vAngleR = Mathf.Deg2Rad * angle;

        //motion in game needs to be in the x/z plane
        end.x = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Cos(vAngleR);
        end.z = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Sin(vAngleR);
        end.y = innerRadius * Mathf.Sin(uAngleR);

        isCombining = true;
    }

    public bool IsCombining()
    {
        return this.isCombining;
    }
}
