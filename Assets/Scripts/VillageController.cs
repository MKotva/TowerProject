using Assets.Core;
using Assets.Core.Items;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VillageController : MonoBehaviour
{
    [Header("Where to spawn the popup")]
    public Transform uiRoot;

    [Header("Popup prefab (single reusable window)")]
    public ShopPopupControlelr popupPrefab;

    [Header("Row prefabs (inside the popup)")]
    public ShopItemPanelUI shopItemRowPrefab;
    public LibraryStatPanelUI libraryRowPrefab;

    [Header("Tower")]
    public GameObject towerAnnouncementPrefab;

    private ShopPopupControlelr _activePopup;

    // Track rows for affordability refresh
    private readonly List<ShopItemPanelUI> _currentShopRows = new();
    private readonly List<LibraryStatPanelUI> _currentLibraryRows = new();

    private class BlacksmithStockEntry
    {
        public IItem Item;
        public Sprite Icon;
        public string Description;
    }

    private readonly List<BlacksmithStockEntry> _blacksmithStock = new();
    private void OnEnable()
    {
        GenerateBlacksmithStock();
    }

    public void OnBlacksmithButton()
    {
        OpenPopup("Blacksmith");
        ClearContainer(_activePopup.itemsContainer);
        _currentShopRows.Clear();
        PopulateBlacksmith(_activePopup.itemsContainer);
    }

    public void OnPotionerButton()
    {
        OpenPopup("Potions");
        ClearContainer(_activePopup.itemsContainer);
        _currentShopRows.Clear();
        PopulatePotioner(_activePopup.itemsContainer);
    }

    public void OnLibraryButton()
    {
        OpenPopup("Library");
        ClearContainer(_activePopup.itemsContainer);
        _currentLibraryRows.Clear();
        PopulateLibrary(_activePopup.itemsContainer);
    }

    public void OnTravelToTowerButton()
    {
        string msg =
            "Beyond the safety of the city walls rises the Old King’s Tower, " +
            "a place of forgotten oaths and restless steel.\n\n" +
            "Many heroes have entered its shadow, few have walked back out on their own feet. " +
            "Should you wish to retreat once you step inside, you will have to summon the royal rescue squad " +
            $"and they never ride for free. Each rescue from the tower will cost you {(int)(GameManager.Instance.towerRescueFee * (GameManager.Instance.ReachedLevel * 0.5))} gold.\n\n" +
            "Do you still wish to travel to the tower?";

        var popupGO = Instantiate(towerAnnouncementPrefab, uiRoot);
        var announcement = popupGO.GetComponent<AnnouncementController>();

        if (announcement == null)
        {
            Destroy(popupGO);
            return;
        }

        announcement.Init(msg, 0, true, accepted =>
        {
            if (!accepted)
            {
                return;
            }
            GoToTower();
        });
    }

    private void GoToTower()
    {
        GameManager.Instance.ScreenBlanker.RunFadeSequence(() =>
        {
            GameData.manager = GameManager.Instance;
            SceneManager.LoadScene("TowerScene");
        });
    }

    private void OpenPopup(string headerText)
    {
        if (_activePopup == null)
        {
            _activePopup = Instantiate(popupPrefab, uiRoot);
            _activePopup.Init(ClosePopup);
        }

        _activePopup.SetHeader(headerText);
    }

    public void ClosePopup()
    {
        if (_activePopup != null)
        {
            Destroy(_activePopup.gameObject);
            _activePopup = null;
            _currentShopRows.Clear();
            _currentLibraryRows.Clear();
        }
    }

    private void GenerateBlacksmithStock()
    {
        _blacksmithStock.Clear();

        var gm = GameManager.Instance;
        int playerLevel = gm.ReachedLevel;

        List<Weapon> allWeapons = gm.Weapons ?? new List<Weapon>();
        List<Armor> allArmor = gm.Armor ?? new List<Armor>();

        int maxWeapons = UnityEngine.Random.Range(1, allWeapons.Count);
        var selectedWeapons = allWeapons
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(maxWeapons);

        foreach (var data in selectedWeapons)
        {
            int scaledDamage = StatScaler.ScaleWeaponDamage(data.DamagePower, playerLevel);
            int scaledValue = StatScaler.ScaleWeaponValue(data.Value, playerLevel);

            var weapon = new Weapon
            {
                Name = data.Name,
                Value = scaledValue,
                DamagePower = scaledDamage,
                Ranged = data.Ranged,
                Icon = data.Icon
            };

            string desc = $"Lvl {playerLevel + 1}+ weapon\n" +
                          $"Damage: {weapon.DamagePower}\n" +
                          $"Range: {( weapon.Ranged ? "Ranged" : "Melee" )}";

            _blacksmithStock.Add(new BlacksmithStockEntry
            {
                Item = weapon,
                Icon = data.Icon,
                Description = desc
            });
        }

        int maxArmor = UnityEngine.Random.Range(1, allArmor.Count);
        var selectedArmor = allArmor
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(maxArmor);

        foreach (var data in selectedArmor)
        {
            int scaledProtection = StatScaler.ScaleArmorProtection(data.ProtectionPoints, playerLevel);
            int scaledValue = StatScaler.ScaleArmorValue(data.Value, playerLevel);

            var armor = new Armor
            {
                Name = data.Name,
                Value = scaledValue,
                ProtectionPoints = scaledProtection,
                Icon = data.Icon
            };

            string desc = $"Lvl {playerLevel + 1}+ armor\n" +
                          $"Protection: {armor.ProtectionPoints}";

            _blacksmithStock.Add(new BlacksmithStockEntry
            {
                Item = armor,
                Icon = data.Icon,
                Description = desc
            });
        }
    }

    private void PopulateBlacksmith(Transform container)
    {
        var player = GameManager.Instance.PlayerController;

        foreach (var entry in _blacksmithStock)
        {
            var row = Instantiate(shopItemRowPrefab, container);
            bool canAfford = player.Gold >= entry.Item.Value;

            row.Bind(entry.Item, entry.Description, entry.Icon, OnBlacksmithBuy, canAfford);
            _currentShopRows.Add(row);
        }
    }

    private void OnBlacksmithBuy(ShopItemPanelUI panel, IItem item)
    {
        var player = GameManager.Instance.PlayerController;
        player.Gold -= item.Value;
        player.Items.Add(item);
        if(item is Weapon)
            player.Weapon = item as Weapon;
        else
            player.Armor = item as Armor;

            _currentShopRows.Remove(panel);
        Destroy(panel.gameObject);

        for (int i = _blacksmithStock.Count - 1; i >= 0; i--)
        {
            if (ReferenceEquals(_blacksmithStock[i].Item, item))
            {
                _blacksmithStock.RemoveAt(i);
                break;
            }
        }

        RefreshShopAffordability(player.Gold);
    }

    private void PopulatePotioner(Transform container)
    {
        var gm = GameManager.Instance;
        var player = gm.PlayerController;

        List<Potion> allPotions = gm.Potions ?? new List<Potion>();

        foreach (var data in allPotions)
        {
            if (data == null)
                continue;

            int increase = StatScaler.CalculatePotionIncrease(
                data.Type,
                data.RestoreFractionOfMax,
                data.FlatBonus,
                player
            );

            int price = StatScaler.ScalePotionPrice(data.Value, increase);

            var potion = new Potion
            {
                Name = data.Name,
                Value = price,
                Type = data.Type,
                IncreaseValue = increase
            };

            string statName = data.Type switch
            {
                PotionType.HP => "HP",
                PotionType.Endurance => "Endurance",
                _ => "Stat"
            };

            string desc = $"{statName} +{increase}";

            var row = Instantiate(shopItemRowPrefab, container);
            bool canAfford = player.Gold >= potion.Value;

            row.Bind(potion, desc, data.Icon, OnPotionBuy, canAfford);
            _currentShopRows.Add(row);
        }
    }

    private void OnPotionBuy(ShopItemPanelUI panel, IItem item)
    {
        var player = GameManager.Instance.PlayerController;
        player.Gold -= item.Value;
        player.Items.Add(item);

        if(item is Potion)
        {
            var potion = (Potion)item;
            if (potion.Type == PotionType.HP)
                player.HPPotions++;
            else if(potion.Type == PotionType.Endurance)
                player.StaminaPotions++;
        }

        RefreshShopAffordability(player.Gold);
    }

    private void PopulateLibrary(Transform container)
    {
        SkillSet skills = GameManager.Instance.PlayerController.SkillSet;

        CreateStatEntry(container, "Strength", () => skills.Strength, v => skills.Strength = v);
        CreateStatEntry(container, "Agility", () => skills.Agility, v => skills.Agility = v);
        CreateStatEntry(container, "Endurance", () => skills.Endurance, v => skills.Endurance = v);
        CreateStatEntry(container, "Inteligence", () => skills.Inteligence, v => skills.Inteligence = v);
        CreateStatEntry(container, "Luck", () => skills.Luck, v => skills.Luck = v);
    }

    private void CreateStatEntry(Transform container,
                                 string statName,
                                 Func<int> getter,
                                 Action<int> setter)
    {
        var player = GameManager.Instance.PlayerController;

        int current = getter();
        int cost = CalculateUpgradeCost(current);

        var row = Instantiate(libraryRowPrefab, container);
        bool canAfford = player.Gold >= cost;

        row.Bind(statName, current, cost, canAfford, () =>
        {
            player.Gold -= cost;
            setter(current + 1);

            Debug.Log($"Upgraded {statName} to {current + 1} for {cost} gold.");

            ClearContainer(container);
            _currentLibraryRows.Clear();
            PopulateLibrary(container);
        });

        _currentLibraryRows.Add(row);
    }

    private int CalculateUpgradeCost(int currentValue)
    {
        return 10 + currentValue * currentValue * 2;
    }

    private void RefreshShopAffordability(int currentGold)
    {
        for (int i = _currentShopRows.Count - 1; i >= 0; i--)
        {
            var row = _currentShopRows[i];
            if (row == null)
            {
                _currentShopRows.RemoveAt(i);
                continue;
            }
            row.UpdateAffordability(currentGold);
        }
    }

    private void ClearContainer(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
    }
}