﻿using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Sheep : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(float speed)
    {
        if (!Stunned)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x - speed, pos.y, pos.z);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("geht");
    }

    protected override void Leap()
    {
        Stunned = true;
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        Debug.Log(ScreenUtil.getRightScreenBorderX(cam.GetComponent<Camera>()));
        transform.position = new Vector3();
        
        Stunned = false;
    }
}
