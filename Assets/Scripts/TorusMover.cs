using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TorusMover : MonoBehaviour
{

    /*
        Torus movement, parametric form:
        x(u,v) = (R+rcos(u))cos(v)
        y(u,v) = (R+rsin(u))sin(v)
        z(u,v) = rsin(u)
        R=full radius
        r=torus radius
        u=angle
        v=angle
    */

    public float speed { get; set; }
    public float torusRadius { get; set; }
    public float innerRadius { get; set; }
    public float vAngleD { get; set; }
    public float uAngleD { get; set; }

    private Rigidbody rb;

    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveVector = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {

        //convert from degrees to radians for cos and sin
        float uAngleR = Mathf.Deg2Rad * uAngleD;
        float vAngleR = Mathf.Deg2Rad * vAngleD;


        //motion in game needs to be in the x/z plane
        moveVector.x = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Cos(vAngleR);
        moveVector.z = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Sin(vAngleR);
        moveVector.y = innerRadius * Mathf.Sin(uAngleR);
        rb.MovePosition(moveVector);

        //update angles
        //uAngleD += speed * Time.deltaTime;
        vAngleD += speed * Time.deltaTime;

    }
}
