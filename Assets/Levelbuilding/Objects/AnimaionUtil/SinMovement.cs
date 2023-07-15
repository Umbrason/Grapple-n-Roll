
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    [SerializeField] private Vector3 Direction;
    [SerializeField] private float Amplitude;
    [SerializeField] private float Frequency;
    [SerializeField] private float Offset = 1f;
    void Update()
    {
        transform.localPosition = (Mathf.Sin(Time.time * Frequency * Mathf.PI * 2f) * Amplitude + Offset) * Direction;
    }
}
