using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepotrer : MonoBehaviour
{
    [SerializeField]
    GameObject destination = null;
    Vector3 destinationPos;
    [SerializeField]
    playerDirection exitDirection = playerDirection.STOP;

    void Start()
    {
        destinationPos = destination.transform.position;
        int offset = 1;
        if (exitDirection == playerDirection.RIGHT) offset = -1;

        destinationPos.x += offset;
        destinationPos.z = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (!actor.isTeleporting)
        {
            actor.isTeleporting = true;
            actor.ChangeDirection(exitDirection);
            actor.transform.position = destinationPos;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor.isTeleporting)
        {
            actor.isTeleporting = false;
        }
    }
}
