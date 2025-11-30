using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.RoomControllerScripts
{
    public class TreasureRoom : RoomBase
    {
        [SerializeField] private ScreenBlanketController blanketController;
        [SerializeField] private Button button;

        public void OnTreasureClick()
        {
            button.interactable =false;

            blanketController.FadeToBlack();
            GameManager.Instance.PlayerController.Gold += (int) ( ( GameManager.Instance.ReachedLevel * 0.4 ) * 200 );
            FinishRoom();
        }
    }
}