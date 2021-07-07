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

    [HideInInspector]
    public bool isBoosted = false;

    Vector3 playerStartPos;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = transform.position;
        direction = Directions.STOP;
        hits = new List<Directions>();
        startPos = transform.position;
        anim = actorModel.GetComponent<Animator>();
    }

    public IEnumerator Boosted()
    {
        float timer = 7;
        float interval = 0.16667f;
        isBoosted = true;
        while (timer > 0)
        {
            yield return new WaitForSeconds(interval);
            timer -= interval;
        }
        isBoosted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted) return;
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
        ModLives();
        //transform.position = playerStartPos;
        direction = Directions.STOP;
        Velocity = Vector3.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        if (!isAlive)
        {
            anim.SetBool("isDead", true);
            anim.SetBool("isMoving", false);
        }

            GameManager.playerDead?.Invoke(lives);

    }

    public override void RestartGame(int lives)
    {
        Velocity = Vector3.zero;
        base.RestartGame(lives);
        anim.SetBool("isDead", false);
        isAlive = true;
        hasStarted = false;
        GetComponent<CircleCollider2D>().enabled = true;
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

    public override void ActorMovement(float _speed = 0)
    {
        
        if (isBoosted)
        { 
            _speed = speed + speedBoost;
        }
        else
        {
            _speed = speed;
        }

        base.ActorMovement(_speed);
    }

    void ChangePlayerOrintation()
    {
        if (direction == Directions.RIGHT && modelOrintation != Directions.RIGHT)
        {
            actorModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            modelOrintation = Directions.RIGHT;
            transform.position = new Vector2(transform.position.x, transform.position.y > 0 ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f);
        }
        else if (direction == Directions.LEFT && modelOrintation != Directions.LEFT)
        {
            actorModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            actorModel.transform.Rotate(0, 0, -180);
            modelOrintation = Directions.LEFT;
            transform.position = new Vector2(transform.position.x, transform.position.y > 0 ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f);

        }
        else if (direction == Directions.UP && modelOrintation != Directions.UP)
        {
            actorModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            actorModel.transform.Rotate(0, 0, 90);
            modelOrintation = Directions.UP;
            transform.position = new Vector2(transform.position.x > 0 ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f, transform.position.y );

        }
        else if (direction == Directions.DOWN && modelOrintation != Directions.DOWN)
        {
            actorModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            actorModel.transform.Rotate(0, 0, -90);
            modelOrintation = Directions.DOWN;
            transform.position = new Vector2(transform.position.x > 0 ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f, transform.position.y);
        }
        else if (direction == Directions.STOP && modelOrintation != Directions.STOP)
        {
            anim.SetBool("isMoving", false);
            modelOrintation = Directions.STOP;
            anim.SetBool("isMoving", false);
            transform.position = new Vector2(transform.position.x > 0 ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f, 
                                             transform.position.y > 0 ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f);

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
        if (collision.gameObject.layer == 9)
        {
            Ghost ghost = collision.gameObject.GetComponent<Ghost>();
            if (ghost.state == GhostState.CHASING || ghost.state == GhostState.SCATTER)
            {
                PlayerDeath();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer =  11;
        if (collision.gameObject.layer == layer)
        {
            collision.GetComponent<Pickup>().OnPickUp();
        }

    }

    public override IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.1f);
        hasStarted = true;
    }
}
