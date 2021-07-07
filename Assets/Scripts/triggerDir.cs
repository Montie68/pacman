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

    private void Update()
    {
        if (directions.Count == 0) RestoreDirections();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject col = collision.gameObject;
        //int layer = 1 << 9;
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
        directions = new List<Directions>();
        yield return new WaitForSeconds(1);
        directions.Add(Directions.DOWN);
        col.GetComponent<Actor>().nextDirection = Directions.DOWN;
        yield return new WaitForSeconds(1/ 2);
        directions = new List<Directions>();
        directions.Add(Directions.STOP);
        col.GetComponent<Actor>().Velocity = Vector3.zero;
        col.GetComponent<Actor>().nextDirection = Directions.STOP;
        yield return new WaitForSeconds(30/60f);
        directions = new List<Directions>();
        directions.Add(Directions.UP);
        col.GetComponent<Animator>().SetBool("IsEaten", false);
        col.GetComponent<Animator>().SetBool("IsFleeing", false);
        col.GetComponent<Actor>().nextDirection = Directions.UP;
        col.GetComponent<Ghost>().state = GhostState.SCATTER;

        yield return new WaitForSeconds(1);
        col.GetComponent<Ghost>().state = GhostState.CHASING;

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        RestoreDirections();
    }

    private void RestoreDirections()
    {
        if (directions != BackUpDirs)
        {
            directions = new List<Directions>();
            foreach (Directions d in BackUpDirs)
                directions.Add(d);
        }
    }
}
