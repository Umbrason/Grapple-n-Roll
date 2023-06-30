

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
        /* if (CI.Grounded)
        { */
        var hvel = RB.velocity._x0z();
        var axis = Vector3.Cross(hvel, Vector3.up);
        var circumference = SphereCollider.radius * Mathf.PI * 2f;
        var angle = 360f * -hvel.magnitude / circumference / 2f;
        AngularVelocity = Quaternion.AngleAxis(angle * Time.fixedDeltaTime, axis);
        RollTarget.transform.rotation = AngularVelocity * RollTarget.transform.rotation;
        /* }
        else
        {
            AngularVelocity = Quaternion.Lerp(AngularVelocity, Quaternion.identity, AngularDrag * Time.fixedDeltaTime);
        } */
    }
}
