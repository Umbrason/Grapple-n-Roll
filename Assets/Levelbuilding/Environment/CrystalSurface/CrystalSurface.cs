
using UnityEngine;

public class CrystalSurface : MonoBehaviour
{
    public AudioSource[] sources;
    public AnimationCurve ForceToVolume = AnimationCurve.Linear(0, 0, 20, 1);

    public void OnCollisionEnter(Collision c)
    {
        var impactVelocity = Vector3.Dot(c.contacts[0].normal, c.relativeVelocity);
        var impactForce = c.rigidbody.mass * impactVelocity;
        var volume = ForceToVolume.Evaluate(impactForce);


        foreach (var source in sources)
        {
            if (source.isPlaying) continue;
            source.transform.position = c.contacts[0].point;
            source.volume = volume;
            source.Play();
            break;
        }
    }
}
