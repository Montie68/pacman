using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPallet : Pickup
{
    public override void OnPickUp()
    {
        base.OnPickUp();
        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        foreach(Ghost g in ghosts)
        {
            g.state = GhostState.FLEEING;
        }
        Destroy(this.gameObject);
    }
}
