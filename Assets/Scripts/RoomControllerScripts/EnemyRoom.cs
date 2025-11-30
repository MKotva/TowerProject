using UnityEngine;
using System.Collections;

namespace Assets.Scripts.RoomControllerScripts
{
    public class EnemyRoom : RoomBase
    {
        void Update()
        {
            if(base.controller != null)
                FinishRoom();
        }
    }
}