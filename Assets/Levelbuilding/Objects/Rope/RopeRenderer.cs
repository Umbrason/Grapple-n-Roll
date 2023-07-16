
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    Mesh mesh;

    [SerializeField] private Material SharedMaterial;
    private Material cached_materialInstance;
    private Material MaterialInstance => cached_materialInstance ??= Instantiate(SharedMaterial);

    public int segments;
    public float radius;

    private readonly List<Point> points = new();

    public struct Point
    {
        public Vector3 position;
        public Vector3 direction;
    }

    void GenerateMesh()
    {
        var indices = new List<int>();
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();

        foreach (var point in points)
        {
            var cross = Vector3.Cross(Vector3.up, point.direction).normalized;
            for (int i = 0; i < segments; i++)
            {
                var angle = i / (float)segments * 360;
                var offset = Quaternion.AngleAxis(angle, point.direction) * cross;
                vertices.Add(point.position + offset);
                if (i > 0)
                {
                    indices.Add(i - segments);
                    indices.Add(i - segments);
                    indices.Add(i - segments);
                    indices.Add(i - segments);
                    indices.Add(i - segments);
                    indices.Add(i - segments);
                }
            }
        }
    }

    void Update()
    {
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, MaterialInstance, 0);

    }
}
