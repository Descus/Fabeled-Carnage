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
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x - speed, pos.y, pos.z);
    }
}
