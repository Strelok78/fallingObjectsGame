using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AbilityBall : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Button abilityButton;
    [SerializeField] private TextMeshProUGUI clicksLeftText;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private int maxClicks = 3;
    [SerializeField] private float resetTime = 5f;

    private int clicksLeft;
    private CanvasGroup abilityButtonCanvasGroup;

    private void Awake()
    {
        clicksLeft = maxClicks;
        UpdateClicksLeftText();
        abilityButtonCanvasGroup = abilityButton.GetComponent<CanvasGroup>();
        if (abilityButtonCanvasGroup == null)
        {
            abilityButtonCanvasGroup = abilityButton.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnButtonPressed()
    {
        if (clicksLeft > 0)
        {
            clicksLeft--;
            UpdateClicksLeftText();

            // Calculate the top position of the player's sprite
            Vector3 playerPosition = transform.position;
            SpriteRenderer playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();
            float playerSpriteHeight = playerSpriteRenderer.bounds.size.y;
            Vector3 projectilePosition = playerPosition + new Vector3(0, playerSpriteHeight / 2, 0);

            GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
            projectile.GetComponent<ProjectileMover>().InitializeScript(projectileSpeed, _mainCamera);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                rb = projectile.AddComponent<Rigidbody2D>();
            }
            rb.isKinematic = true;

            // Ignore collision between the projectile and the player
            Collider2D playerCollider = playerController.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }

            if (clicksLeft == 0)
            {
                StartCoroutine(ResetButton());
            }
        }
    }

    private IEnumerator ResetButton()
    {
        abilityButton.gameObject.SetActive(true);
        abilityButtonCanvasGroup.alpha = 0.5f; // Partly visible
        float elapsedTime = 0f;

        while (elapsedTime < resetTime)
        {
            elapsedTime += Time.deltaTime;
            abilityButtonCanvasGroup.alpha = Mathf.Lerp(0.5f, 1f, elapsedTime / resetTime);
            yield return null;
        }

        clicksLeft = maxClicks;
        UpdateClicksLeftText();
        abilityButtonCanvasGroup.alpha = 1f; // Fully visible
    }

    private void UpdateClicksLeftText()
    {
        clicksLeftText.text = clicksLeft.ToString();
    }
}