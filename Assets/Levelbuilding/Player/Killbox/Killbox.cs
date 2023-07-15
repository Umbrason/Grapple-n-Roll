
using UnityEngine;

public class Killbox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        collision.GetComponentInParent<KillboxTarget>()?.Reset();
    }
}
