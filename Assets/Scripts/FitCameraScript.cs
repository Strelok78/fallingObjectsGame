using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCameraScript : MonoBehaviour
{
    [SerializeField] FallingObject fallingObject;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private float previousCameraWidth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        AdjustScaleToFitCamera();
    }

    private void Update()
    {
        float currentCameraWidth = mainCamera.orthographicSize * mainCamera.aspect * 2.0f;
        if (Mathf.Abs(currentCameraWidth - previousCameraWidth) > 0.01f)
        {
            AdjustScaleToFitCamera();
        }
    }

    private void AdjustScaleToFitCamera()
    {
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float cameraWidth = mainCamera.orthographicSize * mainCamera.aspect * 2.0f;

        // Adjust scale to fit the camera width while maintaining aspect ratio
        transform.localScale = new Vector3(cameraWidth / spriteWidth, transform.localScale.y, transform.localScale.z);

        previousCameraWidth = cameraWidth;
        fallingObject.ResetTargetPoints();
    }
}