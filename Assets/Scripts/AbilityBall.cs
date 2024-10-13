using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class AbilityBall : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Button _abilityButton;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _clicksLeftText;
    [SerializeField] private int _maxClicks = 3;
    [SerializeField] private float _projectileSpeed = 5f;
    [SerializeField] private float _resetTime = 5f;

    private int _clicksLeft;
    private CanvasGroup _abilityButtonCanvasGroup;

    private void Awake()
    {
        _clicksLeft = _maxClicks;
        UpdateClicksLeftText();
        _abilityButtonCanvasGroup = _abilityButton.GetComponent<CanvasGroup>();
        if (_abilityButtonCanvasGroup == null)
        {
            _abilityButtonCanvasGroup = _abilityButton.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnButtonPressed()
    {
        if (_clicksLeft > 0)
        {
            _playerController.CallShootingAnimation();
            _clicksLeft--;
            UpdateClicksLeftText();

            // Calculate the top position of the player's sprite
            Vector3 playerPosition = transform.position;
            SpriteRenderer playerSpriteRenderer = _playerController.GetComponent<SpriteRenderer>();
            float playerSpriteHeight = playerSpriteRenderer.bounds.size.y;
            Vector3 projectilePosition = playerPosition + new Vector3(0, playerSpriteHeight / 2, 0);

            GameObject projectile = Instantiate(_projectilePrefab, projectilePosition, Quaternion.identity);
            projectile.GetComponent<ProjectileMover>().InitializeScript(_projectileSpeed, _mainCamera);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                rb = projectile.AddComponent<Rigidbody2D>();
            }
            rb.isKinematic = true;

            // Ignore collision between the projectile and the player
            Collider2D playerCollider = _playerController.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }

            if (_clicksLeft == 0)
            {
                StartCoroutine(ResetButton());
            }
        }
    }

    private IEnumerator ResetButton()
    {
        _abilityButton.gameObject.SetActive(true);
        _abilityButtonCanvasGroup.alpha = 0.5f; // Partly visible
        float elapsedTime = 0f;

        while (elapsedTime < _resetTime)
        {
            elapsedTime += Time.deltaTime;
            _abilityButtonCanvasGroup.alpha = Mathf.Lerp(0.5f, 1f, elapsedTime / _resetTime);
            yield return null;
        }

        _clicksLeft = _maxClicks;
        UpdateClicksLeftText();
        _abilityButtonCanvasGroup.alpha = 1f; // Fully visible
    }

    private void UpdateClicksLeftText()
    {
        _clicksLeftText.text = _clicksLeft.ToString();
    }
}