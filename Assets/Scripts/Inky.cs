using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        StartCoroutine(LevelTimer());
        StartCoroutine(WaitForStart());
    }

    // Update is called once per frame
    public override void  Update()
    {
        if (!hasStarted) return;
        castRays();
        ActorMovement();
    }

    
    public override void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        target = (Vector2)player.transform.position + targetCoords;

        base.GetRouteToTarget(pos, dirs);

    }
    public override IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(startTimer);
        
        StartCoroutine(GhostActions());
        hasStarted = true;
    }


}
