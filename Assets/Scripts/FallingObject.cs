using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private GameObject fallingObject;
    private float wait = 0.5f;
    private float xLeft;
    private float xRight;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        float screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        xLeft = mainCamera.transform.position.x - screenHalfWidth - 0.5f;
        xRight = mainCamera.transform.position.x + screenHalfWidth - 0.5f;
    }

    private void Start()
    {
        InvokeRepeating("Fall", wait, wait);
    }

    private void Fall()
    {
        GameObject newFallingObject = Instantiate(fallingObject, new Vector3(Random.Range(xLeft, xRight), transform.position.y, 0), Quaternion.identity);

        // Set xLeft and xRight on BallFalling component
        BallFalling ballFalling = newFallingObject.GetComponent<BallFalling>();
        if (ballFalling != null)
        {
            ballFalling.SetBounds(xLeft, xRight);
        }
    }
    
    public float GetXLeft() => xLeft;
    public float GetXRight() => xRight;
}