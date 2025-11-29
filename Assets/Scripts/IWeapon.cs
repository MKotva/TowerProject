using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;

public interface IWeapon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PlayerStats BonusStats { get; set; }
    public int Range { get; set; }
    public int MinDMG {  get; set; }
    public float MaxDMG { get; set; }

    public int BlockDamageReduction { get; set; }
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt); //Tiles affected around the targeted tile. Would be replaced by floating point range in case of 2D combat
}

public class Sword : IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int MinDMG { get; set; }
    public float MaxDMG { get; set; }
    public int BlockDamageReduction { get; set; } = 5;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        var l = new List<Vector2Int>() { new Vector2Int(0, 0) };
        var diffVec = Tgt - Pos;

        if (diffVec.x * diffVec.x >= diffVec.y * diffVec.y)
        {
            l.Add(new Vector2Int(1, 0));
            l.Add(new Vector2Int(-1, 0));
        }
        else
        {
            l.Add(new Vector2Int(0, 1));
            l.Add(new Vector2Int(0, -1));
        }
        return l;
    }
}

public class Spear: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int MinDMG { get; set; }
    public float MaxDMG { get; set; }
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        var l=new List<Vector2Int>() { new Vector2Int(0, 0) };
        var diffVec = Tgt - Pos;
        if (diffVec.x * diffVec.x >= diffVec.y * diffVec.y)
        {
            if (diffVec.x > 0)
            {
                l.Add(new Vector2Int(1, 0));
            }
            else
            {
                l.Add(new Vector2Int(-1, 0));
            }
        }
        else
        {
            if (diffVec.y > 0) 
            {
                l.Add(new Vector2Int(0, 1));
            }
            else
            {
                l.Add(new Vector2Int(0, -1));
            }
        }
        return l;
    }
}
public class Bow: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 8;
    public int MinDMG { get; set; }
    public float MaxDMG { get; set; }
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        return new List<Vector2Int>() { new Vector2Int(0, 0) };
    }
}
public class Shield: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int MinDMG { get; set; }
    public float MaxDMG { get; set; }
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        return new List<Vector2Int>() { new Vector2Int(0, 0) };
    }
}
public class MagicStaff: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int MinDMG { get; set; }
    public float MaxDMG { get; set; }
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        return new List<Vector2Int>() { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
    }
}
