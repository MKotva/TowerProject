using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartsController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform heartsParent;      
    [SerializeField] private Image heartPrefab;         
    [SerializeField] private Sprite fullHeartSprite;    
    [SerializeField] private Sprite emptyHeartSprite;   

    private readonly List<Image> _hearts = new List<Image>();
    private PlayerController _player;

    private void Start()
    {
        _player = GameManager.Instance.PlayerController;
        CreateHearts();
        UpdateHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    private void CreateHearts()
    {
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        _hearts.Clear();

        for (int i = 0; i < _player.MaxLives; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsParent);
            heart.sprite = emptyHeartSprite;
            _hearts.Add(heart);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            double heartIndex = i + 1;
            if (heartIndex <= _player.Lives)
            {
                _hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                _hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}