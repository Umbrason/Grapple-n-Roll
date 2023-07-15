
using UnityEngine;

[RequireComponent(typeof(CollisionInfo), typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private int jumpCount;
    [SerializeField] private float CoyoteTime = .1f;

    [SerializeField] private float JumpHeight;

    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();


    private bool JumpInput;

    void Start()
    {
        RollingInputEvents.OnJumpInputChanged += JumpInputChanged;
    }

    void Destroy()
    {
        RollingInputEvents.OnJumpInputChanged -= JumpInputChanged;
    }

    private int jumpsPerformed;
    void DoJump()
    {
        if (jumpCount <= jumpsPerformed + 1) return;
        if (CoyoteTime < Time.fixedTime - lastGroundTime) jumpsPerformed++;
        var velocity = Mathf.Sqrt(2 * -Physics.gravity.y * JumpHeight);
        RB.velocity = RB.velocity._x0z() + Vector3.up * velocity;
        lastGroundTime = float.MinValue;
    }

    void JumpInputChanged(bool jumpInput)
    {
        this.JumpInput = jumpInput;
        if (jumpInput) DoJump();
    }

    private float lastGroundTime;
    void FixedUpdate()
    {
        if (!CI.FlatGround) return;
        lastGroundTime = Time.fixedTime;
        if (JumpInput) DoJump();
        jumpsPerformed = 0;
    }


}
