using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Assets.Scripts
{
    public class CombatController
    {
        public Entity Enemy;
        public PlayerController Player;

        public void AttackClick(Entity entity)
        {
            var posA = Enemy.transform.position;
            var posB = Player.transform.position;

            float distSq = math.distancesq(posA, posB);
            //float rangeSq = range * range;

            //return distSq <= rangeSq;
        }

        public void Defend() { }

        public void StepRight () 
        {
            
        }

        public void StepLeft ()
        {

        }

        public void JumpLeft()
        {
        }

        public void JumpRight()
        {
        }
    }
}
