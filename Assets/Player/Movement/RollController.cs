
using UnityEngine;

[RequireComponent(typeof(CollisionInfo), typeof(Rigidbody), typeof(Grapple))]
public class RollController : MonoBehaviour
{
    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();

    private Grapple cached_Grapple;
    private Grapple Grapple => cached_Grapple ??= GetComponent<Grapple>();

    private SphereCollider cached_SphereCollider;
    private SphereCollider SphereCollider => cached_SphereCollider ??= GetComponent<SphereCollider>();
    private Vector2 RawMovementInput = new(0, 0);

    [SerializeField] private Transform cameraTransform;
    private Vector3 WorldMovementDirection => (Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * RawMovementInput._x0y()).normalized;

    [Header("Base Speed"), Tooltip("The base speed of the ball")]
    [SerializeField] private float speed = 15f;        

    [Header("Grounded")]
    [SerializeField, Tooltip("Airborne decceleration, relative to base speed")] private float Acceleration = 2f;
    [SerializeField, Tooltip("Airborne decceleration, relative to base speed")] private float Decceleration = 3.3333f;

    [Header("GroundedAirborn")]
    [SerializeField, Tooltip("Airborn decceleration, relative to base speed")] private float AirAcceleration = 1;
    [SerializeField, Tooltip("Airborn decceleration, relative to base speed")] private float AirDecceleration = 1.3333f;

    [Header("Sliding")]
    [SerializeField, Tooltip("Sliding decceleration, relative to base speed")] private float SlopeAcceleration = 1;
    [SerializeField, Tooltip("Sliding decceleration, relative to base speed")] private float SlopeDecceleration = 1.3333f;
    [SerializeField, Tooltip("Downhill acceleration, relative to base speed")] private float SlopeDownwardsAcceleration = .3333f;
    [SerializeField, Tooltip("Max downhill speed, relative to base speed")] private float slopeDownwardsSpeed = 1.6666f;

    [Header("Grappling")]
    [SerializeField, Tooltip("Max speed while grappling, relative to base speed")] private float grapplingSpeed = 2f;


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
        if (ForwardComponent.sqrMagnitude < speed * speed)
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, groundPlaneMovement * speed, speed * Acceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current groundPlaneMovement
        NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * Decceleration * Time.fixedDeltaTime);

        RB.velocity = ForwardComponent + NonForwardComponent;

    }

    private void DoSlopeMovement()
    {
        var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
        var slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
        if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;

        var rotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = rotation * WorldMovementDirection * speed;

        var DownhillComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, slopeDownVector)) * slopeDownVector;
        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity - DownhillComponent, groundPlaneMovement)) * groundPlaneMovement;
        var NonForwardComponent = RB.velocity - DownhillComponent - ForwardComponent;


        var uphillFactor = Mathf.Max(0, Vector3.Dot(groundPlaneMovement.normalized, -slopeDownVector));

        DownhillComponent = Vector3.MoveTowards(DownhillComponent, slopeDownVector * speed * slopeDownwardsSpeed, speed * slopeDownwardsSpeed * SlopeDownwardsAcceleration * Time.fixedDeltaTime);
        //Accelerate movement towards groundPlaneMovement
        if (ForwardComponent.sqrMagnitude < speed * speed * (1 - uphillFactor))
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, groundPlaneMovement * speed * (1 - uphillFactor), speed * SlopeAcceleration * Time.fixedDeltaTime);
        //Decellerate movement that doesnt go towards current groundPlaneMovement
        NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * SlopeDecceleration * Time.fixedDeltaTime);

        RB.velocity = ForwardComponent + NonForwardComponent + DownhillComponent;
    }

    private void DoAirborneMovement()
    {
        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, WorldMovementDirection)) * WorldMovementDirection;
        var NonForwardComponent = RB.velocity - ForwardComponent;

        //Accelerate movement towards groundPlaneMovement
        if (ForwardComponent.sqrMagnitude < speed * speed)
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, WorldMovementDirection * speed, speed * AirAcceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current groundPlaneMovement
        if (!Grapple.Grappling)
            NonForwardComponent = Vector3.MoveTowards(NonForwardComponent._x0z(), Vector3.zero, speed * AirDecceleration * Time.fixedDeltaTime);

        RB.velocity = ForwardComponent + NonForwardComponent._x0z() + RB.velocity._0y0();
    }

    //being grappled should allow for infinite movement. aka. no decelleration in input direction

    //needs rework to fit all states in one method. (input is current maxspeed, acceleration, decelleration and some control bools)
    //needs to be faster when grappling

    //for ariborne/slope movement:
    //check dot product with movement direction
    //only apply motion until the dot product matches the max velocity
    //for slope also slow down movement that goes up-hill
}
