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
    public List<Vector2Int> AffectedTiles { get; set; } //Tiles affected around the targeted tile. Would be replaced by floating point range in case of 2D combat
}

public class Sword : IWeapon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PlayerStats BonusStats { get; set; }
    public int Range { get; set; }
    public List<Vector2Int> AffectedTiles { get; set; }
}
