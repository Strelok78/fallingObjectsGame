using UnityEngine;

public class BallFalling : MonoBehaviour
{
    [SerializeField] private bool _isAbility;  // Flag to determine if this ball has an ability

    private float targetX;  // Target x-axis position
    private float fallSpeed = 5f;  // Speed of falling
    private float moveSpeed = 2f;  // Speed of moving towards the target x position
    private float xLeft;
    private float xRight;

    public void SetBounds(float xLeft, float xRight)
    {
        this.xLeft = xLeft;
        this.xRight = xRight;
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
        {
            Debug.Log("Collided with another BallFalling object. Ignoring.");
            return;
        }

        var bubbleAbility = collision.gameObject.GetComponent<BubbleAbility>();
        if (_isAbility && bubbleAbility != null)
        {
            Debug.Log("Player detected with BubbleAbility script, calling ActivateAbility directly.");
            bubbleAbility.ActivateAbility();
        }
        else
        {
            Debug.Log("No BubbleAbility component found, or _isAbility is false.");
        }

        // Delay destruction to ensure event handling
        Destroy(gameObject, 0.1f);
    }
}