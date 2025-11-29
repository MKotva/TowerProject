using Assets.Scripts.UIControllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        public int MaxLevel = 20;
        public int ReachedLevel = 1;
        public PlayerController PlayerController;
        public PuzzleMenuController PuzzleMenuController;
        public string PuzzlePath;

        public List<List<Puzzle>> Puzzles;

        private bool isSolvingPuzzle = false;

        public List<RoomType> upcomingRooms;

        private void Start()
        {
            Puzzles = PuzzleLoader.LoadPuzzlesGroupedByDifficulty(PuzzlePath);
            if(PuzzleMenuController != null ) 
                PuzzleMenuController.OnAnswerClicked += OnPuzzleAnswer;
            PuzzleTime();
        }

        public void PuzzleTime()
        {
            PuzzleMenuController.gameObject.SetActive(true);
            if (isSolvingPuzzle)
                return;

            if (Puzzles == null || Puzzles.Count == 0)
            {
                Debug.LogWarning("No puzzles loaded!");
                return;
            }

            int difficultyIndex = GetDifficultyIndexForLevel(ReachedLevel);

            List<Puzzle> bucket = Puzzles[difficultyIndex];
            if (bucket == null || bucket.Count == 0)
            {
                Debug.LogWarning($"No puzzles in difficulty bucket {difficultyIndex}");
                return;
            }

            int puzzleIndex = Random.Range(0, bucket.Count);
            Puzzle selectedPuzzle = bucket[puzzleIndex];

            PuzzleMenuController.ShowPuzzle(selectedPuzzle, 60, OnPuzzleTimeExpiration);

            isSolvingPuzzle = true;
        }

        private int GetDifficultyIndexForLevel(int level)
        {
            level = Mathf.Clamp(level, 1, MaxLevel);
            int index = ( level - 1 ) * Puzzles.Count / MaxLevel;
            return Mathf.Clamp(index, 0, Puzzles.Count - 1);
        }

        public void OnPuzzleTimeExpiration()
        {
            PlayerController.HP -= 1;
            isSolvingPuzzle = false;
            PuzzleTime();
        }

        private void OnPuzzleAnswer(bool truth, string answer, Puzzle puzzle)
        {
            if (truth)
            {
                isSolvingPuzzle = false;
                PuzzleMenuController.gameObject.SetActive(false);

            }
            else
            {
                 OnPuzzleTimeExpiration();

            }
        }
    }
}
