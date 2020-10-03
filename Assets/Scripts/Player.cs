using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public float speed;
    public float torusRadius;
    public float innerRadius;
    public float vAngleD;
    public float uAngleD;

    private Rigidbody rb;

    private Vector3 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveVector = Vector3.zero;
        move(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Counter Clockwise
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(-1);
        }
        //Clockwise
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(1);
        }
    }

    private void move(int direction)
    {

        uAngleD += direction * 36;

        //convert from degrees to radians for cos and sin
        float uAngleR = Mathf.Deg2Rad * uAngleD;
        float vAngleR = Mathf.Deg2Rad * vAngleD;


        //motion in game needs to be in the x/z plane
        moveVector.x = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Cos(vAngleR);
        moveVector.z = (torusRadius + innerRadius * Mathf.Cos(uAngleR)) * Mathf.Sin(vAngleR);
        moveVector.y = innerRadius * Mathf.Sin(uAngleR);
        rb.MovePosition(moveVector);


    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.transform != null && other.transform.parent.tag == "Collectible")
        {
            //TODO Mess with score/collectible count here
        }
    }
}
