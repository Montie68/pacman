using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCast2D : MonoBehaviour
{
    public List<playerDirection> directions;

    private void Start()
    {
        directions = new List<playerDirection>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        directions = new List<playerDirection>();

        if (collision.gameObject.layer == 8)
        {
            Debug.Log("touch");
        }
    }
    public List<playerDirection> FreeDirections()
    {
        return directions;
    }
}
