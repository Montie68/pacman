using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    public GameObject blinky;

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
            Vector2 blinkDir = blinky.transform.position;

            Vector2 targetPos = new Vector2();
            switch (playerDir)
            {
                case (Directions.DOWN):

                    targetPos = new Vector2(player.transform.position.x + (targetCoords.x * -1),
                                           player.transform.position.y);
                    break;
                case (Directions.LEFT):

                    targetPos = new Vector2(player.transform.position.x, player.transform.position.y + (targetCoords.y * -1));
                    break;
                case (Directions.RIGHT):

                    targetPos = new Vector2(player.transform.position.x, player.transform.position.y + (targetCoords.y));
                    break;

                case (Directions.UP):
                    targetPos = (Vector2)player.transform.position + targetCoords;
                    break;

                default:
                    break;
            }

            target = ((Vector2)blinky.transform.position + (targetPos)) * 2;
        }
        base.GetRouteToTarget(pos, dirs);

    }
    public override IEnumerator WaitForStart()
    {
        int palletCount = palletCounter.instance.palletCount;

        while(palletCounter.instance.palletCount > (palletCount - startTimer))
        {
            yield return new WaitForSeconds(1 / 15);
        }
        
        StartCoroutine(GhostActions());
        hasStarted = true;
    }


}