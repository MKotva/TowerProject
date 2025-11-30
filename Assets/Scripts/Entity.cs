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
        public double MaxLives = 5;
        public double Lives = 5;
        public double LiveHP = 20;
        public double HP = 20;

        public double MaxMana = 100;
        public double Mana = 100;

        public double MaxEndurance = 100;
        public double Endurance = 100;

        public double Money = 100;
        public SkillSet SkillSet;
        public Weapon Weapon;
        public Armor Armor;

        public void Start()
        {
            SkillSet = new SkillSet();
            Weapon = GameManager.Instance.DefaultWeapon;
            Armor = GameManager.Instance.DefaultArmor;
        }


        public void RecieveDamage(double atackPower)
        {
            var defendPower = 0;
            if (Armor != null)
            {
                defendPower += Armor.ProtectionPoints;
            }


            atackPower -= ( defendPower + ( 0.6 * ( Math.Pow(SkillSet.Agility, 2) ) ) );
            if (atackPower < 0)
                return;
            else
            {
                while(HP - atackPower < 0)
                {
                    atackPower -= HP;
                    LooseLive();
                }
                HP -= atackPower;
            }
        }

        public double DealDamage()
        {
            return ( Weapon.DamagePower + ( 0.6 * ( Math.Pow(SkillSet.Strength, 2) ) ) );
        }

        public void LooseLive()
        {
            Lives--;
            HP = 20;
        }
    }
}
