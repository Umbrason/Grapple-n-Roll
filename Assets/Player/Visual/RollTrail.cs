
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CollisionInfo))]
public class RollTrail : MonoBehaviour
{

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();

    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private SphereCollider cached_SphereCollider;
    private SphereCollider SphereCollider => cached_SphereCollider ??= GetComponent<SphereCollider>();

    [SerializeField] private Material SharedMaterial;
    private Material cached_materialInstance;
    private Material MaterialInstance => cached_materialInstance ??= Instantiate(SharedMaterial);

    private List<Knot> Knots = new();
    private Mesh mesh;
    [SerializeField] private float height = 1f;
    [SerializeField] private float heightMultiplier = .3f;
    [SerializeField] private AnimationCurve HeightCurve;
    [SerializeField] private float gapWidth = .4f;
    [SerializeField] private float lifetime = .4f;
    private float minSpeed = 5f;

    void Update()
    {
        if (CI.Grounded)
        {
            var currentPos = transform.position - Vector3.up * SphereCollider.radius;
            currentPos += RB.velocity.normalized * (SphereCollider.radius * .5f + Time.deltaTime * RB.velocity.magnitude);

            TrySpawnKnot(currentPos);
        }
        while (Knots.Count > 0 && Time.time - Knots[0].SpawnTime > lifetime)
            Knots.RemoveAt(0);

        UpdateMesh();
        if (mesh == null) return;
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, MaterialInstance, 0);
    }

    private void TrySpawnKnot(Vector3 position)
    {
        if (Knots.Count > 0)
        {
            var lastKnot = Knots.Last();
            var distance = lastKnot.Position - position;
            if (distance.magnitude < minSpeed * Time.fixedDeltaTime)
                return;
        }
        Knots.Add(new(position, Time.time));
    }

    private struct Knot
    {
        public Vector3 Position;
        public float SpawnTime;

        public Knot(Vector3 position, float time)
        {
            this.Position = position;
            this.SpawnTime = time;
        }
    }

    private readonly int[] SegmentIndexTable =
    new int[] {
        0, 1, 4,
        1, 5, 4,
        2, 3, 7,
        2, 7, 6,
    };
    private readonly Vector3[] normalsTable =
    new Vector3[]
    {
        new(0,1,0),
        new(0,1,0),
        new(0,1,0),
        new(0,1,0),
    };

    private void UpdateMesh()
    {
        if (Knots.Count < 2)
        {
            mesh = null;
            return;
        }
        List<Vector3> positions = new();
        List<Vector3> normals = new();
        List<Vector2> uvs = new();
        List<int> indices = new();

        float trailLength = 0f;
        for (int i = 1; i < Knots.Count; i++)
            trailLength += (Knots[i - 1].Position - Knots[i].Position).magnitude;
        float distance = 0f;
        for (int i = 1; i < Knots.Count; i++)
        {
            var currentKnot = Knots[i - 1].Position;
            var nextKnot = Knots[i].Position;
            var delta = nextKnot - currentKnot;

            var t = 1 - (distance / trailLength);
            distance += delta.magnitude;

            var height = HeightCurve.Evaluate(t) * heightMultiplier;
            var cross = Vector3.Cross(delta.normalized, Vector3.up).normalized;

            var currentPoints = new Vector3[] {
                currentKnot + ( cross + Vector3.up * this.height * height),
                currentKnot +   cross * gapWidth,
                currentKnot +  -cross * gapWidth,
                currentKnot + (-cross + Vector3.up * this.height * height)
            };
            positions.AddRange(currentPoints);
            normals.AddRange(normalsTable);

            var currentUVs = new Vector2[] {
                new(t, 1),
                new(t, 0),
                new(t, 0),
                new(t, 1),
            };
            uvs.AddRange(currentUVs);

            if (i == 1) continue;
            var indexOffset = positions.Count - currentPoints.Length * 2;
            var currentIndices = SegmentIndexTable.Select(index => index + indexOffset);
            indices.AddRange(currentIndices);
        }
        mesh ??= new();
        mesh.Clear();
        mesh.SetVertices(positions);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
    }
}



