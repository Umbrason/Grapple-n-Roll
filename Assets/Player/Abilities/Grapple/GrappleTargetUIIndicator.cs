
using UnityEngine;
using UnityEngine.UI;

public class GrappleTargetUIIndicator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color inRange;
    [SerializeField] private Color outOfRange;
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private float maxRange = 30f;
    [SerializeField] private Grapple grapple;

    private float maxGrappleAngle = 35;
    private int GrappleLayer => LayerMask.NameToLayer("GrappleTarget");

    void Update()
    {
        var color = CheckGrapple() ? inRange : outOfRange;
        image.color = color;
    }

    bool CheckGrapple()
    {
        if (grapple.InUse) return true;
        if (grapple.CurrentCooldown > 0) return false;
        if (!Physics.Raycast(CameraTransform.position, CameraTransform.forward, out var hit, maxRange - (CameraTransform.position - grapple.transform.position).magnitude))
            return false;
        var angle = Mathf.Acos(-hit.normal.y);
        if ((hit.collider.gameObject.layer != GrappleLayer) || (angle > (maxGrappleAngle + 90) / 180f * Mathf.PI))
            return false;
        return true;
    }
}
