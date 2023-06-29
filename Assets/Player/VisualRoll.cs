

using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CollisionInfo))]
public class VisualRoll : MonoBehaviour
{
    [SerializeField] private Transform RollTarget;

    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();

    private SphereCollider cached_SphereCollider;
    private SphereCollider SphereCollider => cached_SphereCollider ??= GetComponent<SphereCollider>();

    public float AngularDrag = .3f;

    private Quaternion AngularVelocity = Quaternion.identity;
    void FixedUpdate()
    {
        if (CI.Grounded)
        {
            var axis = Vector3.Cross(RB.velocity, Vector3.up);
            var circumference = SphereCollider.radius * Mathf.PI * 2f;
            var angle = 360f * -RB.velocity.magnitude / circumference;
            AngularVelocity = Quaternion.AngleAxis(angle * Time.fixedDeltaTime, axis);
        }
        else
        {
            AngularVelocity = Quaternion.Lerp(AngularVelocity, Quaternion.identity, AngularDrag * Time.fixedDeltaTime);
        }
        RollTarget.transform.rotation = AngularVelocity * RollTarget.transform.rotation;
    }
}
