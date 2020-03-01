using UnityEngine;
using System.Collections;
public enum Directions
{
    STOP = 0,
    LEFT,
    RIGHT,
    UP,
    DOWN
};
public enum GhostState
{
       CHASING,
       SCATTER,
       FLEEING,
       EATEN,
};

