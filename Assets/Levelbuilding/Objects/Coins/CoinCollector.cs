
using UnityEngine;

public class CoinCollector : MonoBehaviour, ICoinCollector
{
    [SerializeField] private AudioSource CoinCollectSource;
    [SerializeField] private AudioClip[] CoinCollectSounds;

    public int collectedCoins;

    private float pitchBuildup = 1f;
    public void Collect()
    {
        collectedCoins++;
        pitchBuildup += .05f;
        CoinCollectSource.pitch = pitchBuildup;
        CoinCollectSource.PlayOneShot(CoinCollectSounds[Random.Range(0, CoinCollectSounds.Length)]);
    }

    public void Update()
    {
        pitchBuildup = Mathf.MoveTowards(pitchBuildup, 1f, Time.deltaTime);
    }
}
