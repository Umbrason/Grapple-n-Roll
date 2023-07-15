
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float maxBoostSpeed;
    Vector3 boostDirection => transform.forward;


    void OnTriggerStay(Collider c)
    {
        var rb = c.attachedRigidbody;
        if (rb == null) return;
        var dot = Vector3.Dot(rb.velocity, boostDirection);
        var missingSpeed = Mathf.Max(0, maxBoostSpeed - dot);
        if (missingSpeed <= 0) return;
        rb.velocity += boostDirection * missingSpeed;
    }
}
