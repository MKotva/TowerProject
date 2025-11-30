using Assets.Core;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyController : Entity
    {
        public int HPPotions = 0;
        public int StaminaPotions = 0;

        new void Start()
        {
            base.Start();
        }

        public bool IsDead => Lives <= 0;
    }
}