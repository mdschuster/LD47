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
            print("Left/Counter Clockwise");
            move(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print("Right/Clockwise");
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
        print(moveVector);
        rb.MovePosition(moveVector);

        //update angles

        //vAngleD += speed * Time.deltaTime;

    }
}
