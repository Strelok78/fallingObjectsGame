// Assets/Scripts/FallingObject.cs
using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private GameObject _fallingObject;
    [SerializeField] private float _ballSpawnTime;
    [SerializeField] private float _minBallSpawnTime = 0.6f;
    [SerializeField] private float _spawnTimeDecreaseRate = 0.1f;
    [SerializeField] private float _spawnTimeDecreaseInterval = 10f;

    private Camera mainCamera;
    private float screenHalfWidth;
    private float _xLeft;
    private float _xRight;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateScreenBounds();
        InvokeRepeating(nameof(Fall), _ballSpawnTime, _ballSpawnTime);
        StartCoroutine(DecreaseSpawnTime());
    }

    private void Update()
    {
        UpdateScreenBounds();
    }

    private void Fall()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(_xLeft, _xRight), transform.position.y, 0);
        GameObject newFallingObject = Instantiate(_fallingObject, spawnPosition, Quaternion.identity);

        BallFalling ballFalling = newFallingObject.GetComponent<BallFalling>();
        ballFalling?.SetBounds(_xLeft, _xRight);
    }

    public void ResetTargetPoints()
    {
        UpdateScreenBounds();
    }

    private void UpdateScreenBounds()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            _xLeft = mainCamera.transform.position.x - screenHalfWidth - 0.05f;
            _xRight = mainCamera.transform.position.x + screenHalfWidth - 0.05f;
        }
    }

    private IEnumerator DecreaseSpawnTime()
    {
        while (_ballSpawnTime > _minBallSpawnTime)
        {
            yield return new WaitForSeconds(_spawnTimeDecreaseInterval);
            _ballSpawnTime = Mathf.Max(_ballSpawnTime - _spawnTimeDecreaseRate, _minBallSpawnTime);
            CancelInvoke(nameof(Fall));
            InvokeRepeating(nameof(Fall), _ballSpawnTime, _ballSpawnTime);
        }
    }
}