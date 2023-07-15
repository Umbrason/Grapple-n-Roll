using UnityEngine;
class TestT
{
    private Rigidbody cached_RB;
    private Rigidbody RB => null;

    private CollisionInfo cached_CI;
    private CollisionInfo CI => null;

    private Grapple cached_Grapple;
    private Grapple Grapple => null;

    private SphereCollider cached_SphereCollider;
    private SphereCollider SphereCollider => null;
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
    [SerializeField, Tooltip("Max speed while grappling, relative to base speed")] private float grapplingSpeed = 2f;
    void DoGroundedMovement()
    {
        var groundPlaneRotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = groundPlaneRotation * WorldMovementDirection;

        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, groundPlaneMovement)) * groundPlaneMovement;
        var NonForwardComponent = RB.velocity - ForwardComponent;

        //Accelerate movement towards groundPlaneMovement
        if (ForwardComponent.sqrMagnitude < speed * speed)
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, groundPlaneMovement * speed, speed * Acceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current groundPlaneMovement
        if (!Grapple.Grappling)
            NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * Decceleration * Time.fixedDeltaTime);

        RB.velocity = ForwardComponent + NonForwardComponent;
    }

    void DoSlopeMovement()
    {
        var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
        var slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
        if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;

        var groundPlaneRotation = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
        var groundPlaneMovement = groundPlaneRotation * WorldMovementDirection * speed;

        var uphillFactor = Mathf.Max(0, Vector3.Dot(groundPlaneMovement.normalized, -slopeDownVector));

        var DownhillComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, slopeDownVector)) * slopeDownVector;
        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity - DownhillComponent, groundPlaneMovement)) * groundPlaneMovement;
        var NonForwardComponent = RB.velocity - DownhillComponent - ForwardComponent;

        //Accelerate towards downhill untill downhillspeed is reached
        if (DownhillComponent.sqrMagnitude < slopeDownwardsSpeed * slopeDownwardsSpeed)
            DownhillComponent = Vector3.MoveTowards(DownhillComponent, slopeDownVector * speed * slopeDownwardsSpeed, speed * slopeDownwardsSpeed * SlopeDownwardsAcceleration * Time.fixedDeltaTime);

        //Accelerate movement towards groundPlaneMovement
        if (ForwardComponent.sqrMagnitude < speed * speed * (1 - uphillFactor))
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, groundPlaneMovement * speed * (1 - uphillFactor), speed * SlopeAcceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current groundPlaneMovement
        if (!Grapple.Grappling)
            NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * SlopeDecceleration * Time.fixedDeltaTime);

        RB.velocity = ForwardComponent + NonForwardComponent + DownhillComponent;
    }

    void DoAirborneMovement()
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
}