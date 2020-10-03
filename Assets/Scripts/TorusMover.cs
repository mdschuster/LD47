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

public abstract class TorusMover : MonoBehaviour, IProduct
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

    public float killAngle { get; set; }

    public float offsetAmt;


    public int worth;

    protected Rigidbody rb;

    protected Vector3 moveVector;

    public System.Action<GameObject> kill;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveVector = Vector3.zero;
        updateOffset();
    }

    // Update is called once per frame
    protected void Update()
    {
        move();
        if (vAngleD >= killAngle)
        {
            despawn();
        }
    }

    protected void move()
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

    public void despawn()
    {
        kill?.Invoke(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }

    public float getTrackAngle()
    {
        return uAngleD;
    }

    public float getCurrentAngle()
    {
        return vAngleD;
    }

    public void updateOffset()
    {
        innerRadius -= offsetAmt;
    }
}
