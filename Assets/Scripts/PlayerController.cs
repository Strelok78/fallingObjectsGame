// Assets/Scripts/PlayerController.cs
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;

    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;
    private bool _isDragging = false;
    private Rigidbody2D rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        HandleTouchInput();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (_isDragging)
        {
            Vector2 targetPosition = Vector2.zero;

            if (Input.touchCount > 0)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            targetPosition = ClampToScreenBounds(targetPosition);
            rigidbody2D.velocity = new Vector2((targetPosition.x - transform.position.x) * _moveSpeed, rigidbody2D.velocity.y);
            _animationController.OnSetXVelocity?.Invoke(Mathf.Abs(rigidbody2D.velocity.x));
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    private void FlipSprite()
    {
        if ((_isFacingRight && rigidbody2D.velocity.x < 0f) || (!_isFacingRight && rigidbody2D.velocity.x > 0f))
        {
            _isFacingRight = !_isFacingRight;
            _animationController.OnFlipSprite?.Invoke(_isFacingRight);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Destroy(gameObject);
            PlayerDied?.Invoke();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began && GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
            {
                _isDragging = true;
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && _isDragging)
            {
                transform.position = new Vector2(touchPosition.x, transform.position.y);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isDragging = false;
            }
        }
    }

    private Vector2 ClampToScreenBounds(Vector2 position)
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        position.x = Mathf.Clamp(position.x, -screenBounds.x, screenBounds.x);
        return position;
    }
}