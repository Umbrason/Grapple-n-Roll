
using UnityEngine;

public class CameraClipStopRaycast : MonoBehaviour
{
    float maxDistance = 8f;
    float minDistance = .1f;

    Vector3 currentLocalpos = default;

    private int noPlayerLayerMask;

    private Vector3 RaycastDirection => -transform.parent.forward;
    private Vector3 RaycastOrigin => transform.parent.position + transform.parent.up + minDistance * RaycastDirection;
    void Start()
    {
        currentLocalpos = transform.localPosition;
        noPlayerLayerMask = ~LayerMask.GetMask("Player");
    }
    void LateUpdate()
    {
        //make sure to ignore player layer!
        var deltaToOrigin = RaycastOrigin - transform.parent.position;
        var OriginObstructed = Physics.Raycast(transform.parent.position, deltaToOrigin, deltaToOrigin.magnitude, noPlayerLayerMask);
        if (OriginObstructed)
        {
            transform.localPosition = Vector3.zero;
            return;
        }
        var targetDistance = Physics.Raycast(RaycastOrigin, RaycastDirection, out var hit, maxDistance, noPlayerLayerMask) ? Mathf.Max(minDistance, hit.distance) : maxDistance;
        var lerpFactor = Mathf.Pow(.1f, Time.deltaTime);
        currentLocalpos = Vector3.Lerp(currentLocalpos, Vector3.back * targetDistance + Vector3.up * 2f, lerpFactor);
        transform.localPosition = currentLocalpos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(RaycastOrigin, .1f);
    }
#endif
}
