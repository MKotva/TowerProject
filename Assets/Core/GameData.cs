using Assets.Core;
using Assets.Scripts;
using System.Collections.Generic;

public static class GameData
{
    public static bool  Backed = false;
    public static int ReachedLevel = 0;
    public static int Gold = 0;
    public static List<IItem> Items = null;
    public static double HP = 0;
    public static double Lives = 0;
    public static SkillSet SkillSet = null;
    public static Weapon Weapon = null;
    public static Armor Armor = null;

    public static void Backup()
    {
        Backed = true;
        ReachedLevel = GameManager.Instance.ReachedLevel;
        var other = GameManager.Instance.PlayerController;
        Gold = other.Gold;
        Items = other.Items;
        HP = other.HP;
        Lives = other.Lives;
        SkillSet = other.SkillSet;
        Weapon = other.Weapon;
        Armor = other.Armor;
    }

    public static void Clear()
    {
        Backed = false;
        ReachedLevel = 0;
        Gold = 0;
        Items = null;
        HP = 0;
        Lives = 0;
        SkillSet = null;
        Weapon = null;
        Armor = null;
    }
}