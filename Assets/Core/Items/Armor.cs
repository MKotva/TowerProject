using Assets.Core;
using UnityEngine;


namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Game/Armor Data")]
    public class Armor : ScriptableObject, IItem
    {
        [SerializeField] private string name;
        [SerializeField] private int protectionPoints;
        [SerializeField] private int value;
        [SerializeField] private Sprite icon;

        public string Name { get { return name; } set { name = value; } }
        public int ProtectionPoints { get { return protectionPoints; } set { protectionPoints = value; } }
        public int Value { get { return value; } set { this.value = value; } }
        public Sprite Icon { get { return icon; } set { icon = value; } }

        public void Use(Entity entity)
        {
        }
    }
}
