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
       HUNTING,
       SCATTER,
       FLEEING,
       EATEN,
};

public class List2D<T, U>
{
    public T item1;
    public U item2;


}