using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;

    private float _moveSpeed = 10f;
    private bool _isFacingRight = true;
    private bool _isDragging = false;
    private bool _isDead = false;
    private Rigidbody2D _rigidbody2D;

    public event UnityAction PlayerDied;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<AnimationController>();
    }

    //updated
    private void Update()
    {
        if (!_isDead)
        {
            HandleTouchInput();
            HandleKeyboardInput();
        }
        
        FlipSprite();
    }


    //updated
    private void FixedUpdate()
    {
        if (_isDragging && !_isDead)
        {
            Vector2 targetPosition = Vector2.zero;

            if (Input.touchCount > 0)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            targetPosition = ClampToScreenBounds(targetPosition);
            _rigidbody2D.linearVelocity = new Vector2((targetPosition.x - transform.position.x) * _moveSpeed, _rigidbody2D.linearVelocity.y);
            _animationController.OnSetXVelocity?.Invoke(Mathf.Abs(_rigidbody2D.linearVelocity.x));
        }
        else if (!_isDragging && !_isDead)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector2 movement = new Vector2(horizontalInput * _moveSpeed, _rigidbody2D.linearVelocity.y);
            _rigidbody2D.linearVelocity = movement;
            
            if (horizontalInput != 0)
            {
                _animationController.OnSetXVelocity?.Invoke(Mathf.Abs(_rigidbody2D.linearVelocity.x));
            }
            else
            {
                _animationController.OnSetXVelocity?.Invoke(0);
            }
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
        }
    }

    
    //added
    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || 
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _isDragging = false;
        }
    }


    private void FlipSprite()
    {
        if ((_isFacingRight && _rigidbody2D.linearVelocity.x < 0f) || (!_isFacingRight && _rigidbody2D.linearVelocity.x > 0f))
        {
            _isFacingRight = !_isFacingRight;
            _animationController.OnFlipSprite?.Invoke(_isFacingRight);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object") && !_isDead)
        {
            PlayerDied?.Invoke();
            _isDead = true;
            _animationController.AnimateDeath();
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

    public void CallShootingAnimation()
    {
        _animationController.AnimateShooting();
    }
}