using UnityEngine;
using UnityEngine.U2D.Animation;

public class VictimSpawner : MonoBehaviour
{
    public GameObject victimPrefab;
    public int maxDay = 70;
    public int maxNight = 25;
    public bool isDay = true;
    public SpriteLibraryAsset[] victimSprites;
    public Transform[] spawnPoints;

    void Update()
    {
        int maxAllowed = isDay ? maxDay : maxNight;
        GameObject[] victims = GameObject.FindGameObjectsWithTag("victims");

        // DESPAWN extra (day → night)
        if (victims.Length > maxAllowed)
        {
            Destroy(victims[0]);   // remove one per frame (safe)
            return;
        }

        // SPAWN missing (night → day or deaths)
        if (victims.Length < maxAllowed)
        {
            int randIndex = Random.Range(0, spawnPoints.Length);
            GameObject victim = Instantiate(victimPrefab, spawnPoints[randIndex].position, Quaternion.identity);
            SpriteLibrary spriteLibrary = victim.GetComponent<SpriteLibrary>();
            int random = Random.Range(0, victimSprites.Length);
            spriteLibrary.spriteLibraryAsset = victimSprites[random];
        }
    }
}





