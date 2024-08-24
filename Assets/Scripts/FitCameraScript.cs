using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCameraScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float cameraWidth = mainCamera.orthographicSize * mainCamera.aspect * 2.0f;

        // Adjust scale to fit the camera width while maintaining aspect ratio
        transform.localScale = new Vector3(cameraWidth / spriteWidth, transform.localScale.y, transform.localScale.z);
    }
}
