using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    [SerializeField]
    int lives = 3;

    Vector3 playerStartPos;
    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
    }

    public void ModLives(int life = -1)
    {
        lives += life;
    }

    public void PlayerDeath()
    {
        transform.position = playerStartPos;
    }

    void playerMovement()
    {
        
    }
}
