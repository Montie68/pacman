using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : Pickup
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPickUp()
    {
        // if player add points to the Game manger.
        base.OnPickUp();
        palletCounter.instance.palletEaten.Invoke(this);
        // Strech goal add test if player 1 or 2 and add score to the correct player

        // ------------- remove after implemnetation --------------------- //
        // Debug.Log("Yummy! Points Added:" + points);
    }
}
