
using UnityEngine;

public class RollSFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private CollisionInfo CI;
    [SerializeField] private Rigidbody RB;

    private float volumeChangeTime = .1f;    

    void Update()
    {
        var volumeTarget = CI.Grounded && RB.velocity.magnitude > .5f ? 1f : 0f;
        volumeTarget *= Time.timeScale;
        SFXSource.volume = Mathf.MoveTowards(SFXSource.volume, volumeTarget, 1f / volumeChangeTime * Time.unscaledDeltaTime);
        SFXSource.enabled = SFXSource.volume > 0;
    }
}
