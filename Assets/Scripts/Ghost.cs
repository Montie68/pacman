using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : Actor
{
    [HideInInspector]
   public Vector3 target = new Vector3();
   public GameObject Player;

}
