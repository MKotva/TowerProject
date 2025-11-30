using Assets.Core.Items;
using Assets.Scripts;
using UnityEngine;

public class ButtonPotion : MonoBehaviour
{
    public PotionType PotionType;
    public void On_Click()
    {
        var player=GameManager.Instance.PlayerController;

        bool enoughPotions = false;
        if (PotionType == PotionType.HP && player.HPPotions > 0) enoughPotions = true;
        if (PotionType == PotionType.Endurance && player.StaminaPotions>0) enoughPotions = true;
        if (enoughPotions)
        {
            for (int i = 0; i< player.Items.Count; i++)
            {
                var item = player.Items[i];
                if (item is Potion)
                {
                    var potion = (Potion)item;
                    if(potion.Type == PotionType)
                    {
                        potion.Use(player);
                        player.Items.RemoveAt(i);
                        if (PotionType == PotionType.HP)
                        {
                            player.HPPotions--;
                        }
                        if (PotionType == PotionType.Endurance)
                        {
                            player.StaminaPotions--;
                        }
                    }
                }
            }
        }
    }
}
