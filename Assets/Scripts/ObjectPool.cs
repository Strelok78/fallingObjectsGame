using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject fallingObject;
    [SerializeField] private Transform groundBlock;
    private float[] spawnRadius = new float[2];
    private float spawnTime = 2f;
    private float fallingSpeed = 10f;

    private void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            spawnRadius[i] = i == 0 ? -groundBlock.localScale.x : groundBlock.localScale.x;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
