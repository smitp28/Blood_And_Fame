using JetBrains.Annotations;
using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class FOV : MonoBehaviour
{
    public GameObject paparazzi;
    public Mesh mesh;
    public Vector3 origin;
    public Collider2D papacoll;
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
    private float scanTime = 1f;
    public bool isCheckingcorpse = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        paparazzi = GameObject.FindWithTag("Paparazzi");
        papacoll = paparazzi.GetComponent<Collider2D>();
        GameObject player = GameObject.FindWithTag("Player");
        playercoll = player.GetComponent<Collider2D>();
        GameObject corpse = GameObject.FindWithTag("Corpse");
        corpsecoll = corpse.GetComponent<Collider2D>();
        GameObject popularitymeter = GameObject.FindWithTag("PopularityMeter");
    }

    // Update is called once per frame
    void Update()
    {
        playervisible = false;
        corpsevisible = false;
        float fov = 90f;
        int rayCount = 90;
        float angle = transform.parent.eulerAngles.z - fov/2f + 90f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;
        int mask = ~LayerMask.GetMask("Paparazzi");
        origin = transform.parent.position;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        int[] triangles = new int[3 * rayCount];
        Vector2[] uv = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 worlddir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            Vector3 worldend = origin + worlddir * viewDistance;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, worlddir, viewDistance, mask);
            if (raycastHit2D.collider == null)
            {
                worldend = origin + worlddir * viewDistance;
            }
            else if (raycastHit2D.collider == playercoll)
            {
                playervisible = true;
            }
            else if (raycastHit2D.collider == corpsecoll)
            { 
                corpsevisible = true;
                if (!isCheckingcorpse)
                {
                    StartCoroutine(CheckCorpse());
                }
            }
            else
            {
                worldend = raycastHit2D.point;
            }
            vertices[vertexIndex] = transform.InverseTransformPoint(worldend);

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void SetOrigin(Vector3 origin)
    { 
        this.origin = origin;
    }

    IEnumerator CheckCorpse()
    {
        if (corpsevisible == true)
        {
            isCheckingcorpse = true;
            GetComponentInParent<Npc_Paparazzi>().agent.isStopped = true;
            yield return new WaitForSeconds(scanTime);
            if (Vector2.Distance(playercoll.bounds.center, corpsecoll.bounds.center) < lowkilldist)
            {
                InsanityMeter.instance.ApplyInsanity(highinsanity);
            }
            else if (Vector2.Distance(playercoll.bounds.center, corpsecoll.bounds.center) < medkilldist)
            {
                InsanityMeter.instance.ApplyInsanity(medinsanity);
            }
            else if (Vector2.Distance(playercoll.bounds.center, corpsecoll.bounds.center) < highkilldist)
            {
                InsanityMeter.instance.ApplyInsanity(lowinsanity);
            }
            GetComponentInParent<Npc_Paparazzi>().agent.isStopped = false;
            isCheckingcorpse = false;
        }
        
    }
}
