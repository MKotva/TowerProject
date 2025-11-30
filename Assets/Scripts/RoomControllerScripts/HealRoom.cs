using UnityEngine;
using System.Collections;

namespace Assets.Scripts.RoomControllerScripts
{
    public class HealRoom : RoomBase
    {
        public void OnHealClick()
        {
            GameManager.Instance.PlayerController.HealPlayer();
            FinishRoom();
        }
    }
}