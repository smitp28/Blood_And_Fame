using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class FOV : MonoBehaviour
{
    public GameObject paparazzi;
    public Mesh mesh;
    public Vector3 origin;
    public Collider2D pcoll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        paparazzi = GameObject.FindWithTag("Paparazzi");
    }

    // Update is called once per frame
    void Update()
    {
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
}
