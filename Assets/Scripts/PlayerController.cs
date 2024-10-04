using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private BubbleAbility _bubbleAbility;

    private float _horizontalInput;
    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;
    private Rigidbody2D rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<AnimationController>();
        _bubbleAbility = GetComponent<BubbleAbility>();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        HandleTouchInput();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(_horizontalInput * _moveSpeed, rigidbody2D.velocity.y);
        _animationController.OnSetXVelocity?.Invoke(Math.Abs(rigidbody2D.velocity.x));
    }

    private void FlipSprite()
    {
        if (_isFacingRight && _horizontalInput < 0f || !_isFacingRight && _horizontalInput > 0f)
        {
            _isFacingRight = !_isFacingRight;
            _animationController.OnFlipSprite?.Invoke(_isFacingRight);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object") && !_bubbleAbility.IsShielded)
        {
            Destroy(gameObject);
            PlayerDied?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Object") && _bubbleAbility.IsShielded)
        {
            _bubbleAbility.DeactivateAbility();
        }
    }

    private void HandleTouchInput()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    _horizontalInput = -1f; // Move left
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    _horizontalInput = 1f; // Move right
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _horizontalInput = 0f; // Stop moving
            }
        }
        else
        {
            _horizontalInput = 0f; // Stop moving if no touch
        }
    }
}