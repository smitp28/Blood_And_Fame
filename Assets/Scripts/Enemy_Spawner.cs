using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.U2D.Animation;

public class Enemy_Spawner : MonoBehaviour
{
    public int enemyCount;
    public GameObject _enemyPrefab;
    public SpriteLibraryAsset[] victimSprites;
    public GameObject PopMeter;

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Paparazzi");
        enemyCount = enemies.Length;
        if (PopularityMeter.instance.Current > 100f)
        {
            if (enemyCount <= 25)
            { 
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 80f)
        {
            if (enemyCount <= 20)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 60f)
        {
            if (enemyCount <= 15)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 40f)
        {
            if (enemyCount <= 10)
            {
                SpawnEnemy();
            }
        }
        else if (PopularityMeter.instance.Current > 20f)
        {
            if (enemyCount <= 5)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject victim = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        enemyCount++;
        if (victim.GetComponent<SpriteLibrary>() != null)
        {
            SpriteLibrary spriteLibrary = victim.GetComponent<SpriteLibrary>();
            int random = Random.Range(0, victimSprites.Length);
            spriteLibrary.spriteLibraryAsset = victimSprites[random];
        }
    }
}