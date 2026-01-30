using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Enemy_Spawner : MonoBehaviour
{
    private float timer;
    public float spawnInterval = 3f;
    public GameObject _enemyPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
    }
}