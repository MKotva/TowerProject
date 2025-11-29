using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UIControllers
{
    public class GoldPileController : MonoBehaviour
    {
        public TMP_Text TextField;

        public void Update()
        {
           if(GameManager.Instance != null)
            {
                TextField.text = GameManager.Instance.PlayerController.Gold.ToString();
            }
        }
    }
}
