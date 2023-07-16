using System.Collections.Generic;
using UnityEngine;

public class KillboxTarget : MonoBehaviour
{
    [SerializeField] private CollisionInfo info;
    [SerializeField] private Rigidbody RB;

    public int ResetCounter { get; private set; }

    private readonly List<Vector3> savePosition = new();
    void FixedUpdate()
    {
        if (info.FlatGround) savePosition.Add(RB.position);
        if (savePosition.Count > .1f / Time.fixedDeltaTime) savePosition.RemoveAt(0);
    }

    public void Reset()
    {
        RB.position = savePosition[0];
        RB.velocity = Vector3.zero;
        RB.useGravity = false;
        ResetCounter++;
    }
}
