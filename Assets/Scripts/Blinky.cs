﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Target();
    }
    
    private void Target()
    {
        target = Player.transform.position;
    }
}