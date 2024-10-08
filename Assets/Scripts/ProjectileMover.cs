using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private float projectileSpeed;
    private Camera mainCamera;

    public void Initialize(float speed, Camera camera)
    {
        projectileSpeed = speed;
        mainCamera = camera;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime, Space.World);

        if (IsOutOfView())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.y > 1;
    }
}