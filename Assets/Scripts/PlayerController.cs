using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float moveSpeed = 10f;

    private Rigidbody2D rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, rigidbody2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Destroy(gameObject);
            PlayerDied?.Invoke();
        }
    }
}