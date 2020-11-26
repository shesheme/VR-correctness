using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    Vector3 currentPos;

    int edge;
    int radius = 6;
    float startArc;
    // Start is called before the first frame update
    void Start()
    {
        edge = 3;

        currentPos = transform.position;
        vertices = new Vector3[edge + 1];
        vertices[0] = currentPos;

        for(int i = 1; i< vertices.Length; i++)
        {
            vertices[i] = new Vector3(currentPos.x + radius * Mathf.Cos(Mathf.Deg2Rad * ((i * 360.0f / edge) + startArc)), currentPos.y + radius * Mathf.Sin(Mathf.Deg2Rad * ((i * 360.0f / edge) + startArc)), currentPos.z);



        }
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.vertices = vertices;

        DrawTriangle();
        mesh.RecalculateNormals();
    }

    void DrawTriangle()
    {
        int[] tri = new int[3 * edge];
        for (int i = 0, k = 0; i < tri.Length - 3; i += 3, k += 2)
        {
            tri[i] = 0;
            tri[i + 1] = i + 1 - k;
            tri[i + 2] = i + 2 - k;
        }

        tri[tri.Length - 1] = 1;
        tri[tri.Length - 2] = edge;
        tri[tri.Length - 3] = 0;

        mesh.triangles = tri;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (vertices == null) return;
        foreach (Vector3 v in vertices)
            Gizmos.DrawSphere(v, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
