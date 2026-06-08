using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
   public TextMeshProUGUI nameText;
   public TextMeshProUGUI healthText;
   public Slider healthSlider;

   public void SetHUD(Unit unit)
   {
       nameText.text = unit.unitName;
       healthText.text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
       healthSlider.maxValue = unit.maxHealth;
       healthSlider.value = unit.currentHealth;
   }

   public void SetHP(int hp)
   {
       healthSlider.value = hp;
       healthText.text = "HP: " + hp + "/" + healthSlider.maxValue;
   }
}
