using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum playerDirection
    {
        STOP = 0,
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    public float speed = 2f;
    [SerializeField]
    int lives = 3;
    [SerializeField]
    float speedBoost = 0.2f;
    Vector3 Velocity = Vector3.zero;
    [HideInInspector]
    public bool isAlive = true;
    playerDirection direction;
    [SerializeField]
    playerDirection nextDirection;
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

    void castRays()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        hits = new List<playerDirection>();

        if (Physics2D.Raycast(transform.TransformDirection(Vector3.left), transform.TransformDirection(Vector3.left), 1.5f, layerMask))
            hits.Add(playerDirection.LEFT);
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 1.5f, layerMask))
            hits.Add(playerDirection.RIGHT);
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), 1.5f, layerMask))
            hits.Add(playerDirection.UP);
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.5f, layerMask))
            hits.Add(playerDirection.DOWN);
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
            Velocity.y = (int)Velocity.y;
        }
        else if (direction == playerDirection.RIGHT)
        {
            Velocity = Vector3.right * speed * Time.deltaTime;
            Velocity.y = (int)Velocity.y;
        }
        else if (direction == playerDirection.UP)
        {
            Velocity = Vector3.up * speed * Time.deltaTime;
            Velocity.x = (int)Velocity.x;
        }
        else if (direction == playerDirection.DOWN)
        {
            Velocity = Vector3.down * speed * Time.deltaTime;
            Velocity.x = (int)Velocity.x;
       
        }

        transform.Translate(Velocity);
        transform.localRotation = Quaternion.identity;

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
        if (other.gameObject.tag == "Pickup")
        {
            other.GetComponent<Pickup>().OnPickUp(0);
        }
    }
}
