
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(rotation * Time.deltaTime);
    }
}
