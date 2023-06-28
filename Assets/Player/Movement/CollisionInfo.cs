
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//idea: rember all collisions, dont allow movement into a wall
public class CollisionInfo : MonoBehaviour
{

    private readonly Dictionary<Collider, Vector3> activeGroundCollisions = new();
    public IReadOnlyDictionary<Collider, Vector3> ActiveGroundCollisions => activeGroundCollisions;
    private readonly Dictionary<Collider, List<Vector3>> activeCollisions = new();
    public IReadOnlyDictionary<Collider, List<Vector3>> ActiveCollisions => activeCollisions;
    public bool Grounded => activeGroundCollisions.Count > 0;
    public bool FlatGround => Grounded && ContactNormal.y >= DotThreshold;

    private Vector3? cached_ContactNormal;
    public Vector3 ContactNormal => cached_ContactNormal ??= activeGroundCollisions.Values.Aggregate((a, b) => a + b).normalized;
    void FixedUpdate() => cached_ContactNormal = null;

    private float cached_dotThreshold = Mathf.Cos(Mathf.PI * .2f);
    private float DotThreshold => cached_dotThreshold;

    private float cached_MaxSlopeRads = Mathf.PI * .2f;
    public float MaxSlopeRads
    {
        get => cached_MaxSlopeRads; set
        {
            cached_dotThreshold = Mathf.Cos(value);
            cached_MaxSlopeRads = value;
        }
    }


    void OnCollisionExit(Collision other)
    {
        activeCollisions.Remove(other.collider);
        activeGroundCollisions.Remove(other.collider);
    }
    void OnCollisionEnter(Collision other)
    {
        var collider = other.collider;
        var normals = other.contacts.Select(c => c.normal).ToList();
        activeCollisions.Add(collider, normals);
        var groundCollisions = ExtractGroundCollisions(collider, normals);
        if (groundCollisions.Count == 0) return;
        activeGroundCollisions.Add(collider, groundCollisions.Aggregate((a, b) => a + b).normalized);
    }


    void OnCollisionStay(Collision other)
    {
        var contactNormals = other.contacts.Select(c => c.normal).ToList();
        var collider = other.collider;
        activeCollisions[collider] = contactNormals;
        var validCollisions = ExtractGroundCollisions(collider, contactNormals);
        if (validCollisions.Count == 0)
        {
            activeGroundCollisions.Remove(collider);
            return;
        }
        var sum = validCollisions.Aggregate((a, b) => a + b);
        activeGroundCollisions[collider] = sum.normalized;
    }

    private List<Vector3> ExtractGroundCollisions(Collider collider, List<Vector3> contactNormals)
    {
        return contactNormals.Where(cp => IsGround(collider, cp)).ToList();
    }

    private bool IsGround(Collider c, Vector3 normal)
    {
        return normal.y >= 0;
    }

    private bool IsWall(Collider c, Vector3 normal)
    {
        return normal.y < 0; //Mathf.Abs(Vector3.Dot(Vector3.up, normal)) <= DotThreshold;
    }

    public Vector3 CollideXZVelocity(Vector3 velocity)
    {
        var vy = velocity.y;
        velocity = velocity._x0z();
        foreach (var pair in activeCollisions)
            foreach (var normal in pair.Value)
            {
                if (!IsWall(pair.Key, normal)) continue;
                var normalXZ = normal._x0z().normalized;
                var dot = Vector3.Dot(velocity, normalXZ);
                if (dot >= 0) continue;
                velocity -= normalXZ * dot;
                if (velocity.magnitude < 0.01f) return Vector3.zero;
            }
        return velocity + Vector3.up * vy;
    }
}
