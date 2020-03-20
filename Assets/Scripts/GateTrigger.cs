using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameObject Gate;
    triggerDir dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = GetComponent<triggerDir>();

    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.layer == 9)
        {
            if (col.GetComponent<Ghost>().state == GhostState.EATEN)
            {
                dir.directions.Remove(Directions.RIGHT);
                dir.directions.Remove(Directions.LEFT);
                dir.directions.Add(Directions.DOWN);
                Gate.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.layer == 9)
        {
            if (col.GetComponent<Ghost>().state == GhostState.EATEN)
            {
                StartCoroutine(Delay());
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
          Gate.GetComponent<Collider2D>().enabled = true;
    }
}
