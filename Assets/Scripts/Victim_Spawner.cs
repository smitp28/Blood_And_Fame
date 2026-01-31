using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.U2D.Animation;
public class Victim_Spawner : MonoBehaviour
{
    public int enemyCount;
    public GameObject _enemyPrefab;
    public SpriteLibraryAsset[] victimSprites;
    public GameObject PopMeter;
    public int maxEnemyday=35;
    public int maxEnemynight=15;
    public int current;


    public void SpawnEnemyNight()
    {
        current = GameObject.FindGameObjectsWithTag("victims").Length;
        while (current < maxEnemynight)
        {
            SpawnEnemy();
            current++;
        }
        while (current > maxEnemynight)
        {
            GameObject[] victims = GameObject.FindGameObjectsWithTag("victims");
            Destroy(victims[0]);
            current--;
        }
    }

    public void SpawnEnemyDay()
    {
        current = GameObject.FindGameObjectsWithTag("victims").Length;
        while (current < maxEnemyday)
        {
            SpawnEnemy();
            current++;
        }
        while (current > maxEnemyday)
        {
            GameObject[] victims = GameObject.FindGameObjectsWithTag("victims");
            Destroy(victims[0]);
            current--;
        }
    }

    private void SpawnEnemy()
    {
        GameObject victim = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        SpriteLibrary spriteLibrary = victim.GetComponent<SpriteLibrary>();
        int random = Random.Range(0, victimSprites.Length);
        spriteLibrary.spriteLibraryAsset = victimSprites[random];
    }

}