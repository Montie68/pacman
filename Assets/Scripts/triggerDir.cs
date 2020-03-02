using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDir : MonoBehaviour
{
    public List<Directions> directions;
    public bool isGhostHouse = false;
    List<Directions> BackUpDirs;
    
    void Start()
    {
        BackUpDirs = new List<Directions>();
        foreach (Directions d in directions)
            BackUpDirs.Add(d);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject col = collision.gameObject;
        if (col.layer == 9 && isGhostHouse)
        {
            if (col.GetComponent<Ghost>().state == GhostState.EATEN)
            {
                StartCoroutine(Deley(col, collision.GetComponent<Ghost>()));
            }
        }
    }

    IEnumerator Deley(GameObject col, Ghost ghost)
    {
        yield return new WaitForSeconds(2);
        col.GetComponent<Animator>().SetBool("IsEaten", false);
        col.GetComponent<Animator>().SetBool("IsFleeing", false);
        col.GetComponent<Actor>().nextDirection = Directions.UP;
        col.GetComponent<Ghost>().state = GhostState.SCATTER;

        yield return new WaitForSeconds(1);
        col.GetComponent<Ghost>().state = GhostState.CHASING;

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (directions != BackUpDirs)
        {
            directions = new List<Directions>();
            foreach (Directions d in BackUpDirs)
                directions.Add(d);
        }
    }
}
