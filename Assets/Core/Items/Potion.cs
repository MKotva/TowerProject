using Assets.Scripts;
using UnityEngine;

namespace Assets.Core.Items
{
    public enum PotionType { HP, MANA, Endurance}

    [CreateAssetMenu(menuName = "Game/Potion Data")]
    public class Potion : ScriptableObject, IItem
    {
        [SerializeField] private string name;
        [SerializeField] private int value;
        [SerializeField] private PotionType type;
        [SerializeField] private int increaseValue;
        [SerializeField] private Sprite icon;

        public float RestoreFractionOfMax = 0.25f;

        public int FlatBonus = 0;

        public string Name { get { return name; } set { name = value; } }
        public int Value { get { return value; } set { this.value = value; } }
        public PotionType Type { get { return type; } set { type = value; } }
        public int IncreaseValue { get { return increaseValue; } set { increaseValue = value; } }
        public Sprite Icon { get { return icon; } set { icon = value; } }

        public void Use(Entity entity)
        {
            switch (Type)
            {
                case PotionType.HP: 
                    var healValue = IncreaseValue;
                    while(entity.HP + healValue <= entity.LiveHP)
                    {
                        healValue -= (int)(entity.LiveHP - entity.HP);
                        entity.Lives++;
                    }

                    entity.HP += healValue;

                    break;
                case PotionType.MANA:
                    if (entity.Mana + IncreaseValue <= entity.MaxMana)
                        entity.Mana += IncreaseValue;
                    else
                        entity.Mana = entity.MaxMana;
                    break;
                case PotionType.Endurance:
                    if (entity.Endurance + IncreaseValue <= entity.MaxEndurance)
                        entity.Endurance += IncreaseValue;
                    else
                        entity.Endurance = entity.MaxEndurance;
                    break;
            }
        }
    }
}
