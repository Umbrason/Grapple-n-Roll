
using UnityEngine;

public class WoodBounce : MonoBehaviour
{
    [SerializeField] private AudioClip[] Clips;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private AnimationCurve ImpactToVolume;

    void OnCollisionEnter(Collision c)
    {
        var maxVel = 0f;
        foreach (var contact in c.contacts)
            maxVel = Mathf.Max(maxVel, Vector3.Dot(contact.normal, c.relativeVelocity));
        var volume = ImpactToVolume.Evaluate(maxVel);
        if (volume <= 0) return;
        SFXSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)], volume);
    }
}
