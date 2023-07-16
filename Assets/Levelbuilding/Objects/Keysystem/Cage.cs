
using System.Collections;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public static Cage Instance;

    [SerializeField] private GameObject Bars;
    [SerializeField] private GameObject FracturedBars;

    [SerializeField] private AudioSource UnlockSFX;

    void OnEnable() => Instance = this;
    void OnDisable() => Instance = Instance == this ? null : Instance;
    [field: SerializeField] public int LockCount { get; private set; }
    void OnCollisionEnter(Collision c)
    {
        var keyChain = c.collider.GetComponentInParent<Keychain>();
        if (keyChain == null) return;
        if (keyChain.KeyCount < LockCount) return;
        StartCoroutine(UnlockRoutine());
    }

    private IEnumerator UnlockRoutine()
    {

        UnlockSFX.Play();
        yield return new WaitForSeconds(UnlockSFX.clip.length * 2f);
        Bars.SetActive(false);
        FracturedBars.SetActive(true);
        foreach (var rb in FracturedBars.GetComponentsInChildren<Rigidbody>())
            rb.AddExplosionForce(5f, transform.position, 10f, .2f, ForceMode.VelocityChange);
        Destroy(this);
    }
}
