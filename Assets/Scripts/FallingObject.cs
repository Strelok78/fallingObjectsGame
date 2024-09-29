using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private GameObject _fallingObject;
    [SerializeField] private GameObject _fallingAbilityObject;
    [SerializeField] private float _ballSpawnTime;
    [SerializeField] private float _abilitySpawnTime;
    [SerializeField] private float _minBallSpawnTime = 0.2f;
    [SerializeField] private float _spawnTimeDecreaseRate = 0.1f;
    [SerializeField] private float _spawnTimeDecreaseInterval = 10f;

    private Camera _mainCamera;

    private float _xLeft;
    private float _xRight;

    private void Awake()
    {
        _mainCamera = Camera.main;
        float screenHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
        _xLeft = _mainCamera.transform.position.x - screenHalfWidth - 0.5f;
        _xRight = _mainCamera.transform.position.x + screenHalfWidth - 0.5f;
    }

    private void Start()
    {
        InvokeRepeating("Fall", _ballSpawnTime, _ballSpawnTime);
        InvokeRepeating("FallAbility", _abilitySpawnTime, _abilitySpawnTime);
        StartCoroutine(DecreaseSpawnTime());
    }

    private void FallAbility()
    {
        GameObject newFallingAbiityObject = Instantiate(_fallingAbilityObject, new Vector3(Random.Range(_xLeft, _xRight), transform.position.y, 0), Quaternion.identity);
        BallFalling ballAbilityFalling = newFallingAbiityObject.GetComponent<BallFalling>();

        if (ballAbilityFalling != null)
        {
            ballAbilityFalling.SetBounds(_xLeft, _xRight);
        }
    }

    private void Fall()
    {
        GameObject newFallingObject = Instantiate(_fallingObject, new Vector3(Random.Range(_xLeft, _xRight), transform.position.y, 0), Quaternion.identity);

        // Set xLeft and xRight on BallFalling component
        BallFalling ballFalling = newFallingObject.GetComponent<BallFalling>();

        if (ballFalling != null)
        {
            ballFalling.SetBounds(_xLeft, _xRight);
        }
    }

    private IEnumerator DecreaseSpawnTime()
    {
        while (_ballSpawnTime > _minBallSpawnTime)
        {
            yield return new WaitForSeconds(_spawnTimeDecreaseInterval); // Adjust the interval as needed
            _ballSpawnTime = Mathf.Max(_ballSpawnTime - _spawnTimeDecreaseRate, _minBallSpawnTime);
            CancelInvoke("Fall");
            InvokeRepeating("Fall", _ballSpawnTime, _ballSpawnTime);
        }
    }

    public float GetXLeft() => _xLeft;
    public float GetXRight() => _xRight;
}