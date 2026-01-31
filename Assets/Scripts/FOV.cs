using System.Collections;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public GameObject paparazzi;
    public Mesh mesh;
    public Vector3 origin;

    public Collider2D playercoll;
    public Collider2D corpsecoll;

    public bool playervisible;
    public bool corpsevisible;

    public float lowkilldist = 2f;
    public float medkilldist = 4f;
    public float highkilldist = 6f;

    public float lowinsanity = 5f;
    public float medinsanity = 10f;
    public float highinsanity = 15f;

    private bool corpseVisiblelastframe;
    private float scanTime = 1f;
    public bool isCheckingcorpse;

    private float timer;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        paparazzi = GameObject.FindWithTag("Paparazzi");
    }

    void Update()
    {
        if (playercoll == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            { return; }
            playercoll = player.GetComponent<Collider2D>();
            if (playercoll == null)
            { return; }
        }
        timer += Time.deltaTime;

        // RESET VISIBILITY EACH FRAME
        playervisible = false;
        corpsevisible = false;
        corpsecoll = null;

        float fov = 90f;
        int rayCount = 90;
        float viewDistance = 5f;
        int mask = ~LayerMask.GetMask("Paparazzi");

        float currentAngle =
            transform.parent.eulerAngles.z - fov / 2f + 90f;

        float angleIncrease = fov / rayCount;
        origin = transform.parent.position;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[3 * rayCount];
        Vector2[] uv = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            float angleRad = currentAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            Vector3 endPoint = origin + dir * viewDistance;
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, mask);

            if (hit.collider != null)
            {
                endPoint = hit.point;

                if (hit.collider == playercoll)
                    playervisible = true;

                if (hit.collider.CompareTag("victims"))
                {
                    Npc_Victims npc = hit.collider.GetComponent<Npc_Victims>();
                    if (npc != null && npc.isDead)
                    {
                        corpsevisible = true;
                        corpsecoll = hit.collider;
                    }
                }
            }

            vertices[vertexIndex] =
                transform.InverseTransformPoint(endPoint);

            if (i > 0)
            {
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = vertexIndex - 1;
                triangles[triangleIndex++] = vertexIndex;
            }

            vertexIndex++;
            currentAngle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        if (!corpsevisible)
        {
            corpseVisiblelastframe = false;
        }

        bool corpseJustbecamevisible = corpsevisible && !corpseVisiblelastframe;

        // ================= FINAL DECISION =================

        // CASE 1: Player + corpse seen → GAME OVER
        if (playervisible && corpsevisible)
        {
            InsanityMeter.instance.Current = 100f;
            return;
        }

        // CASE 2: Only corpse seen → distance insanity
        if (corpseJustbecamevisible && !playervisible && !isCheckingcorpse)
        {
            StartCoroutine(CheckCorpse(corpsecoll));
        }

        corpseVisiblelastframe = corpsevisible;
    }

    IEnumerator CheckCorpse(Collider2D corpse)
    {
        if (corpse == null)
        {
            isCheckingcorpse = false;
            yield break;
        }
        Npc_Victims npc = corpse.GetComponent<Npc_Victims>();
        if (npc == null || npc.hasBeenscanned)
        {
            yield break;
        }
        npc.hasBeenscanned = true;
        isCheckingcorpse = true;
        GetComponentInParent<Npc_Paparazzi>().agent.isStopped = true;

        yield return new WaitForSeconds(scanTime);

        float dist = Vector2.Distance(
            playercoll.bounds.center,
            corpse.bounds.center
        );

        if (dist < lowkilldist)
            InsanityMeter.instance.ApplyInsanity(highinsanity);
        else if (dist < medkilldist)
            InsanityMeter.instance.ApplyInsanity(medinsanity);
        else
            InsanityMeter.instance.ApplyInsanity(lowinsanity);

        GetComponentInParent<Npc_Paparazzi>().agent.isStopped = false;
        isCheckingcorpse = false;

        Destroy(corpse.gameObject);
    }
}
