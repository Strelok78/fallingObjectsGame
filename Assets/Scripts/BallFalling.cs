// Assets/Scripts/BallFalling.cs
using UnityEngine;

public class BallFalling : MonoBehaviour
{
    private float targetX;
    private float fallSpeed = 5f;
    private float moveSpeed = 2f;

    public void SetBounds(float xLeft, float xRight)
    {
        targetX = Random.Range(xLeft, xRight);
    }

    private void Update()
    {
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BallFalling>())
            return;

        Destroy(gameObject);
    }
}