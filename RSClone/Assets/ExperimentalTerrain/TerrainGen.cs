using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGen : MonoBehaviour {

    public Texture2D heightMap;
    public float height = 1.0f;
    public int xSize, ySize;

    private float[] mapheights;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Mesh mesh;

    private void Awake()
    {
        Generate();
    }

    private float[] GetHeightMap()
    {
        float[] heights = new float[(xSize + 1) * (ySize + 1)];
        for (int x = 0; x <= xSize; x++)
        {
            for(int y = 0; y <= ySize; y++)
            {
                heights[x + (y * ySize)] = height * (heightMap.GetPixel(x,y).r - 0.5f);
            }
        }

        return heights;
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Map";
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        uvs = new Vector2[(xSize + 1) * (ySize + 1)];
        mapheights = GetHeightMap();
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, mapheights[x + (y * ySize)], y);
                uvs[i] = new Vector2((float)x / (float)xSize, (float)y / (float)ySize);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
