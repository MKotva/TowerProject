using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Core;
using Assets.Core.Items;
using Assets.Scripts;

public class PlayerEquipmentPanel : MonoBehaviour
{
    [Header("Weapon & Armor UI")]
    public Image weaponImage;
    public Button sellWeaponButton;

    public Image armorImage;
    public Button sellArmorButton;

    [Header("Potions UI")]
    public Image hpPotionImage;
    public TMP_Text hpPotionCountText;
    public Button sellHpPotionButton;

    public Image staminaPotionImage;
    public TMP_Text staminaPotionCountText;
    public Button sellStaminaPotionButton;

    [Header("Colors")]
    public Color hpNormalColor = Color.white;
    public Color staminaNormalColor = Color.white;
    public Color emptyColor = Color.gray;

    [Header("Tooltips")]
    public GameObject tooltipPanel;
    public TMP_Text tooltipTitleText;
    public TMP_Text tooltipBodyText;

    [Header("Announcements")]
    public Transform uiRoot;
    public GameObject announcementPrefab; 

    private Potion _hpPotionExample;
    private Potion _staminaPotionExample;

    private void Awake()
    {
        HideTooltip();
    }

    private void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        var gm = GameManager.Instance;
        var player = gm.PlayerController;

        Weapon currentWeapon = player.Weapon;
        bool isDefaultWeapon = ( currentWeapon != null &&
                                gm.DefaultWeapon != null &&
                                currentWeapon == gm.DefaultWeapon );

        if (weaponImage != null)
        {
            if (currentWeapon != null && currentWeapon.Icon != null)
            {
                weaponImage.sprite = currentWeapon.Icon;
                weaponImage.color = Color.white;
            }
            else
            {
                weaponImage.sprite = null;
                weaponImage.color = emptyColor;
            }
        }

        if (sellWeaponButton != null)
        {
            bool canSellWeapon = currentWeapon != null && !isDefaultWeapon;
            sellWeaponButton.interactable = canSellWeapon;
            TintButton(sellWeaponButton, canSellWeapon);
            sellWeaponButton.onClick.RemoveAllListeners();
            if (canSellWeapon)
                sellWeaponButton.onClick.AddListener(OnSellWeaponClicked);
        }

        Armor currentArmor = player.Armor;
        bool isDefaultArmor = ( currentArmor != null &&
                               gm.DefaultArmor != null &&
                               currentArmor == gm.DefaultArmor );

        if (armorImage != null)
        {
            if (currentArmor != null && currentArmor.Icon != null)
            {
                armorImage.sprite = currentArmor.Icon;
                armorImage.color = Color.white;
            }
            else
            {
                armorImage.sprite = null;
                armorImage.color = emptyColor;
            }
        }

        if (sellArmorButton != null)
        {
            bool canSellArmor = currentArmor != null && !isDefaultArmor;
            sellArmorButton.interactable = canSellArmor;
            TintButton(sellArmorButton, canSellArmor);
            sellArmorButton.onClick.RemoveAllListeners();
            if (canSellArmor)
                sellArmorButton.onClick.AddListener(OnSellArmorClicked);
        }

        _hpPotionExample = FindPotionOfType(player, PotionType.HP);
        _staminaPotionExample = FindPotionOfType(player, PotionType.Endurance);

        int hpCount = player.HPPotions;
        int staminaCount = player.StaminaPotions;

        if (hpPotionCountText != null)
            hpPotionCountText.text = hpCount.ToString();

        if (hpPotionImage != null)
            hpPotionImage.color = hpCount > 0 ? hpNormalColor : emptyColor;

        if (sellHpPotionButton != null)
        {
            bool canSellHp = hpCount > 0;
            sellHpPotionButton.interactable = canSellHp;
            TintButton(sellHpPotionButton, canSellHp);
            sellHpPotionButton.onClick.RemoveAllListeners();
            if (canSellHp)
                sellHpPotionButton.onClick.AddListener(OnSellHpPotionClicked);
        }

        if (staminaPotionCountText != null)
            staminaPotionCountText.text = staminaCount.ToString();

