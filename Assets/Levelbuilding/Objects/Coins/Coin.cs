
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int RemainingCoins { get; private set; }
    void OnTriggerEnter(Collider collider)
    {
        var CoinCollector = collider.GetComponentInParent<ICoinCollector>();
        if (CoinCollector == null) return;
        CoinCollector.Collect();
        Destroy(gameObject);
    }

    void OnEnable()
    {
        RemainingCoins++;
    }

    void OnDisable()
    {
        RemainingCoins--;
    }
}

public interface ICoinCollector
{
    void Collect();
}
