using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.UIControllers;
using Unity.VisualScripting;
using static CountdownTimer;

public class PuzzleMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text riddleText;          
    [SerializeField] private Transform answersParent;      
    [SerializeField] private Button answerButtonPrefab;
    [SerializeField] private CountdownTimer timer;

    public Action<bool, string, Puzzle> OnAnswerClicked;
    private Puzzle currentPuzzle;

    public void ShowPuzzle(Puzzle puzzle, int time, OnTimerEnd callBack)
    {
        if (puzzle == null)
        {
            Debug.LogWarning("ShowPuzzle called with null puzzle.");
            return;
        }

        currentPuzzle = puzzle;

        if (riddleText != null)
            riddleText.text = puzzle.Riddle;
        else
            Debug.LogWarning("PuzzleUIController: riddleText is not assigned!");

        ClearAnswers();
        var answers = new List<string>(puzzle.Answers ?? new List<string>());
        Shuffle(answers);

        foreach (var answer in answers)
        {
            var buttonInstance = Instantiate(answerButtonPrefab, answersParent);

            var label = buttonInstance.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = answer;
            else
                Debug.LogWarning("Answer button prefab has no TMP_Text in children.");

            bool isCorrect = string.Equals(answer, puzzle.CorrectOne, StringComparison.Ordinal);

            string capturedAnswer = answer;
            buttonInstance.onClick.AddListener(() =>
            {
                HandleAnswerClick(isCorrect, capturedAnswer);
            });
        }

        timer.StartTimer(time);
    }

    private void ClearAnswers()
    {
        if (answersParent == null)
        {
            Debug.LogWarning("PuzzleUIController: answersParent is not assigned!");
            return;
        }

        for (int i = answersParent.childCount - 1; i >= 0; i--)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }
    }
    private void HandleAnswerClick(bool isCorrect, string answerText)
    {
        // Let other systems (score, FX, etc.) react
        OnAnswerClicked?.Invoke(isCorrect, answerText, currentPuzzle);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
