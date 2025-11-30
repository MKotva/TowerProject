using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlanketController : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Image blanketImage;

    private Coroutine _currentRoutine;

    private void Awake()
    {
        if (blanketImage == null)
            blanketImage = GetComponent<Image>();
    }

    private void Start()
    {
        SetAlpha(1f);
    }
    public void RunFadeSequence(Action midAction)
    {
        if (blanketImage == null)
            return;

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeSequenceRoutine(midAction));
    }

    public void FadeToBlack(Action onFinished = null)
    {
        if (blanketImage == null)
            return;

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeTo(1f, fadeDuration, onFinished));
    }

    public void FadeFromBlack(Action onFinished = null)
    {
        if (blanketImage == null)
            return;

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeTo(0f, fadeDuration, onFinished));
    }

    private IEnumerator FadeSequenceRoutine(Action midAction)
    {
        yield return FadeTo(1f, fadeDuration);
        midAction?.Invoke();
        yield return FadeTo(0f, fadeDuration);

        _currentRoutine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration, Action onFinished = null)
    {
        if (blanketImage == null)
            yield break;

        if (duration <= 0f)
        {
            SetAlpha(targetAlpha);
            onFinished?.Invoke();
            yield break;
        }

        float startAlpha = blanketImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(newAlpha);

            yield return null;
        }

        SetAlpha(targetAlpha);
        onFinished?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        if (blanketImage == null)
            return;

        Color c = blanketImage.color;
        c.a = Mathf.Clamp01(alpha);
        blanketImage.color = c;
    }

    public void SetAlpha255(int alpha255)
    {
        float a = Mathf.Clamp01(alpha255 / 255f);
        SetAlpha(a);
    }
}