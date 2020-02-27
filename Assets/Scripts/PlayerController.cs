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
public class PlayerController : Actor
{
    public float speed = 2f;
    [SerializeField]
    int lives = 3;
    [SerializeField]
    float speedBoost = 0.2f;
    [SerializeField]
    float raylength = 0.5f;
    [SerializeField]
    float FrontRaylength = 0.5f;
    public GameObject playerModel;

    [HideInInspector]
    public bool isAlive = true;

    Vector3 Velocity = Vector3.zero;
    Vector3 playerStartPos;

    playerDirection direction;
    playerDirection nextDirection;
    playerDirection lastDirection;
    playerDirection modelOrintation;

    // List of Raycats Hits
    List<playerDirection> hits;


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
        ChangePlayerOrintation();
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
        if (Velocity.magnitude == 0)
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
        else if (direction == playerDirection.STOP)
        {
            Velocity = Vector3.zero;
        }
        if (Velocity.magnitude > 0)
            playerModel.GetComponent<Animator>().SetBool("isMoving", true);
        else
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);


        transform.Translate(Velocity);

    }
    void castRays()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        hits = new List<playerDirection>();

        Vector3 pos = transform.position;

        if ( Physics2D.Raycast(pos + (Vector3.up * 0.4f), transform.TransformDirection(Vector3.left), raylength, layerMask) ||
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
        {
            hits.Add(playerDirection.RIGHT);
        }

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.4f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.4f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.2f), transform.TransformDirection(Vector3.up), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.2f), transform.TransformDirection(Vector3.up), raylength, layerMask))
        {
            hits.Add(playerDirection.UP); 
        }

        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.4f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.4f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.left * 0.2f), transform.TransformDirection(Vector3.down), raylength, layerMask) ||
            Physics2D.Raycast(pos + (Vector3.right * 0.2f), transform.TransformDirection(Vector3.down), raylength, layerMask))
        { 
            hits.Add(playerDirection.DOWN);
        }
        
        if (Physics2D.Raycast(pos, transform.TransformDirection(Vector3.left), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.up), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.right), FrontRaylength, layerMask) ||
            Physics2D.Raycast(pos, transform.TransformDirection(Vector3.down), FrontRaylength, layerMask)
            && nextDirection == direction)
        {
            nextDirection = playerDirection.STOP;
        }

    }

    void ChangePlayerOrintation()
    {
        if (direction == playerDirection.RIGHT && modelOrintation != playerDirection.RIGHT)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            modelOrintation = playerDirection.RIGHT;
        }
        else if (direction == playerDirection.LEFT && modelOrintation != playerDirection.LEFT)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, -180);
            modelOrintation = playerDirection.LEFT;
        }
        else if (direction == playerDirection.UP && modelOrintation != playerDirection.UP)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, 90);
            modelOrintation = playerDirection.UP;
        }
        else if (direction == playerDirection.DOWN && modelOrintation != playerDirection.DOWN)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, -90);
            modelOrintation = playerDirection.DOWN;
        }
        else if (direction == playerDirection.STOP && modelOrintation != playerDirection.STOP)
        {
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);
            modelOrintation = playerDirection.STOP;
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);

        }


    }

    public override void ChangeDirection(playerDirection _direction)
    {
        direction = _direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Walls")
        {
            Velocity = Vector3.zero;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer =  11;
        if (collision.gameObject.layer == layer)
        {
            collision.GetComponent<Pickup>().OnPickUp(0);
        }
    }

}
