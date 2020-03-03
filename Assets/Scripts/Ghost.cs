using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ghost : Actor
{
    [HideInInspector]
    public Vector2 target = new Vector2();
    public Vector2 scatterTarget;
    public Vector2 ghostHouse = Vector2.zero;

    public GameObject player;
    [Header("The coordinates of the target based on the players Position")]
    public Vector2 targetCoords;
    [HideInInspector]
    public List<Directions> routesToTarget;

    public GhostState state;
    // public class to get target
    [HideInInspector]
    public float timer = 0f;
    [HideInInspector]
    public float fleeTimer = 7f;

    public virtual IEnumerator FleeTarget()
    {
        if (state == GhostState.FLEEING)
        {
            while (true)
            {
                target = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                yield return new WaitForSeconds(Random.Range(0, 8));
                FleeTarget();
                if (state != GhostState.FLEEING)
                {
                    StopCoroutine(FleeTarget());
                    continue;
                };
            }
        }
        yield return null;
    }
    public virtual IEnumerator LevelTimer()
    {
        float initFleeTimer = fleeTimer;
        Animator anim = GetComponent<Animator>();
        float interval = 0.1667f;
        bool levelRunning = true;
        while (levelRunning)
        {
            if (state != GhostState.FLEEING && state != GhostState.EATEN)
            {
                yield return new WaitForSeconds(interval);
                timer += interval;
            }
            else if (state == GhostState.FLEEING)
            {
                yield return new WaitForSeconds(interval);
                fleeTimer -= 0.1667f;
                anim.SetFloat("FleeTimer", fleeTimer);

                if (fleeTimer < 0)
                {
                    fleeTimer = initFleeTimer;
                    anim.SetFloat("FleeTimer", initFleeTimer);
                    anim.SetBool("IsFleeing", false);
                    state = GhostState.SCATTER;
                }
            }
            else if (state == GhostState.EATEN)
            {
                yield return new WaitForSeconds(interval);
                if (fleeTimer != initFleeTimer)
                {
                    fleeTimer = initFleeTimer;
                }
            }
            // Check game state to check if the level has ended.
        }
        //  yield return null;
    }
    public virtual IEnumerator GhostActions()
    {
        GhostState lastState = state;
        while (true)
        {

            if ((this.state != GhostState.FLEEING) && (this.state != GhostState.EATEN))
            {
                if (timer < 20)
                    this.state = GhostState.CHASING;
                else if (timer > 20 && timer < 28) this.state = GhostState.SCATTER;
                else if (timer > 28 && timer < 48) this.state = GhostState.CHASING;
                else if (timer < 48 && timer < 55) this.state = GhostState.SCATTER;
                else if (timer > 55 && timer < 75) this.state = GhostState.CHASING;
                else if (timer > 75 && timer < 80) this.state = GhostState.SCATTER;
                else if (timer > 80 && timer < 100) this.state = GhostState.CHASING;
                else if (timer > 100 && timer < 105) this.state = GhostState.SCATTER;
                else if (timer > 105) this.state = GhostState.CHASING;
            }
            // Check game state to check if the level has ended.

            if (lastState != state)
            {
                lastState = state;
                unityEvent.Invoke();
            }
            yield return new WaitForSeconds(0.1667f);
        }
    }

    public virtual void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        if (state == GhostState.SCATTER )
            target = scatterTarget;
        else if (state == GhostState.FLEEING)
        {
            StartCoroutine(FleeTarget());
        }
        else if (state == GhostState.EATEN)
            target = ghostHouse;

        if (dirs != null) routesToTarget = dirs;
        else
        {
            routesToTarget = new List<Directions>();
        }
        switch (direction)
        {
            case (Directions.LEFT):
                if (state != GhostState.FLEEING )
                {
                    routesToTarget.Remove(Directions.RIGHT);
                }
                break;
            case (Directions.RIGHT):
                if (state != GhostState.FLEEING )
                {
                    routesToTarget.Remove(Directions.LEFT);
                }
                break;
            case (Directions.UP):
                if (state != GhostState.FLEEING )
                {
                    routesToTarget.Remove(Directions.DOWN);
                }
                break;
            case (Directions.DOWN):
                if (state != GhostState.FLEEING )
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
                        Vector2 testPos = pos + Vector2.left;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.RIGHT):
                    {
                        Vector2 testPos = pos + Vector2.right;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.UP):
                    {
                        Vector2 testPos = pos + Vector2.up;
                        dirTest.Add(dir, Vector2.Distance(testPos, target));
                        break;
                    }
                case (Directions.DOWN):
                    {
                        Vector2 testPos = pos + Vector2.down;
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
            GetRouteToTarget((Vector2)transform.position, other.gameObject.GetComponent<triggerDir>().directions);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 )
        {
            if(state == GhostState.FLEEING)
            {
                state = GhostState.EATEN;
            }
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
    public virtual void Update()
    {

    }

    public virtual void OnApplicationQuit()
    {
        unityEvent.RemoveListener(OnStateChange);
    }
    public virtual void Start()
    {
        unityEvent.AddListener(OnStateChange);
    }
    // Event system to check if the state has changed
    public virtual void OnStateChange()
    {
        Animator anim = this.GetComponent<Animator>();

        if (state == GhostState.FLEEING)
        {
            anim.SetBool("IsFleeing", true);
            switch (direction)
            {
                case (Directions.LEFT):
                    anim.SetBool("moveL", false);
                    direction = Directions.RIGHT;
                    break;
                case (Directions.RIGHT):
                    anim.SetBool("moveR", false);
                    direction = Directions.LEFT;
                    break;
                case (Directions.UP):
                    anim.SetBool("moveU", false);
                    direction = Directions.DOWN;
                    break;
                case (Directions.DOWN):
                    anim.SetBool("moveD", false);
                    direction = Directions.UP;
                    break;
                default:
                    break;
            }

        }
        if (state == GhostState.EATEN)
        {
            anim.SetBool("IsEaten", true);
            anim.SetBool("IsFleeing", false);

            switch (direction)
            {
                case (Directions.LEFT):
                    direction = Directions.RIGHT;
                    break;
                case (Directions.RIGHT):
                    direction = Directions.LEFT;
                    break;
                case (Directions.UP):
                    direction = Directions.DOWN;
                    break;
                case (Directions.DOWN):
                    direction = Directions.UP;
                    break;
                default:
                    break;
            }

        }

    }

}