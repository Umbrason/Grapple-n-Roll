
using UnityEngine;

public class RollSFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private CollisionInfo CI;
    [SerializeField] private Rigidbody RB;

    private float volumeChangeTime = .1f;

    void Update()
    {
        var volumeTarget = CI.Grounded && RB.velocity.magnitude > .5f ? 1 : 0;
        SFXSource.volume = Mathf.MoveTowards(SFXSource.volume, volumeTarget, 1f / volumeChangeTime * Time.deltaTime);
    }
}
