﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void StartGame() {
        StartCoroutine(LevelTimer());
        castRays();
        StartCoroutine(GhostActions());
    }
    // Update is called once per frame
    public override void  Update()
    {
        if (GameManager.main.isStarted)
        {
            if (!hasStarted) return;
            castRays();
            ActorMovement();
        }
    }

    
    public override void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        if (state == GhostState.CHASING) 
        target = (Vector2)player.transform.position + targetCoords;

        base.GetRouteToTarget(pos, dirs);

    }

    public override IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(startTimer);

        hasStarted = true;
    }
}
