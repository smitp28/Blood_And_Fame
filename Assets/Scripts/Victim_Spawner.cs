using UnityEngine;
using UnityEngine.U2D.Animation;

public class VictimSpawner : MonoBehaviour
{
    public GameObject victimPrefab;
    public int maxDay = 35;
    public int maxNight = 15;
    public bool isDay = true;
    public SpriteLibraryAsset[] victimSprites;
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
            GameObject victim = Instantiate(victimPrefab, transform.position, Quaternion.identity);
            SpriteLibrary spriteLibrary = victim.GetComponent<SpriteLibrary>();
            int random = Random.Range(0, victimSprites.Length);
            spriteLibrary.spriteLibraryAsset = victimSprites[random];
        }
    }
}





