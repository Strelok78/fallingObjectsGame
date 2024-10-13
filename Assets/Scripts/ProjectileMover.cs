using System;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private float _projectileSpeed;
    private Camera _mainCamera;

    public void InitializeScript(float speed, Camera camera)
    {
        _projectileSpeed = speed;
        _mainCamera = camera ?? Camera.main;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _projectileSpeed * Time.deltaTime, Space.World);

        if (IsOutOfView())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfView()
    {
        Vector3 screenPoint = _mainCamera.WorldToViewportPoint(transform.position);
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