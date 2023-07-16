
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
    [SerializeField, Tooltip("Downhill acceleration, relative to base speed")] private float SlopeDownwardsAcceleration = 1f;
    [SerializeField, Tooltip("Max downhill speed, relative to base speed")] private float slopeDownwardsSpeed = 1.6666f;

    [Header("Grappling")]
    [SerializeField, Tooltip("Max speed while grappling, relative to base speed")] private float grapplingSpeed = 1.5f;


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
        DoMovemement(Grapple.Grappling ? speed * grapplingSpeed : speed);
    }

    [SerializeField] private float MaxMomentumKeepingTurnAngle = 5f;
    [SerializeField] private float MomentumKeepingTurnAngleFalloff = 10f;

    void DoMovemement(float speed)
    {
        var acceleration = CI.FlatGround ? Acceleration : CI.Grounded ? SlopeAcceleration : AirAcceleration;
        var decceleration = CI.FlatGround ? Decceleration : CI.Grounded ? SlopeDecceleration : AirDecceleration;
        var movementDirection = WorldMovementDirection;

        var slopeDownVector = Vector3.zero;
        var uphillFactor = 0f;
        var DownhillComponent = Vector3.zero;
        var movementPlaneNormal = Vector3.up;
        if (CI.Grounded)
        {
            movementPlaneNormal = CI.ContactNormal;
            var rot = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
            movementDirection = rot * movementDirection;
            if (!CI.FlatGround)
            {
                var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
                slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
                if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;
                uphillFactor = Mathf.Max(0, Vector3.Dot(movementDirection.normalized, -slopeDownVector));
                DownhillComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, slopeDownVector)) * slopeDownVector;
            }
        }

        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity - DownhillComponent, movementDirection)) * movementDirection;
        var movementPlaneSpeed = RB.velocity - Vector3.Dot(RB.velocity, movementPlaneNormal) * movementPlaneNormal;
        var NonForwardComponent = RB.velocity - DownhillComponent - ForwardComponent;

        if (CI.FlatGround)
        {
            var turnAngle = movementDirection.magnitude > 0 ? Vector3.Angle(movementDirection, movementPlaneSpeed) : float.MaxValue;
            var anglePenalty = Mathf.Clamp(turnAngle - MaxMomentumKeepingTurnAngle, 0, MomentumKeepingTurnAngleFalloff) / MomentumKeepingTurnAngleFalloff;
            var angleKeepLerpT = 1 - anglePenalty;
            angleKeepLerpT *= angleKeepLerpT * angleKeepLerpT;
            ForwardComponent = Vector3.Lerp(ForwardComponent, movementPlaneSpeed.magnitude * movementDirection - DownhillComponent, angleKeepLerpT);
            NonForwardComponent = Vector3.Lerp(NonForwardComponent, RB.velocity - movementPlaneSpeed, angleKeepLerpT);
        }

        //Accelerate Downhill until SlopeDownwardsSpeed is reached, then deccelerate to maintain downhillspeed
        DownhillComponent = Vector3.MoveTowards(DownhillComponent, slopeDownVector * speed * slopeDownwardsSpeed, speed * slopeDownwardsSpeed * SlopeDownwardsAcceleration * Time.fixedDeltaTime);
        //Accelerate movement towards MovementDirection or stay at higher movement
        if (ForwardComponent.sqrMagnitude < speed * speed)
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, movementDirection * speed, speed * acceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current MovementDirection
        NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * decceleration * Time.fixedDeltaTime);
        if (!CI.Grounded) RB.velocity = ForwardComponent + DownhillComponent + NonForwardComponent._x0z() + RB.velocity._0y0();
        else RB.velocity = ForwardComponent + NonForwardComponent + DownhillComponent;
    }
    //being grappled should allow for infinite movement. aka. no decelleration in input direction

    //needs rework to fit all states in one method. (input is current maxspeed, acceleration, decelleration and some control bools)
    //needs to be faster when grappling

    //for ariborne/slope movement:
    //check dot product with movement direction
    //only apply motion until the dot product matches the max velocity
    //for slope also slow down movement that goes up-hill
}
