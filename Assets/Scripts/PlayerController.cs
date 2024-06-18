using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;

    private Animator _animator;
    private Rigidbody2D rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(_horizontalInput * _moveSpeed, rigidbody2D.velocity.y);
        _animator.SetFloat("xVelocity", Math.Abs(rigidbody2D.velocity.x));
    }

    private void FlipSprite()
    {
        if (_isFacingRight && _horizontalInput < 0f || !_isFacingRight && _horizontalInput > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x = -1f;
            transform.localScale = localScale;
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
}