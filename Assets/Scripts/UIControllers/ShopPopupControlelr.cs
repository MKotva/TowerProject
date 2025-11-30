using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopupControlelr : MonoBehaviour
{

    public Transform itemsContainer;
    public Button exitButton;
    public TMP_Text headerText;

    public void Init(Action onClose)
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(() => onClose?.Invoke());
        }
    }

    public void SetHeader(string text)
    {
        if (headerText != null)
            headerText.text = text;
    }
}
