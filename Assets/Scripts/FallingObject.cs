using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
        Instantiate(fallingObject, new Vector3(Random.Range(xLeft, xRight), transform.position.y, 0), Quaternion.identity);
    }
}
