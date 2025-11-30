using UnityEngine;
using UnityEngine.EventSystems;

public class HoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum SlotType
    {
        Weapon,
        Armor,
        HpPotion,
        StaminaPotion
    }

    [Header("Config")]
    public PlayerEquipmentPanel panel;   // reference to your panel script
    public SlotType slotType;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panel == null) return;

        switch (slotType)
        {
            case SlotType.Weapon:
                panel.ShowWeaponTooltip();
                break;
            case SlotType.Armor:
                panel.ShowArmorTooltip();
                break;
            case SlotType.HpPotion:
                panel.ShowHpPotionTooltip();
                break;
            case SlotType.StaminaPotion:
                panel.ShowStaminaPotionTooltip();
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panel == null) return;
        panel.HideTooltip();
    }
}
