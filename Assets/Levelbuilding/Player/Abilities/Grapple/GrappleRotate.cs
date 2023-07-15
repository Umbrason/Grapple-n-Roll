
using UnityEngine;

public class GrappleRotate : MonoBehaviour
{
    [SerializeField] private Grapple grapple;
    Vector3 grappleDirection;

    void Update()
    {
        if (grapple.GrapplePoint == null)
            grappleDirection = Vector3.RotateTowards(grappleDirection, Vector3.up, 15 * Time.deltaTime, 0f);
        else grappleDirection = grapple.GrapplePoint.Value - transform.position;
        transform.up = grappleDirection;
    }
}
