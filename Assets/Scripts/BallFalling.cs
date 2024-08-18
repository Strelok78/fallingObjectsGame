using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFalling : MonoBehaviour
{
    private float targetX;  // Target x-axis position
    private float fallSpeed = 5f;  // Speed of falling
    private float moveSpeed = 2f;  // Speed of moving towards the target x position
    private float xLeft;
    private float xRight;

    // Method to set the boundaries from FallingObject
    public void SetBounds(float xLeft, float xRight)
    {
        this.xLeft = xLeft;
        this.xRight = xRight;
        // Randomize the target x position within the boundaries
        targetX = Random.Range(xLeft, xRight);
    }

    private void Update()
    {
        // Move the object towards the target x position
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z);
    }

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