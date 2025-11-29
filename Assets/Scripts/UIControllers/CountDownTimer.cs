using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountdownTimer : MonoBehaviour
{

    public delegate void OnTimerEnd();
    public OnTimerEnd timerEndEvent;

    public int startTimeSeconds = 30;
    public TMP_Text timerText;


    private int _timeLeft;
    private Coroutine _timerRoutine;

    public void StartTimer(int seconds)
    {
        _timeLeft = Mathf.Max(0, seconds);
        UpdateText();

        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);

        _timerRoutine = StartCoroutine(TimerCoroutine());
    }

    public void StartTimer()
    {
        StartTimer(startTimeSeconds);
    }

    private System.Collections.IEnumerator TimerCoroutine()
    {
        while (_timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeLeft--;
            UpdateText();
        }

        _timerRoutine = null;
        timerEndEvent?.Invoke();
    }

    private void UpdateText()
    {
        if (timerText != null)
            timerText.text = _timeLeft.ToString();
    }
}
