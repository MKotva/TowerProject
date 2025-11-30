using Assets.Core;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStats
{
    //Player should get 3 stats base
    public int STR = 0;
    public int END = 0;
    public int AGI = 0;
    public int INT = 0;
    public int LCK = 0;
}
public class PlayerController : Entity
{
    public bool isInCombat;
    public int Gold = 100;

    public List<IItem> Items {  get; set; }

    public int HPPotions = 0;

    public int StaminaPotions = 0;
    private bool isDying = false;
    new void Start()
    {
        base.Start();
        Items = new List<IItem>();
    }

    void Update()
    {
        if(Lives <= 0 && !isDying)
        {
            isDying = true;
            GameManager.Instance.ScreenBlanker.FadeToBlack(() =>
            {
                SceneManager.LoadScene("GameLostScene");
            });
        }
    }

    public void Init()
    {
        Gold = GameData.Gold;
        Items = GameData.Items;
        HP = GameData.HP;
        Lives = GameData.Lives;
        SkillSet = GameData.SkillSet;
        Weapon = GameData.Weapon;
        Armor = GameData.Armor;
    }

    public void HealPlayer()
    {
       Lives = MaxLives;
       HP = LiveHP;
    }
}
