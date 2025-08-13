using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // (опционально) чтобы игнорировать клики по UI

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

    // NEW: кэш камеры и ширины спрайта (для "бережной" блокировки по экрану)
    private Camera _mainCam;
    [SerializeField] private float _halfWidth = 0.5f; // подстрой под ширину спрайта

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<AnimationController>();
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (!_isDead)
        {
            HandlePointerInput();   // UPDATED: единый ввод (тач + мышь)
            HandleKeyboardInput();
        }

        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (_isDragging && !_isDead)
        {
            Vector2 targetPosition = Vector2.zero;

            // UPDATED: если есть тач — используем его, иначе мышь
            if (Input.touchCount > 0)
            {
                targetPosition = _mainCam.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (Input.GetMouseButton(0))
            {
                targetPosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                // перестраховка: если ни тача ни мыши — перестаём тянуть
                _isDragging = false;
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
                _animationController.OnSetXVelocity?.Invoke(Mathf.Abs(_rigidbody2D.linearVelocity.x));
            else
                _animationController.OnSetXVelocity?.Invoke(0);
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _isDragging = false; // при управлении клавишами отключаем перетягивание
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

    // UPDATED: единый обработчик для тача и мыши
    private void HandlePointerInput()
    {
        // (опционально) если курсор над UI — игнорируем, чтобы кнопки не тянули игрока
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // --- НАЖАТИЕ ---
        if (PointerDown())
        {
            Vector2 wp = GetPointerWorldPosition();
            // условие "начинать тянуть только если ткнули по самому игроку"
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(wp))
            {
                _isDragging = true;
            }
        }

        // --- ОТПУСКАНИЕ ---
        if (PointerUp())
        {
            _isDragging = false;
        }

        // ПРОЦЕСС перетаскивания в Update не двигаем (чтобы не ломать физику),
        // сам сдвиг делаем в FixedUpdate через скорость — как у тебя было.
    }

    private Vector2 ClampToScreenBounds(Vector2 position)
    {
        // ограничиваем только по X (как в твоей логике)
        Vector3 rt = _mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        position.x = Mathf.Clamp(position.x, -rt.x + _halfWidth, rt.x - _halfWidth);
        return position;
    }

    public void CallShootingAnimation()
    {
        _animationController.AnimateShooting();
    }

    // ======== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (мышь как "псевдо-тач") ========

    private bool PointerDown()
    {
        // Если есть тач — считаем его основным
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;

        // иначе мышь
        return Input.GetMouseButtonDown(0);
    }

    private bool PointerUp()
    {
        if (Input.touchCount > 0)
        {
            var ph = Input.GetTouch(0).phase;
            return ph == TouchPhase.Ended || ph == TouchPhase.Canceled;
        }
        return Input.GetMouseButtonUp(0);
    }

    private Vector2 GetPointerWorldPosition()
    {
        if (Input.touchCount > 0)
            return _mainCam.ScreenToWorldPoint(Input.GetTouch(0).position);

        return _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
