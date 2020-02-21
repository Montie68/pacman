using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : Pickup
{
    // The Points to add the the players score.
    [SerializeField]
    int points = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPickUp(int ID = 0)
    {
        base.OnPickUp(ID);
        // Test if the collider is the player
        // if player add points to the Game manger.

        // Strech goal add test if player 1 or 2 and add score to the correct player\

        // ------------- remove after implemnetation --------------------- //
        Debug.Log("Yummy! Points Added:" + points);
        Destroy(this.gameObject);
    }
}
