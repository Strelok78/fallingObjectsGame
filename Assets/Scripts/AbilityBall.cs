using UnityEngine;

public class AbilityBall : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private PlayerController playerController; // Reference to the PlayerController

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnButtonPressed()
    {
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
    }
}