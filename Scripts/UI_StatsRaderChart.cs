using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRaderChart : MonoBehaviour
{
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;

    private void Awake()
    {
       radarMeshCanvasRenderer = GameObject.Find("raderMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats)
    {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }
    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 20f;
        Vector3 HeadVertax = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Head);
        int HeadVertexIndex = 1;
        Vector3 RHVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.RH);
        int RHVertexIndex = 2;
        Vector3 RFVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.RF);
        int RFVertexIndex = 3;
        Vector3 LFVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.LF);
        int LFVertexIndex = 4;
        Vector3 LHVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.LH);
        int LHVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[HeadVertexIndex] = HeadVertax;
        vertices[RHVertexIndex] = RHVertex;
        vertices[RFVertexIndex] = RFVertex;
        vertices[LFVertexIndex] = LFVertex;
        vertices[LHVertexIndex] = LHVertex;

        uv[0] = Vector2.zero;
        uv[HeadVertexIndex] = Vector2.one;
        uv[RHVertexIndex] = Vector2.one;
        uv[RFVertexIndex] = Vector2.one;
        uv[LFVertexIndex] = Vector2.one;
        uv[LHVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = HeadVertexIndex;
        triangles[2] = RHVertexIndex;

        triangles[3] = 0;
        triangles[4] = RHVertexIndex;
        triangles[5] = RFVertexIndex;

        triangles[6] = 0;
        triangles[7] = RFVertexIndex;
        triangles[8] = LFVertexIndex;

        triangles[9] = 0;
        triangles[10] = LFVertexIndex;
        triangles[11] = LHVertexIndex;

        triangles[12] = 0;
        triangles[13] = LHVertexIndex;
        triangles[14] = HeadVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);
    }
}
