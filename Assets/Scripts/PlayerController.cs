using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private BubbleAbility _bubbleAbility;  // Reference to BubbleAbility

    private float _horizontalInput;
    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;
    private Rigidbody2D rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<AnimationController>();
        _bubbleAbility = GetComponent<BubbleAbility>(); // Get the BubbleAbility component

        // Subscribe to the ability actions
        _bubbleAbility.OnAbilityActivated += OnAbilityActivated;
        _bubbleAbility.OnAbilityDeactivated += OnAbilityDeactivated;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_bubbleAbility.IsShielded)
        {
            _bubbleAbility.ActivateAbility();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _bubbleAbility.DeactivateAbility();
        }

        _horizontalInput = Input.GetAxis("Horizontal");
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

    private void OnAbilityActivated()
    {
        // Handle any additional logic needed when the ability is activated
    }

    private void OnAbilityDeactivated()
    {
        // Handle any additional logic needed when the ability is deactivated
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object") && !_bubbleAbility.IsShielded)
        {
            Destroy(gameObject);
            PlayerDied?.Invoke();
        }
    }
}
