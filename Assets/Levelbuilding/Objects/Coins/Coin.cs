
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        var CoinCollector = collider.GetComponentInParent<ICoinCollector>();
        if (CoinCollector == null) return;
        CoinCollector.Collect();
        Destroy(gameObject);
    }
}

public interface ICoinCollector
{
    void Collect();
}
