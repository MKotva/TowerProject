using Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Entity : MonoBehaviour
    {
        public double MaxHP = 5;
        public double HP = 5;

        public double MaxMana = 100;
        public double Mana = 100;

        public double MaxEndurance = 100;
        public double Endurance = 100;

        public double Money = 100;
        public SkillSet SkillSet;
        public Weapon Weapon;
        public Armor[] Armor = new Scripts.Armor[5];


        public void RecieveDamage(double atackPower)
        {
            var defendPower = 0;
            foreach(var armor in Armor)
            {
                if(armor != null)
                {
                    defendPower += armor.ProtectionPoints;
                }
            }

            atackPower -= (defendPower + (0.6 * (Math.Pow(SkillSet.Agility, 2))));
            if (atackPower < 0)
                return;
            else
                HP -= atackPower;
        }

        public double DealDamage()
        {
            return (Weapon.DamagePower + ( 0.6 * ( Math.Pow(SkillSet.Strength, 2))));
        }
    }
}
