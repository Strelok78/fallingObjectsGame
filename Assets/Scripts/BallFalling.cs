using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFalling : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}