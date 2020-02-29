using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    // Start is called before the first frame update
    void Start()
    {
        castRays();
        GetRouteToTarget(new List<Directions>() {Directions.LEFT, Directions.RIGHT });

    }

    // Update is called once per frame
    void Update()
    {
        castRays();
        ActorMovement();
    }

    
    public override void GetRouteToTarget(List<Directions> dirs = null)
    {
        target = (Vector2)Player.transform.position + targetCoords;

        base.GetRouteToTarget(dirs);

    }
}
