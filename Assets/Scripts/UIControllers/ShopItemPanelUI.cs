using Assets.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanelUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text descriptionText;
    public Image icon;
    public Button buyButton;

    [Header("Visuals")]
    public Image buyButtonBackground;
    public Color affordableColor = Color.white;
    public Color unaffordableColor = Color.gray;

    private IItem _item;
    private Action<ShopItemPanelUI, IItem> _onBuy;
    private bool _canAfford;

    public void Bind(IItem item, string description, Sprite sprite, Action<ShopItemPanelUI, IItem> onBuy, bool canAfford)
    {
        _item = item;
        _onBuy = onBuy;

        nameText.text = item.Name;
        priceText.text = item.Value.ToString();
        descriptionText.text = description;

        if (icon != null) icon.sprite = sprite;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => _onBuy?.Invoke(this, _item));

        SetAffordability(canAfford);
    }

    public void UpdateAffordability(int currentGold)
    {
        if (_item == null) return;
        SetAffordability(currentGold >= _item.Value);
    }

    private void SetAffordability(bool canAfford)
    {
        _canAfford = canAfford;

        var bg = buyButtonBackground != null
            ? buyButtonBackground
            : buyButton.targetGraphic as Image;

        buyButton.interactable = canAfford;
        if (bg != null)
            bg.color = canAfford ? affordableColor : unaffordableColor;
    }
}