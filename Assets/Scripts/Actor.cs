using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public bool isTeleporting = false;
    public virtual void ChangeDirection(playerDirection _direction)
    {
    }
}
