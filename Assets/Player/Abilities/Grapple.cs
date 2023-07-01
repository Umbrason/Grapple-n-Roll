
using UnityEngine;

public class Grapple : MonoBehaviour, IPlayerAbilityWithCooldown
{
    private SpringJoint SJ;
    [SerializeField] private Transform CameraTransform;

    [SerializeField] public float maxRange = 30f;
    [SerializeField] public float minRange = 15f;
    [SerializeField] public float reelInSpeed = 1f;

    [SerializeField] private LineRenderer LR;
    [SerializeField] private Transform GrappleOrigin;

    public Vector3? GrapplePoint => SJ?.connectedAnchor;

    public bool Grappling => !(SJ == null);

    private float GrappleStopTime = float.MinValue;
    public float CurrentCooldown => Mathf.Clamp(CooldownDuration - (Time.fixedTime - GrappleStopTime), 0, CooldownDuration);
    [field: SerializeField] public float CooldownDuration { get; private set; } = 3f;
    public bool InUse => Grappling;

    void Start()
    {
        RollingInputEvents.OnGrappleInputChanged += OnGrappleInputChanged;
    }

    void Destroy()
    {
        RollingInputEvents.OnGrappleInputChanged -= OnGrappleInputChanged;
    }

    void OnGrappleInputChanged(bool grappleHeld)
    {
        if (!grappleHeld)
        {
            ReleaseGrapple();
            return;
        }
        if (CurrentCooldown > 0) return;
        if (!Physics.Raycast(CameraTransform.position, CameraTransform.forward, out var hit, maxRange - (CameraTransform.position - transform.position).magnitude)) return;
        if (hit.normal.y <= 0) AttachGrapple(hit.point);
    }

    void AttachGrapple(Vector3 targetPosition)
    {
        GrappleStopTime = float.MaxValue;
        var initialDistance = (transform.position - targetPosition).magnitude;
        SJ = gameObject.AddComponent<SpringJoint>();
        SJ.spring = 1000f;
        SJ.autoConfigureConnectedAnchor = false;
        SJ.connectedAnchor = targetPosition;

        SJ.maxDistance = Mathf.Max(minRange, initialDistance);
        LR.positionCount = 2;
        LR.SetPositions(new Vector3[] { targetPosition, GrappleOrigin.position });
    }

    void FixedUpdate()
    {
        if (!SJ) return;
        var currDistance = (transform.position - SJ.connectedAnchor).magnitude;
        SJ.maxDistance = Mathf.Clamp(Mathf.MoveTowards(SJ.maxDistance, minRange, reelInSpeed * Time.fixedDeltaTime), minRange, currDistance);
    }

    void Update()
    {
        if (!SJ) return;
        LR.SetPositions(new Vector3[] { SJ.connectedAnchor, GrappleOrigin.position });
    }

    void ReleaseGrapple()
    {
        if (SJ == null) return;
        GrappleStopTime = Time.fixedTime;
        LR.positionCount = 0;
        Destroy(SJ);
        SJ = null;
    }

}
