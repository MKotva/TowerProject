using Assets.Scripts.UIControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RoomControllerScripts
{
    public class PuzzleRoom : RoomBase
    {
        [SerializeField] private ScreenBlanketController blanketController;
        [SerializeField] private Button button;
        public void OnPuzzleClick()
        {
            if (GameManager.Instance.PuzzleMenuController != null)
                GameManager.Instance.PuzzleMenuController.OnAnswerClicked += OnPuzzleAnswer;
            GameManager.Instance.PuzzleTime();
        }

        private void OnPuzzleAnswer(bool truth, string answer, Puzzle puzzle)
        {
            if (truth)
            {
                button.interactable = false;
                blanketController.FadeToBlack();
                FinishRoom();
            }

            GameManager.Instance.OnPuzzleAnswer(truth, answer, puzzle);
        }
    }
}