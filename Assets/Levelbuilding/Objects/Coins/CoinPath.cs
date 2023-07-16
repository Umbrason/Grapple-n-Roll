using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CoinPath : MonoBehaviour
{

    [SerializeField] private SplineContainer splineContainer;
    [SerializeField, HideInInspector] private List<GameObject> Coins;
    [SerializeField] private float Spacing = 1f;
    [SerializeField] private GameObject CoinPrefab;
    void OnEnable()
    {
        splineContainer.Spline.changed += OnSplineChanged;
    }

    void OnDisable()
    {
        splineContainer.Spline.changed -= OnSplineChanged;
    }

    void OnSplineChanged() => Dirty = true;
    void OnValidate() => Dirty = true;

    private bool Dirty;
    void Update()
    {
        if (!Dirty) return;
        Dirty = false;
        PlaceCoins();
    }

    private void PlaceCoins()
    {
        var spline = splineContainer.Spline;
        var length = spline.GetLength();
        var CoinCount = Mathf.CeilToInt(length / Spacing);
        if (spline.Closed) CoinCount--;
        while (Coins.Count > CoinCount)
        {
            DestroyImmediate(Coins[0]);
            Coins.RemoveAt(0);
        }
        for (int i = 0; i <= CoinCount; i++)
        {
            var coin = Coins.Count > i ? Coins[i] : null;
            if (coin == null)
            {

                coin = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(CoinPrefab, transform);
                Coins.Add(coin);
            }
            spline.Evaluate(i / (float)CoinCount, out var position, out var tangent, out var upvector);
            coin.transform.localPosition = position;
        }
    }

}