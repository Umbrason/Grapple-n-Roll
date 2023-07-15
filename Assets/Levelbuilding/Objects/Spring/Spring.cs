
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private AnimationCurve activationCurve;
    [SerializeField] private float cooldownTime = 3f;

    [SerializeField] SkinnedMeshRenderer mr;

    [SerializeField] float springHeight;

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody == null) return;
        var velocity = Mathf.Sqrt(2 * -Physics.gravity.y * springHeight);
        c.attachedRigidbody.velocity = c.attachedRigidbody.velocity._x0z() + velocity * Vector3.up;
        TriggerTime = Time.time;
    }


    private float TriggerTime = float.MinValue;
    private float t => (Time.time - TriggerTime) / cooldownTime;
    void Update()
    {
        if (t >= 1) return;
        var blendShapeValue = activationCurve.Evaluate(t);
        mr.SetBlendShapeWeight(0, blendShapeValue * 100);
    }
}
