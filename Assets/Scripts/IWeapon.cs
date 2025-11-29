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
    public int BlockDamageReduction { get; set; }
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt); //Tiles affected around the targeted tile. Would be replaced by floating point range in case of 2D combat
}

public class Sword : IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; }=new PlayerStats();
    public int Range { get; set; } = 1;
    public int BlockDamageReduction { get; set; } = 5;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        throw new NotImplementedException();
    }
}

public class Spear: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        throw new NotImplementedException();
    }
}
public class Bow: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 8;
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        throw new NotImplementedException();
    }
}
public class Shield: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        throw new NotImplementedException();
    }
}
public class MagicStaff: IWeapon
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PlayerStats BonusStats { get; set; } = new PlayerStats();
    public int Range { get; set; } = 1;
    public int BlockDamageReduction { get; set; } = 1;
    public List<Vector2Int> GetAffectedTiles(Vector2Int Pos, Vector2Int Tgt)
    {
        throw new NotImplementedException();
    }
}
