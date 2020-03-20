using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
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
        if (state == GhostState.CHASING)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
                
                if (distance > targetCoords.x)
                    target = (Vector2)player.transform.position + targetCoords;
                else
                    target = scatterTarget;
        }
        base.GetRouteToTarget(pos, dirs);

    }
    public override IEnumerator WaitForStart()
    {
        float palletCount = palletCounter.instance.palletCount*startTimer;
        int test = (int)palletCount;
        while(palletCounter.instance.palletCount > (palletCount))
        {
            yield return new WaitForSeconds(1 / 15);
        }
        
        StartCoroutine(GhostActions());
        hasStarted = true;
    }


}