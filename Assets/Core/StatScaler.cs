using UnityEngine;
using Assets.Core.Items;

public static class StatScaler
{
    public static int ScaleWeaponDamage(int baseDamagePower, int level)
    {
        float levelFactor = 1f + ( level + 1 ) * 0.3f;
        return Mathf.Max(1, Mathf.RoundToInt(baseDamagePower * levelFactor));
    }

    public static int ScaleWeaponValue(int baseValue, int level)
    {
        float valueFactor = 1f + ( level + 1 ) * 0.4f;
        return Mathf.Max(1, Mathf.RoundToInt(baseValue * valueFactor));
    }

    public static int ScaleArmorProtection(int baseProtectionPoints, int level)
    {
        float levelFactor = 1f + ( level + 1 ) * 0.25f;
        return Mathf.Max(1, Mathf.RoundToInt(baseProtectionPoints * levelFactor));
    }

    public static int ScaleArmorValue(int baseValue, int level)
    {
        float valueFactor = 1f + ( level + 1 ) * 0.35f;
        return Mathf.Max(1, Mathf.RoundToInt(baseValue * valueFactor));
    }

    public static int CalculatePotionIncrease(PotionType type, float restoreFractionOfMax, int flatBonus, PlayerController player)
    {
        double baseMax = type switch
        {
            PotionType.HP => player.MaxLives * player.LiveHP,
            PotionType.Endurance => player.MaxEndurance,
            _ => 0
        };

        double raw = baseMax * restoreFractionOfMax + flatBonus;
        int inc = Mathf.Max(1, Mathf.RoundToInt((float) raw));
        return inc;
    }

    public static int ScalePotionPrice(int baseValue, int increaseAmount, float pricePerPoint = 0.5f)
    {
        int price = Mathf.Max(1, Mathf.RoundToInt(baseValue + increaseAmount * pricePerPoint));
        return price;
    }

    public static int ScaleStat(int baseValue, int level, float perLevelFactor, int min = 1, int levelOffset = 1)
    {
        float factor = 1f + ( level + levelOffset ) * perLevelFactor;
        return Mathf.Max(min, Mathf.RoundToInt(baseValue * factor));
    }

    public static int ScaleStatAdditive(int baseValue, int level, float perLevel, int bonus = 0, int min = 1)
    {
        float raw = baseValue + level * perLevel + bonus;
        return Mathf.Max(min, Mathf.RoundToInt(raw));
    }
}