
using UnityEngine;
using UnityEngine.UI;

public class KeychainHUD : MonoBehaviour
{
    [SerializeField] private Keychain keychain;
    [SerializeField] private Image keyIconTemplate;


    private Image[] icons;
    void Start()
    {
        if (icons != null) foreach (var icon in icons) Destroy(icon.gameObject);
        icons = new Image[Cage.Instance.LockCount];
        for (int i = 0; i < icons.Length; i++)
            icons[i] = Instantiate(keyIconTemplate, transform).transform.GetChild(0).GetComponent<Image>();
    }

    void OnEnable()
    {
        keychain.OnKeyCountChanged += UpdateIcons;
    }

    void OnDisable()
    {
        keychain.OnKeyCountChanged -= UpdateIcons;
    }

    void UpdateIcons(int enabledCount)
    {
        Debug.Log(enabledCount);
        for (int i = 0; i < enabledCount; i++)
            icons[i].enabled = true;
    }
}
