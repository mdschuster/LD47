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

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public float speed;
    public float torusRadius;
    public float innerRadius;
    public float vAngleD;
    public float uAngleD;

    private Rigidbody rb;

    private GameManager gm;

    private Vector3 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance();
        rb = GetComponent<Rigidbody>();
        moveVector = Vector3.zero;
        move(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.dead)
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
            gm.updateFusion(other.transform.parent.GetComponent<TorusMover>().worth);
            gm.spawnCollectEffect(other.transform.parent);
            gm.killCollectible(other.transform.parent.gameObject);
        }
    }
}
