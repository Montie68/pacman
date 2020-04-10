using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    // This is a singleton
    public static GameManager main;
    // List of Actors
    Actor[] actors;
    // List of player Prefabs
    // List of UI text for scores
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI readyText;
    // List of player Lives
    // Delay for start of level
    public int Deley = 3;
    // bool for start 
    [HideInInspector]
    public bool isStarted = false;

    public UnityEvent playerDead = new UnityEvent();

    int Points = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerDead.AddListener(PlayerDeath);
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
        // Get the actors
        actors = FindObjectsOfType<Actor>();
        // start level.
        StartCoroutine(StartLevel());
    }

    private void PlayerDeath()
    {
        foreach(Actor actor in actors)
        {
            actor.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // method to add new player return player ID
    // method to add score.
    internal void addPoints(int points)
    {
        Points += points;
        string pts =  Points.ToString();
        scoreText.text = pts;
    }
    // method to start level
    public IEnumerator StartLevel()
    {
        readyText.text = "Ready!";
        int countDown = Deley;
        yield return new WaitForSeconds(1);

        while (countDown > 0)
        {
            readyText.text = countDown-- + "!";
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(FadeReady());
        foreach(Actor a in actors)
        {
            a.StartGame();
            StartCoroutine(a.WaitForStart());
        }
    }
    private IEnumerator FadeReady()
    {
        readyText.text = "Go!";
        float interval = 1 / 15;
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(interval);
            readyText.alpha -= (interval*2);
        }
        readyText.text = "";
        readyText.alpha = 1;
    }
    // method to -player.lives
    // method gameover
}
