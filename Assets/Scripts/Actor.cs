using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Actor : MonoBehaviour
{
    public float raylength = 0.8f;
    public float FrontRaylength = 0.5f;
    public float speed = 2f;
    [HideInInspector]
    public UnityEvent unityEvent = new UnityEvent();
    [HideInInspector]
    public bool isTeleporting = false;

    // List of Raycats Hits
   // [HideInInspector]
    public List<Directions> hits;
    public GameObject actorModel;

   // [HideInInspector]
    public Directions direction;
    [HideInInspector]
    public Directions nextDirection;
    [HideInInspector]
    public Directions lastDirection;
    [HideInInspector]
    public Directions modelOrintation;
    [HideInInspector]
    public Vector3 Velocity = Vector3.zero;
    public float[] rayPoints = new float[2] {0.2f, 0.4f };
    [HideInInspector]
    public Vector3 startPos;

    [HideInInspector]
    public bool hasStarted = false;

    public virtual void ChangeDirection(Directions _direction)
    {
    }
    public virtual void StartGame() { }

    public virtual void castRays(int _layerMask = -1)
    {

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        if (_layerMask == -1)
            layerMask = 1 << 8;
        else
            layerMask = 1 << _layerMask;

            hits = new List<Directions>();

        Vector3 pos = transform.position;

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * rayPoints[1]), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * rayPoints[1]), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up *  rayPoints[0]), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down *  rayPoints[0]), transform.TransformDirection(Vector3.left), raylength, layerMask))
        {
            hits.Add(Directions.LEFT);
        }

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * rayPoints[1]), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * rayPoints[1]), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up *  rayPoints[0]), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down *  rayPoints[0]), transform.TransformDirection(Vector3.right), raylength, layerMask))
        {
            hits.Add(Directions.RIGHT);
        }

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left *  rayPoints[1]), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right *  rayPoints[1]), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left *  rayPoints[0]), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right *  rayPoints[0]), transform.TransformDirection(Vector3.up), raylength, layerMask))
        {
            hits.Add(Directions.UP);
        }

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left *  rayPoints[1]), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right *  rayPoints[1]), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left *  rayPoints[0]), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right *  rayPoints[0]), transform.TransformDirection(Vector3.down), raylength, layerMask))
        {
            hits.Add(Directions.DOWN);
        }
        if (FrontRaylength == 0) return;
        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.left), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.up), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.right), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.down), FrontRaylength, layerMask)
            && nextDirection == direction)
        {
            nextDirection = Directions.STOP;
        }

    }
    public virtual void ActorMovement(float _speed = 0)
    {
        if (Velocity.magnitude == 0)
        {
            direction = Directions.STOP;
        }
        bool nextDirClear = true;
        foreach (Directions pd in hits)
        {
            if (pd == nextDirection)
            {
                nextDirClear = false;
                continue;
            }
        }

        if (nextDirClear)
        {
            lastDirection = direction;
            direction = nextDirection;
        }

        if (direction == Directions.LEFT)
        {
            Velocity = Vector3.left * _speed * Time.deltaTime;
        }
        else if (direction == Directions.RIGHT)
        {
            Velocity = Vector3.right * _speed * Time.deltaTime;
        }
        else if (direction == Directions.UP)
        {
            Velocity = Vector3.up * _speed * Time.deltaTime;
        }
        else if (direction == Directions.DOWN)
        {
            Velocity = Vector3.down * _speed * Time.deltaTime;
        }
        else if (direction == Directions.STOP)
        {
            Velocity = Vector3.zero;
        }
        if (Velocity.magnitude > 0)
            actorModel.GetComponent<Animator>().SetBool("isMoving", true);
        else
            actorModel.GetComponent<Animator>().SetBool("isMoving", false);


        transform.Translate(Velocity);

    }
    public virtual IEnumerator WaitForStart()
    {
        yield return null;
    }

}
