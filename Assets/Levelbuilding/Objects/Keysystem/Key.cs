
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject Visual;
    [SerializeField] private AudioClip[] CollectionClips;
    [SerializeField] private AudioSource SFX;
    [SerializeField] private GameObject ParticleSystem;
    [SerializeField] private float DestroyDelay = 3f;
    void OnTriggerEnter(Collider c)
    {
        var keyChain = c.GetComponentInParent<Keychain>();
        if (keyChain == null) return;
        keyChain.Collect();

        //Destroy Visuals and Script
        Destroy(this);
        Destroy(Visual);

        //Destroy gameObject after delay
        Destroy(gameObject, DestroyDelay);

        //play AVFX
        SFX.clip = CollectionClips[Random.Range(0, CollectionClips.Length)];
        SFX.Play();
        if (ParticleSystem != null) ParticleSystem.SetActive(true);
    }
}
