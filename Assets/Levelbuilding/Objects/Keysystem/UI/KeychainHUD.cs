
using UnityEngine;
using UnityEngine.UI;

public class KeychainHUD : MonoBehaviour
{
    [SerializeField] private Keychain keychain;
    [SerializeField] private Image keyIconTemplate;


    private Image[] icons;
    void Update()
    {
        if (icons != null || Cage.Instance == null) return;
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
        for (int i = 0; i < enabledCount; i++)
            icons[i].enabled = true;
    }
}
