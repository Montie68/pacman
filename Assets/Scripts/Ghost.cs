using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : Actor
{
    [HideInInspector]
    public Vector2 target = new Vector2();
    public GameObject Player;
    [Header("The coorinates of the target based on the players Position")]
    public Vector2 targetCoords;
    [HideInInspector]
    public List<Directions> routesToTarget;

    public GhostState state;
    // public class to get target
    public virtual void GetRouteToTarget(List<Directions> dirs = null) 
    {
        if (dirs != null) routesToTarget = dirs;
        else
        {
            routesToTarget = new List<Directions>();
        }
        switch (direction)
        {
            case (Directions.LEFT):
                if (state != GhostState.FLEEING)
                {
                    routesToTarget.Remove(Directions.RIGHT);
                }
                break;
            case (Directions.RIGHT):
                if (state != GhostState.FLEEING)
                {
                    routesToTarget.Remove(Directions.LEFT);
                }
                break;
            case (Directions.UP):
                if (state != GhostState.FLEEING)
                {
                    routesToTarget.Remove(Directions.DOWN);
                }
                break;
            case (Directions.DOWN):
                if (state != GhostState.FLEEING)
                {
                    routesToTarget.Remove(Directions.UP);
                }
                break;

            case (Directions.STOP):
            default:
                break;
        }

        Dictionary<Directions, float> dirTest = new Dictionary<Directions, float>();
        foreach (Directions dir in routesToTarget)
        {
            switch (dir)
            {
                case (Directions.LEFT):
                    {
                        Vector2 testPos = (Vector2)transform.position + Vector2.left;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.RIGHT):
                    {
                        Vector2 testPos = (Vector2)transform.position + Vector2.right;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.UP):
                    {
                        Vector2 testPos = (Vector2)transform.position + Vector2.up;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.DOWN):
                    {
                        Vector2 testPos = (Vector2)transform.position + Vector2.down;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                default:
                    break;
            }
        }
        float pathLenght = 30000;

        foreach (KeyValuePair<Directions, float> lst in dirTest)
        {
            if (lst.Value < pathLenght)
            {
                pathLenght = lst.Value;
                nextDirection = lst.Key;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12)
        {
            GetRouteToTarget(other.gameObject.GetComponent<triggerDir>().directions);
        }
    }

    public override void ActorMovement()
    {
        base.ActorMovement();
        Animator anim = this.GetComponent<Animator>();
        if (direction != lastDirection && state != GhostState.FLEEING)
        {
            switch (lastDirection)
            {
                case (Directions.LEFT):
                    anim.SetBool("moveL", false);
                    break;
                case (Directions.RIGHT):
                    anim.SetBool("moveR", false);
                    break;
                case (Directions.UP):
                    anim.SetBool("moveU", false);
                    break;
                case (Directions.DOWN):
                    anim.SetBool("moveD", false);
                    break;
                default:
                    break;
            }

            switch (direction)
            {
                case (Directions.LEFT):
                    anim.SetBool("moveL", true);
                    break;
                case (Directions.RIGHT):
                    anim.SetBool("moveR", true);
                    break;
                case (Directions.UP):
                    anim.SetBool("moveU", true);
                    break;
                case (Directions.DOWN):
                    anim.SetBool("moveD", true);
                    break;
                default:
                    break;
            }
        }
    }
}
