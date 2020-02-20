using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This is a singleton
    [HideInInspector]
    public GameManager main;
    // List of players
    // List of player Prefabs
    // List of UI text for scores
    // List of player Lives
    // Delay for start of level


    // Start is called before the first frame update
    void Start()
    {
        // make sure that this is to only copy of the gameManger
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(main);
        }
        else
        {
            // if main is set destory duplicate.
            Destroy(this);
        }

        // start level.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // method to add new player return player ID
    // method to add score.
    // method to start level
    // method to -player.lives
    // method gameover
}
