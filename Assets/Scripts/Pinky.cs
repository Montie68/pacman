using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void StartGame()
    {
        StartCoroutine(LevelTimer());
        castRays();
        StartCoroutine(GhostActions());
    }
    // Update is called once per frame
    public override void Update()
    {
        if (GameManager.main.isStarted)
        {
            if (!hasStarted) return;
            castRays();
            ActorMovement();
        }
    }


    public override void GetRouteToTarget(Vector2 pos, List<Directions> dirs = null)
    {
        if (state == GhostState.CHASING)
        {
            Directions playerDir = player.GetComponent<Actor>().direction;
            switch (playerDir)
            {
                case (Directions.DOWN):

                    target = new Vector2(player.transform.position.x + (targetCoords.x * -1),
                                           player.transform.position.y);
                    break;
                case (Directions.LEFT):

                    target = new Vector2(player.transform.position.x, player.transform.position.y + (targetCoords.y * -1));
                    break;
                case (Directions.RIGHT):

                    target = new Vector2(player.transform.position.x, player.transform.position.y + (targetCoords.y));
                    break;

                case (Directions.UP):
                    target = (Vector2)player.transform.position + targetCoords;
                    break;

                default:
                    break;
            }
        }
        base.GetRouteToTarget(pos, dirs);

    }
    public override IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(startTimer);
        
        StartCoroutine(GhostActions());
        hasStarted = true;
    }


}