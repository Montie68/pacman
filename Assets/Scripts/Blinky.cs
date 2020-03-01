using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelTimer());
        castRays();
        StartCoroutine(GhostActions());

    }

    // Update is called once per frame
    public override void  Update()
    {
        castRays();
        ActorMovement();
    }

    
    public override void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        target = (Vector2)player.transform.position + targetCoords;

        base.GetRouteToTarget(pos, dirs);

    }

    public override IEnumerator GhostActions()
    {
        while(true)
        {
            if ((this.state != GhostState.FLEEING) && (this.state != GhostState.EATEN))
            {
                if (timer < 20)
                    this.state = GhostState.CHASING;
                else if (timer > 20 && timer < 28) this.state = GhostState.SCATTER;
                else if (timer > 28 && timer < 48) this.state = GhostState.CHASING;
                else if (timer < 48 && timer < 55) this.state = GhostState.SCATTER;
                else if (timer > 55 && timer < 75) this.state = GhostState.CHASING;
                else if (timer > 75 && timer < 80) this.state = GhostState.SCATTER;
                else if (timer > 80 && timer < 100) this.state = GhostState.CHASING;
                else if (timer > 100 && timer < 105) this.state = GhostState.SCATTER;
                else if (timer > 105) this.state = GhostState.CHASING;
            }
            // Check game state to check if the level has ended.
            yield return new WaitForSeconds(0.3333f);
        }
       // yield return null;
    }


}
