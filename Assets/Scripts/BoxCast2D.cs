using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCast2D : MonoBehaviour
{
    public List<Directions> directions;

    private void Start()
    {
        directions = new List<Directions>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        directions = new List<Directions>();

        if (collision.gameObject.layer == 8)
        {
            Debug.Log("touch");
        }
    }
    public List<Directions> FreeDirections()
    {
        return directions;
    }
}
