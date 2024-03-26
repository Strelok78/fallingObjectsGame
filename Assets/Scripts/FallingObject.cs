using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private GameObject fallingObject;
    private float wait = 1f;

    private void Fall()
    {
        Instantiate(fallingObject, new Vector3(Random.Range(-1, 1), transform.position.y, 0), Quaternion.identity);
    }
}
