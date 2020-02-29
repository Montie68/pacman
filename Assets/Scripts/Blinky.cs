using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    // Start is called before the first frame update
    void Start()
    {
        castRays();
        GetRouteToTarget((Vector2)transform.position, new List<Directions>() {Directions.LEFT, Directions.RIGHT } );

    }

    // Update is called once per frame
    void Update()
    {
        castRays();
        ActorMovement();
    }

    
    public override void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        target = (Vector2)player.transform.position + targetCoords;

        base.GetRouteToTarget(pos, dirs);

    }
}
