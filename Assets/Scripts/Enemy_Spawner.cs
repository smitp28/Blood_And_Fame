using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Enemy_Spawner : MonoBehaviour
{
    public int enemyCount;
    public GameObject _enemyPrefab;
    public GameObject PopMeter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Paparazzi");
        enemyCount = enemies.Length;
        if (PopularityMeter.instance.Current > 100f)
        {
            if (enemyCount <= 10)
            { 
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 80f)
        {
            if (enemyCount <= 8)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 60f)
        {
            if (enemyCount <= 6)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 40f)
        {
            if (enemyCount <= 4)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 20f)
        {
            if (enemyCount <= 2)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        enemyCount++;
    }
}