using Assets.Scripts;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Items
{
    public enum PotionType { HP, MANA, Endurance}

    internal class Potion : IItem
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public PotionType Type { get; set; }

        public int IncreaseValue { get; set; }

        public void Use(Entity entity)
        {
            switch (Type)
            {
                case PotionType.HP: 
                    if(entity.HP + IncreaseValue <= entity.MaxHP)
                        entity.HP += IncreaseValue;
                    else
                        entity.HP = entity.MaxHP;
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
