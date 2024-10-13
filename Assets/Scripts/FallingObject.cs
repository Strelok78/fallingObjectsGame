using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private GameObject _fallingObject;
    [SerializeField] private float _ballSpawnTime;
    [SerializeField] private float _minBallSpawnTime = 0.6f;
    [SerializeField] private float _spawnTimeDecreaseRate = 0.1f;
    [SerializeField] private float _spawnTimeDecreaseInterval = 10f;

    private Camera _mainCamera;
    private float _screenHalfWidth;
    private float _xLeft;
    private float _xRight;

    private void Start()
    {
        _mainCamera = Camera.main;
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
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        if (_mainCamera != null)
        {
            _screenHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            _xLeft = _mainCamera.transform.position.x - _screenHalfWidth - 0.05f;
            _xRight = _mainCamera.transform.position.x + _screenHalfWidth - 0.05f;
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