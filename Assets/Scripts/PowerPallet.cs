using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPallet : Pickup
{
    public override void OnPickUp()
    {
        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        foreach(Ghost g in ghosts)
        {
            if (g.state != GhostState.EATEN) {
                g.state = GhostState.FLEEING;
                g.fleeTimer = 7;
            }
        }
        PlayerController player = FindObjectOfType<PlayerController>();
        StartCoroutine(player.Boosted());
               base.OnPickUp();

    }
}
