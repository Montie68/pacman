using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepotrer : MonoBehaviour
{
    [SerializeField]
    GameObject destination = null;
    Vector3 destinationPos;
    [SerializeField]
    Directions exitDirection = Directions.STOP;

    void Start()
    {
        destinationPos = destination.transform.position;
        int offset = 1;
        if (exitDirection == Directions.RIGHT) offset *= -1;

        destinationPos.x += offset;
        destinationPos.z = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (!actor.isTeleporting)
        {
            StartCoroutine(Teleport(actor));
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
    public IEnumerator Teleport(Actor actor)
    {
        actor.isTeleporting = true;
        if (actor.tag == "Enemies")
        {
            actor.transform.position = new Vector2(100,100);
            yield return new WaitForSeconds(1);
        }
        actor.ChangeDirection(exitDirection);
        actor.transform.position = destinationPos;

        yield return null;
    }
}
