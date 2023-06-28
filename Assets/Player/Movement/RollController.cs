
using UnityEngine;

[RequireComponent(typeof(CollisionInfo), typeof(Rigidbody))]
public class RollController : MonoBehaviour
{
    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();

    private SphereCollider cached_SphereCollider;
    private SphereCollider SphereCollider => cached_SphereCollider ??= GetComponent<SphereCollider>();
    private Vector2 MovementInput = new(0, 0);

    private float speed = 30f;

    void Start()
    {
        RollingInputEvents.OnRollDirectionChanged += dir => MovementInput = dir;
    }

    void Destroy()
    {
        RollingInputEvents.OnRollDirectionChanged -= dir => MovementInput = dir;
    }

    void FixedUpdate()
    {
        RB.useGravity = !CI.FlatGround;
        if (CI.FlatGround) DoGroundedMovement();
        else if (CI.Grounded) DoSlopeMovement();
        else DoAirborneMovement();
    }

    private void DoGroundedMovement()
    {
        var rotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = rotation * MovementInput._x0y() * speed;
        RB.velocity = groundPlaneMovement;
    }

    private void DoSlopeMovement()
    {
        var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
        var slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
        if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;

        var rotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = rotation * MovementInput._x0y() * speed;

        var velocityInMovementDir = Vector3.Dot(RB.velocity, groundPlaneMovement);
        var uphillFactor = Mathf.Max(0, Vector3.Dot(groundPlaneMovement.normalized, -slopeDownVector));
        Debug.Log(uphillFactor);
        RB.velocity += groundPlaneMovement / Mathf.Max(1, velocityInMovementDir) * (1 - uphillFactor);
        RB.velocity += slopeDownVector;
    }

    private void DoAirborneMovement()
    {
        Debug.Log("airborne");
    }

    //for ariborne/slope movement:
    //check dot product with movement direction
    //only apply motion until the dot product matches the max velocity
    //for slope also slow down movement that goes up-hill
}
