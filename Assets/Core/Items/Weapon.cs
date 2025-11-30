using Assets.Core;
using UnityEngine;


namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Game/Weapon Data")]
    public class Weapon : ScriptableObject, IItem
    {
        [SerializeField] private string name;
        [SerializeField] private int value;
        [SerializeField] private Sprite icon;
        [SerializeField] private bool ranged;
        [SerializeField] private int damagePower;
        [SerializeField] private int range;

        public string Name { get { return name; } set { name = value; } }
        public int Value { get { return value; } set { this.value = value; } }
        public Sprite Icon { get { return icon; } set { icon = value; } }
        public bool Ranged { get { return ranged; } set { ranged = value; } }
        public int DamagePower { get { return damagePower; } set { damagePower = value; } }
        public int Range { get { return range; } set { range = value; } }

        public void Use(Entity entity)
        {
        }
    }
}
