using Assets.Scripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementController : MonoBehaviour
{
    [Header("Announcement")]
    public TMP_Text messageText;
    public Button exitButton;

    [Header("Decision / Payment (optional)")]
    public GameObject paymentPanel;
    public TMP_Text priceText;
    public Image coinImage;
    public Button acceptButton;
    public Button declineButton;

    [Header("Visuals")]
    public Image acceptButtonBackground;
    public Color affordableColor = Color.white;
    public Color unaffordableColor = Color.gray;

    private int _price;
    private bool _isDecision;
    private Action<bool> _onFinished;


    public void Init(string message, int price, bool isDecision, Action<bool> onFinished)
    {
        _price = Mathf.Max(0, price);
        _isDecision = isDecision || ( price > 0 );
        _onFinished = onFinished;

        if (messageText != null)
            messageText.text = message;

        var player = GameManager.Instance.PlayerController;
        bool hasPrice = _price > 0;

        bool infoOnly = !_isDecision && !hasPrice;

        if (infoOnly)
        {
            if (paymentPanel != null)
                paymentPanel.SetActive(false);

            if (exitButton != null)
            {
                exitButton.gameObject.SetActive(true);
                exitButton.onClick.RemoveAllListeners();
                exitButton.onClick.AddListener(OnExit);
            }

            if (acceptButton != null) acceptButton.onClick.RemoveAllListeners();
            if (declineButton != null) declineButton.onClick.RemoveAllListeners();
        }
        else
        {
            if (paymentPanel != null)
                paymentPanel.SetActive(true);

            if (exitButton != null)
            {
                exitButton.onClick.RemoveAllListeners();
                exitButton.gameObject.SetActive(false);
            }

            if (priceText != null)
            {
                if (hasPrice)
                    priceText.text = _price.ToString();
                else
                {
                    priceText.text = "";
                    coinImage.enabled = false;
                }
            }

            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveAllListeners();

                if (hasPrice)
                {
                    bool canAfford = player.Gold >= _price;
                    if (canAfford)
                        acceptButton.onClick.AddListener(OnAccept);
                    else
                        acceptButton.onClick.AddListener(OnCantAffordClicked);

                    SetAcceptAffordability(canAfford);
                }
                else
                {
                    acceptButton.onClick.AddListener(OnAccept);
                    SetAcceptAffordability(true);
                }
            }

            if (declineButton != null)
            {
                declineButton.onClick.RemoveAllListeners();
                declineButton.onClick.AddListener(OnDecline);
            }
        }
    }

    public void RefreshAffordability()
    {
        if (_price <= 0 || acceptButton == null)
            return;

        var player = GameManager.Instance.PlayerController;
        bool canAfford = player.Gold >= _price;

        acceptButton.onClick.RemoveAllListeners();
        if (canAfford)
            acceptButton.onClick.AddListener(OnAccept);
        else
            acceptButton.onClick.AddListener(OnCantAffordClicked);

        SetAcceptAffordability(canAfford);
    }

    private void SetAcceptAffordability(bool canAfford)
    {
        if (acceptButton != null)
            acceptButton.interactable = canAfford;

        var bg = acceptButtonBackground != null ? acceptButtonBackground : acceptButton != null ? acceptButton.targetGraphic as Image : null;

        if (bg != null)
            bg.color = canAfford ? affordableColor : unaffordableColor;
    }

    private void OnAccept()
    {
        if (_price > 0)
        {
            var player = GameManager.Instance.PlayerController;
            player.Gold -= _price;
        }

        _onFinished?.Invoke(true);
        Destroy(gameObject);
    }

    private void OnDecline()
    {
        _onFinished?.Invoke(false);
        Destroy(gameObject);
    }

    private void OnExit()
    {
        OnDecline();
    }

    private void OnCantAffordClicked()
    {
    }
}