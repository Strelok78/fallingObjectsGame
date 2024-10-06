using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
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
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            rigidbody2D.velocity = new Vector2((touchPosition.x - transform.position.x) * _moveSpeed, rigidbody2D.velocity.y);
            _animationController.OnSetXVelocity?.Invoke(Math.Abs(rigidbody2D.velocity.x));
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    private void FlipSprite()
    {
        if (_isFacingRight && rigidbody2D.velocity.x < 0f || !_isFacingRight && rigidbody2D.velocity.x > 0f)
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

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
                    {
                        _isDragging = true;
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_isDragging)
                    {
                        transform.position = new Vector2(touchPosition.x, transform.position.y);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _isDragging = false;
                    break;
            }
        }
    }
}