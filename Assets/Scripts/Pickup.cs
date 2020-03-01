using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // The Points to add the the players score.
    
    public int points = 1;
    public virtual void OnPickUp()
    {}

}
