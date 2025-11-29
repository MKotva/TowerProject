using NUnit.Framework;
using System;
using UnityEngine;

public class IWeapon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PlayerStats BonusStats { get; set; }
    public int Range { get; set; }
    //public List<Vector2Int> AffectedTiles { get; set; } //I am not sure why TF this doesn't work

}
