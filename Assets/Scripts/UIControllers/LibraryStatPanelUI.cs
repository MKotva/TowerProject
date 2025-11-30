using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LibraryStatPanelUI : MonoBehaviour
{
    public TMP_Text statNameText;
    public TMP_Text currentValueText;
    public TMP_Text costText;
    public Button upgradeButton;

    [Header("Visuals")]
    public Image upgradeButtonBackground;
    public Color affordableColor = Color.white;
    public Color unaffordableColor = Color.gray;

    private Action _onUpgrade;
    private int _cost;

    public void Bind(string statName,
                     int currentValue,
                     int cost,
                     bool canAfford,
                     Action onUpgrade)
    {
        statNameText.text = statName;
        currentValueText.text = currentValue.ToString();
        costText.text = cost.ToString();

        _cost = cost;
        _onUpgrade = onUpgrade;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => _onUpgrade?.Invoke());

        SetAffordability(canAfford);
    }

    public void UpdateAffordability(int currentGold)
    {
        SetAffordability(currentGold >= _cost);
    }

    private void SetAffordability(bool canAfford)
    {
        var bg = upgradeButtonBackground != null
            ? upgradeButtonBackground
            : upgradeButton.targetGraphic as Image;

        upgradeButton.interactable = canAfford;
        if (bg != null)
            bg.color = canAfford ? affordableColor : unaffordableColor;
    }
}