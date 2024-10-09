using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AbilityBall : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private PlayerController playerController; // Reference to the PlayerController
    [SerializeField] private Button abilityButton; // Reference to the button
    [SerializeField] private TextMeshProUGUI clicksLeftText; // Reference to the text displaying clicks left
    [SerializeField] private int maxClicks = 3; // Maximum number of clicks
    [SerializeField] private float resetTime = 5f; // Time to reset the button

    private Camera mainCamera;
    private int clicksLeft;

    private void Start()
    {
        mainCamera = Camera.main;
        clicksLeft = maxClicks;
        UpdateClicksLeftText();
    }

    public void OnButtonPressed()
    {
        if (clicksLeft > 0)
        {
            clicksLeft--;
            UpdateClicksLeftText();

            Vector3 playerPosition = transform.position; // Use the player's position
            GameObject projectile = Instantiate(projectilePrefab, playerPosition, Quaternion.identity);

            // Ensure the projectile has a Rigidbody2D and set it to kinematic
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

            // Add ProjectileMover component to handle movement
            projectile.AddComponent<ProjectileMover>().Initialize(projectileSpeed, mainCamera);

            if (clicksLeft == 0)
            {
                StartCoroutine(ResetButton());
            }
        }
    }

    private IEnumerator ResetButton()
    {
        abilityButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetTime);
        clicksLeft = maxClicks;
        UpdateClicksLeftText();
        abilityButton.gameObject.SetActive(true);
    }

    private void UpdateClicksLeftText()
    {
        clicksLeftText.text = clicksLeft.ToString();
    }
}