using Assets.Scripts.UIControllers;
using System;
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

            int puzzleIndex = UnityEngine.Random.Range(0, bucket.Count);
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

        private Tuple<Tuple<List<RoomType>, string>, Tuple<List<RoomType>, string>> CreatePathHints()
        {
            float rng = UnityEngine.Random.Range(0, 1f);
            var fixedPoints1 = CreateFixedPathPoints();
            var fixedPoints2 = CreateFixedPathPoints();

            throw new NotImplementedException();
        }
        private List<RoomType> CreateFixedPathPoints()
        {
            //We split the fixed path chances.
            float rng = UnityEngine.Random.Range(0, 5);
            if (rng < 1)
            {
                return new List<RoomType>();
            }
            if (rng < 3)
            {
                var l=new List<RoomType>();
                l.Add((RoomType)UnityEngine.Random.Range(0, RoomGenerator.roomTypes));
                return l;
            }
            else
            {
                var l = new List<RoomType>();
                int roll1 = UnityEngine.Random.Range(0, RoomGenerator.roomTypes);
                int roll2 = UnityEngine.Random.Range(0, RoomGenerator.roomTypes - 1);
                if (roll2 >= roll1)
                {
                    roll2++;//Ensuring roll1 and 2 differ.
                }
                l.Add((RoomType)roll1);                
                return l;
            }
        }
    }
}
