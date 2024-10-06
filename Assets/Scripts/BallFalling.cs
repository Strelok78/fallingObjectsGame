using UnityEngine;

public class BallFalling : MonoBehaviour
{
    [SerializeField] private bool _isAbility;  // Flag to determine if this ball has an ability

    private float targetX;  // Target x-axis position
    private float fallSpeed = 5f;  // Speed of falling
    private float moveSpeed = 2f;  // Speed of moving towards the target x position

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
        Debug.Log("OnCollisionEnter2D triggered with: " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<BallFalling>())
            return;

        Destroy(gameObject);
    }
}