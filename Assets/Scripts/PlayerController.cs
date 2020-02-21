using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerDirection
{
    STOP = 0,
        LEFT,
        RIGHT,
        UP,
        DOWN
};
public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField]
    int lives = 3;
    [SerializeField]
    float speedBoost = 0.2f;
    [SerializeField]
    float raylength = 0.5f;

    Vector3 Velocity = Vector3.zero;
    [HideInInspector]
    public bool isAlive = true;
    [SerializeField]
    playerDirection direction;
    [SerializeField]
    playerDirection nextDirection;
    playerDirection lastDirection;
    // List of Raycats Hits
    [SerializeField]
    List<playerDirection> hits;

    Vector3 playerStartPos;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = transform.position;
        direction = playerDirection.STOP;
        hits = new List<playerDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        castRays();
        getInput();
        playerMovement();
    }


    public void ModLives(int life = -1)
    {
        lives += life;
        if (lives >= 0 && isAlive)
            isAlive = false;
    }

    public void PlayerDeath()
    {
        transform.position = playerStartPos;
        direction = playerDirection.STOP;
    }

    void getInput()
    {
        if (Input.GetAxis("Horizontal") > 0) 
        {
            nextDirection = playerDirection.RIGHT;
        }
        else if (Input.GetAxis("Horizontal") < 0) 
        {
            nextDirection = playerDirection.LEFT;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            nextDirection = playerDirection.UP;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            nextDirection = playerDirection.DOWN;
        }
    }

    void playerMovement()
    {
        if (Velocity == Vector3.zero)
        {
            direction = playerDirection.STOP;
            
        }
        
        bool nextDirClear = true;
        foreach(playerDirection pd in hits)
        {
            if (pd == nextDirection)
            { 
                nextDirClear = false;
                continue;
            }
        }

        if (nextDirClear)
            direction = nextDirection;

        if (direction == playerDirection.LEFT)
        {
            Velocity = Vector3.left * speed * Time.deltaTime;
        }
        else if (direction == playerDirection.RIGHT)
        {
            Velocity = Vector3.right * speed * Time.deltaTime;
        }
        else if (direction == playerDirection.UP)
        {
            Velocity = Vector3.up * speed * Time.deltaTime;
        }
        else if (direction == playerDirection.DOWN)
        {
            Velocity = Vector3.down * speed * Time.deltaTime;
       
        }

        transform.Translate(Velocity);

    }
    void castRays()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        hits = new List<playerDirection>();

        Vector3 pos = transform.position;

        if (Physics2D.Raycast(pos , transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * 0.4f), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * 0.4f), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * 0.2f), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * 0.2f), transform.TransformDirection(Vector3.left), raylength, layerMask)  )
        {
            hits.Add(playerDirection.LEFT);

        }
        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * 0.4f), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * 0.4f), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.up * 0.2f), transform.TransformDirection(Vector3.right), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.down * 0.2f), transform.TransformDirection(Vector3.right), raylength, layerMask))
            hits.Add(playerDirection.RIGHT);
        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.4f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.4f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.2f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.2f), transform.TransformDirection(Vector3.up), raylength, layerMask))
            hits.Add(playerDirection.UP);
        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.4f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.4f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.2f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.2f), transform.TransformDirection(Vector3.down), raylength, layerMask))

            hits.Add(playerDirection.DOWN);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Walls")
        {
            Velocity = Vector3.zero;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        int layer = 1 << 11;
        if (other.gameObject.layer == layer)
        {
            other.GetComponent<Pickup>().OnPickUp(0);
        }

    }
}