        if (staminaPotionImage != null)
            staminaPotionImage.color = staminaCount > 0 ? staminaNormalColor : emptyColor;

        if (sellStaminaPotionButton != null)
        {
            bool canSellStamina = staminaCount > 0;
            sellStaminaPotionButton.interactable = canSellStamina;
            TintButton(sellStaminaPotionButton, canSellStamina);
            sellStaminaPotionButton.onClick.RemoveAllListeners();
            if (canSellStamina)
                sellStaminaPotionButton.onClick.AddListener(OnSellStaminaPotionClicked);
        }
    }

    private void OnSellWeaponClicked()
    {
        var gm = GameManager.Instance;
        var player = gm.PlayerController;
        Weapon weapon = player.Weapon;

        if (weapon == null)
            return;

        int sellPrice = GetSellPrice(weapon.Value);

        string msg =
            $"The merchants here are a bit cheeky – they only pay 75% of an item's true value.\n\n" +
            $"Sell weapon \"{weapon.Name}\" for {sellPrice} gold?\n" +
            "After selling, your current weapon will be replaced with your best previously owned weapon.";

        ShowSellAnnouncement(msg, () =>
        {
            player.Gold += sellPrice;
            player.Money = player.Gold;

            RemoveItemFromInventory(player.Items, weapon);

            var bestWeapon = player.Items
                .OfType<Weapon>()
                .OrderByDescending(w => w.DamagePower)
                .FirstOrDefault();

            player.Weapon = bestWeapon ?? gm.DefaultWeapon;
            RefreshUI();
        });
    }

    private void OnSellArmorClicked()
    {
        var gm = GameManager.Instance;
        var player = gm.PlayerController;
        Armor armor = player.Armor;

        if (armor == null)
            return;

        int sellPrice = GetSellPrice(armor.Value);

        string msg =
            $"The merchants here are a bit cheeky – they only pay 75% of an item's true value.\n\n" +
            $"Sell armor \"{armor.Name}\" for {sellPrice} gold?\n" +
            "After selling, your current armor will be replaced with your best previously owned armor.";

        ShowSellAnnouncement(msg, () =>
        {
            player.Gold += sellPrice;
            player.Money = player.Gold;

            RemoveItemFromInventory(player.Items, armor);

            var bestArmor = player.Items
                .OfType<Armor>()
                .OrderByDescending(w => w.ProtectionPoints)
                .FirstOrDefault();

            player.Armor = bestArmor ?? gm.DefaultArmor;
            RefreshUI();
        });
    }

    private void OnSellHpPotionClicked()
    {
        var player = GameManager.Instance.PlayerController;
        if (player.HPPotions <= 0)
            return;

        Potion potionToSell = FindPotionOfType(player, PotionType.HP);
        if (potionToSell == null)
            return;

        int sellPrice = GetSellPrice(potionToSell.Value);

        string msg =
            $"The merchants here are a bit cheeky – they only pay 75% of an item's true value.\n\n" +
            $"Sell one HP potion for {sellPrice} gold?";

        ShowSellAnnouncement(msg, () =>
        {
            player.Gold += sellPrice;
            player.Money = player.Gold;

            player.HPPotions = Mathf.Max(0, player.HPPotions - 1);
            RemoveItemFromInventory(player.Items, potionToSell);

            RefreshUI();
        });
    }

    private void OnSellStaminaPotionClicked()
    {
        var player = GameManager.Instance.PlayerController;
        if (player.StaminaPotions <= 0)
            return;

        Potion potionToSell = FindPotionOfType(player, PotionType.Endurance);
        if (potionToSell == null)
            return;

        int sellPrice = GetSellPrice(potionToSell.Value);

        string msg =
            $"The merchants here are a bit cheeky – they only pay 75% of an item's true value.\n\n" +
            $"Sell one stamina potion for {sellPrice} gold?";

        ShowSellAnnouncement(msg, () =>
        {
            player.Gold += sellPrice;
            player.Money = player.Gold;

            player.StaminaPotions = Mathf.Max(0, player.StaminaPotions - 1);
            RemoveItemFromInventory(player.Items, potionToSell);

            RefreshUI();
        });
    }

    private void ShowSellAnnouncement(string message, Action onConfirmed)
    {
        var popup = Instantiate(announcementPrefab, uiRoot);
        var ctrl = popup.GetComponent<AnnouncementController>();

        if (ctrl == null)
        {
            Destroy(popup);
            return;
        }

        ctrl.Init(message, 0, true, accepted =>
        {
            if (accepted)
                onConfirmed?.Invoke();
        });
    }

    private int GetSellPrice(int baseValue)
    {
        return Mathf.RoundToInt(baseValue * 0.75f);
    }

    private void TintButton(Button btn, bool enabled)
    {
        if (btn == null) return;
        var img = btn.targetGraphic as Image;
        if (img != null)
            img.color = enabled ? Color.white : emptyColor;
    }

    private void RemoveItemFromInventory(List<IItem> items, IItem item)
    {
        if (items == null || item == null)
            return;

        int idx = items.FindIndex(i => ReferenceEquals(i, item));
        if (idx < 0)
            idx = items.FindIndex(i => i.Name == item.Name);

        if (idx >= 0)
            items.RemoveAt(idx);
    }

    private Potion FindPotionOfType(PlayerController player, PotionType type)
    {
        if (player.Items == null)
            return null;

        return player.Items.OfType<Potion>().FirstOrDefault(p => p.Type == type);
    }

    public void ShowWeaponTooltip()
    {
        var weapon = GameManager.Instance.PlayerController.Weapon;
        if (weapon == null)
        {
            HideTooltip();
            return;
        }
        string body = 
                $"Damage: {weapon.DamagePower}\n" +
                $"Range: {weapon.Range}\n\n\n" +
                "Default weapon\nThis item cannot be sold.";

        if (weapon != GameManager.Instance.DefaultWeapon)
        {
            body =
                $"Damage: {weapon.DamagePower}\n" +
                $"Range: {weapon.Range}\n" +
                $"Type: {( weapon.Ranged ? "Ranged" : "Melee" )}\n" +
                $"Value: {weapon.Value} gold";
        }
        ShowTooltip(weapon.Name, body);
    }

    public void ShowArmorTooltip()
    {
        var armor = GameManager.Instance.PlayerController.Armor;
        if (armor == null)
        {
            HideTooltip();
            return;
        }

        string body =
            $"Protection: {armor.ProtectionPoints}\n\n\n" +
            "Default armor\nThis item cannot be sold.";
        if (armor != GameManager.Instance.DefaultArmor)
        {
            body =
            $"Protection: {armor.ProtectionPoints}\n" +
            $"Value: {armor.Value} gold";
        }

        ShowTooltip(armor.Name, body);
    }

    public void ShowHpPotionTooltip()
    {
        var player = GameManager.Instance.PlayerController;
        if (player.HPPotions <= 0 || _hpPotionExample == null)
        {
            ShowTooltip("HP Potion", "You have no HP potions.");
            return;
        }

        string body =
            $"Restores HP by +{_hpPotionExample.IncreaseValue}\n" +
            $"Owned: {player.HPPotions}\n" +
            $"Value: {_hpPotionExample.Value} gold each";

        ShowTooltip(_hpPotionExample.Name, body);
    }

    public void ShowStaminaPotionTooltip()
    {
        var player = GameManager.Instance.PlayerController;
        if (player.StaminaPotions <= 0 || _staminaPotionExample == null)
        {
            ShowTooltip("Stamina Potion", "You have no stamina potions.");
            return;
        }

        string body =
            $"Restores Endurance by +{_staminaPotionExample.IncreaseValue}\n" +
            $"Owned: {player.StaminaPotions}\n" +
            $"Value: {_staminaPotionExample.Value} gold each";

        ShowTooltip(_staminaPotionExample.Name, body);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    private void ShowTooltip(string title, string body)
    {
        if (tooltipPanel == null || tooltipTitleText == null || tooltipBodyText == null)
            return;

        tooltipTitleText.text = title;
        tooltipBodyText.text = body;
        tooltipPanel.SetActive(true);
    }
}
