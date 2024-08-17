using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;

    private float _horizontalInput;
    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;
    private bool _isShieled = false;
    private float _abilityCooldown;
    private float _abilityTimer;
    private float _abilityBaseCooldown = 5f;
    private float _abilityBaseTimer = 5f;
    private bool _abilityIsReady = true;

    private Animator _animator;
    private Rigidbody2D rigidbody2D;
    private GameObject _bubleExample;
    private Coroutine _abilityCoroutine;

    public event UnityAction PlayerDied;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        ResetAbility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _abilityIsReady)
        {
            ActivateAbility();
        }

        if (_bubleExample != null)
        {
            _bubleExample.transform.position = transform.position;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            DeactivateAbility();
        }

        _horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();
    }

    private void ActivateAbility()
    {
        _isShieled = true;
        _bubleExample = Instantiate(_bubble);
        _abilityIsReady = false;  // Делаем способность недоступной, пока она активирована
        _abilityCoroutine = StartCoroutine(AbilityUseTimer());
    }

    private void DeactivateAbility()
    {
        _isShieled = false;
        DestroyBubble();

        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
        }

        _abilityCoroutine = StartCoroutine(AbilityCooldown());
    }

    private void DestroyBubble()
    {
        if (_bubleExample != null)
        {
            Destroy(_bubleExample);
            _bubleExample = null;
        }
    }

    private void ResetAbility()
    {
        _abilityCooldown = _abilityBaseCooldown;
        _abilityTimer = _abilityBaseTimer;
        _abilityIsReady = true;
    }

    private IEnumerator AbilityUseTimer()
    {
        while (_abilityTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            _abilityTimer -= 1f;
            Debug.Log("Time remaining for Ability: " + _abilityTimer);
        }

        _abilityTimer = 0f;
        Debug.Log("Ability timer ended, starting cooldown.");
        DeactivateAbility();
    }

    private IEnumerator AbilityCooldown()
    {
        while (_abilityCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            _abilityCooldown -= 1f;
            Debug.Log("Cooldown remaining: " + _abilityCooldown);
        }

        ResetAbility();
        Debug.Log("Ability is ready again.");
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
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object") && !_isShieled)
        {
            Destroy(gameObject);
            PlayerDied?.Invoke();
        }
    }
}