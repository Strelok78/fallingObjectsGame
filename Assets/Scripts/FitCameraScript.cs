using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FitCameraScript : MonoBehaviour
{
    [SerializeField] FallingObject _objectPool;
    
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private float _previousCameraWidth;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        AdjustScaleToFitCamera();
    }

    private void Update()
    {
        float currentCameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect * 2.0f;
        if (Mathf.Abs(currentCameraWidth - _previousCameraWidth) > 0.01f)
        {
            AdjustScaleToFitCamera();
        }
    }

    private void AdjustScaleToFitCamera()
    {
        float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        float cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect * 2.0f;
        
        transform.localScale = new Vector3(cameraWidth / spriteWidth, transform.localScale.y, transform.localScale.z);
        _previousCameraWidth = cameraWidth;
        _objectPool.ResetTargetPoints();
    }
}