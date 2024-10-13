// ProjectileMover.cs
using System;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private float projectileSpeed;
    private Camera mainCamera;

    public void InitializeScript(float speed, Camera camera)
    {
        projectileSpeed = speed;
        mainCamera = camera ?? Camera.main; // Ensure mainCamera is not null
        Debug.Log("ProjectileMover initialized with speed: " + speed + " and camera: " + mainCamera);
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            Debug.LogError("mainCamera is null in ProjectileMover");
            return;
        }

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }
}