
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
    private Vector2 RawMovementInput = new(0, 0);

    [SerializeField] private Transform cameraTransform;
    private Vector3 WorldMovementDirection => Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * RawMovementInput._x0y();

    public float Acceleration = 15f;
    public float Decceleration = 5f;
    public float AirAcceleration = .1f;

    [SerializeField] private float speed = 15f;

    void Start()
    {
        RollingInputEvents.OnRollDirectionChanged += dir => RawMovementInput = dir;
    }

    void Destroy()
    {
        RollingInputEvents.OnRollDirectionChanged -= dir => RawMovementInput = dir;
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
        var groundPlaneMovement = rotation * WorldMovementDirection;
        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, groundPlaneMovement)) * groundPlaneMovement;
        var NonForwardComponent = RB.velocity - ForwardComponent;

        //Accelerate movement towards groundPlaneMovement
        ForwardComponent = Vector3.MoveTowards(ForwardComponent, groundPlaneMovement * speed, Acceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current groundPlaneMovement
        NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, Decceleration * Time.fixedDeltaTime);


        RB.velocity = ForwardComponent + NonForwardComponent;

    }

    private void DoSlopeMovement()
    {
        var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
        var slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
        if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;

        var rotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = rotation * WorldMovementDirection * speed;

        var velocityInMovementDir = Vector3.Dot(RB.velocity, groundPlaneMovement) / speed;
        var uphillFactor = Mathf.Max(0, Vector3.Dot(groundPlaneMovement.normalized, -slopeDownVector));

        Debug.Log(uphillFactor);
        RB.velocity += groundPlaneMovement * AirAcceleration * ((speed - velocityInMovementDir) / speed) * (1 - uphillFactor);
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
