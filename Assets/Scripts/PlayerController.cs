using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    [SerializeField]
    int lives = 3;
    [SerializeField]
    float speedBoost = 0.2f;

    [HideInInspector]
    public bool isAlive = true;

    Vector3 playerStartPos;



    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = transform.position;
        direction = Directions.STOP;
        hits = new List<Directions>();
    }

    // Update is called once per frame
    void Update()
    {
        castRays();
        getInput();
        ActorMovement();
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
        direction = Directions.STOP;
    }

    void getInput()
    {
        if (Input.GetAxis("Horizontal") > 0) 
        {
            nextDirection = Directions.RIGHT;
        }
        else if (Input.GetAxis("Horizontal") < 0) 
        {
            nextDirection = Directions.LEFT;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            nextDirection = Directions.UP;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            nextDirection = Directions.DOWN;
        }
    }

    public override void ActorMovement()
    {
        if (Velocity.magnitude == 0)
        {
            direction = Directions.STOP;
        }
        bool nextDirClear = true;
        foreach(Directions pd in hits)
        {
            if (pd == nextDirection)
            { 
                nextDirClear = false;
                continue;
            }
        }

        if (nextDirClear)
            direction = nextDirection;

        if (direction == Directions.LEFT)
        {
            Velocity = Vector3.left * speed * Time.deltaTime;
        }
        else if (direction == Directions.RIGHT)
        {
            Velocity = Vector3.right * speed * Time.deltaTime;
        }
        else if (direction == Directions.UP)
        {
            Velocity = Vector3.up * speed * Time.deltaTime;
        }
        else if (direction == Directions.DOWN)
        {
            Velocity = Vector3.down * speed * Time.deltaTime;
        }
        else if (direction == Directions.STOP)
        {
            Velocity = Vector3.zero;
        }
        if (Velocity.magnitude > 0)
            playerModel.GetComponent<Animator>().SetBool("isMoving", true);
        else
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);


        transform.Translate(Velocity);

    }

    void ChangePlayerOrintation()
    {
        if (direction == Directions.RIGHT && modelOrintation != Directions.RIGHT)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            modelOrintation = Directions.RIGHT;
        }
        else if (direction == Directions.LEFT && modelOrintation != Directions.LEFT)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, -180);
            modelOrintation = Directions.LEFT;
        }
        else if (direction == Directions.UP && modelOrintation != Directions.UP)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, 90);
            modelOrintation = Directions.UP;
        }
        else if (direction == Directions.DOWN && modelOrintation != Directions.DOWN)
        {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            playerModel.transform.Rotate(0, 0, -90);
            modelOrintation = Directions.DOWN;
        }
        else if (direction == Directions.STOP && modelOrintation != Directions.STOP)
        {
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);
            modelOrintation = Directions.STOP;
            playerModel.GetComponent<Animator>().SetBool("isMoving", false);

        }


    }

    public override void ChangeDirection(Directions _direction)
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
