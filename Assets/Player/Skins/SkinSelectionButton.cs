
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private static readonly List<SkinSelectionButton> SkinSelectionButtons = new();

    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Color active;
    [SerializeField] private Color inactive;

    [SerializeField] private float fadeDuration = .1f;

    private bool selected = false;
    private bool hovered = false;

    public int skinIndex = 0;

    private float brightness = 0f;
    private float TargetBrightness => (selected || hovered) ? 1 : 0;

    void OnEnable()
    {
        SkinSelectionButtons.Add(this);
    }

    void OnDisable()
    {
        SkinSelectionButtons.Remove(this);
    }

    void Start()
    {
        selected = skinIndex == PlayerPrefs.GetInt(Playerskins.playerPrefsSkinKey);
        mr.material = Playerskins.skins[skinIndex];
        brightness = TargetBrightness;
        mr.material.color = Color.Lerp(inactive, active, brightness);
    }
    void Update()
    {
        if (brightness == TargetBrightness) return;
        brightness = Mathf.MoveTowards(brightness, TargetBrightness, Time.unscaledDeltaTime / fadeDuration);
        mr.material.color = Color.Lerp(inactive, active, brightness);
    }

    public void Select()
    {
        foreach (var button in SkinSelectionButtons)
            button.selected = button == this;
        PlayerPrefs.SetInt(Playerskins.playerPrefsSkinKey, skinIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }
}
