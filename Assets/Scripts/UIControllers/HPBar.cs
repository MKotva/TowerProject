using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartsController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform heartsParent;      // e.g. a Horizontal Layout Group
    [SerializeField] private Image heartPrefab;          // prefab with an Image component
    [SerializeField] private Sprite fullHeartSprite;     // red heart sprite
    [SerializeField] private Sprite emptyHeartSprite;    // black heart sprite

    private readonly List<Image> _hearts = new List<Image>();
    private PlayerController _player;

    private void Awake()
    {
        // Get player from GameManager singleton
        _player = GameManager.Instance.PlayerController;
    }

    private void Start()
    {
        CreateHearts();
        UpdateHearts();
    }

    private void Update()
    {
        // Easiest: refresh every frame.
        // For a polished version, call UpdateHearts() only when HP changes.
        UpdateHearts();
    }

    private void CreateHearts()
    {
        // Clear existing children (if any)
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        _hearts.Clear();

        int maxHearts = Mathf.CeilToInt((float) _player.MaxHP);

        for (int i = 0; i < maxHearts; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsParent);
            heart.sprite = emptyHeartSprite;
            _hearts.Add(heart);
        }
    }

    private void UpdateHearts()
    {
        double hp = _player.HP;

        for (int i = 0; i < _hearts.Count; i++)
        {
            // Heart i represents HP in (i, i+1]
            double heartIndex = i + 1;

            if (hp >= heartIndex)
            {
                // full heart
                _hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                // empty heart
                _hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}