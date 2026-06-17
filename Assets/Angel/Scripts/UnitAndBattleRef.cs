using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Unit))]
public class UnitAndBattleRef : MonoBehaviour
{
    private SpriteRenderer Sp;
   public TurnBased_Combat turnBasedCombat;
   [SerializeField] private Unit _enemyUnit;
   [SerializeField] private SpriteRenderer _enemySprite;
   [SerializeField] private Canvas battleCanvas;
   private BoxCollider2D _collider;
   void Start()
   {
       Sp = GetComponent<SpriteRenderer>();
       turnBasedCombat = GameObject.Find("BattleSystem").GetComponent<TurnBased_Combat>();
       _enemyUnit = GetComponent<Unit>();
       _enemySprite = GetComponent<SpriteRenderer>();
       _collider = GetComponent<BoxCollider2D>();
   }

   private void OnTriggerEnter2D(Collider2D collision){
       if (collision.CompareTag("Player"))
       {
           Sp.enabled = false;
           turnBasedCombat.enabled = true;
           turnBasedCombat.enemySprite = _enemySprite;
           turnBasedCombat.enemyUnitPrefab = _enemyUnit;
           battleCanvas.enabled = true;
           turnBasedCombat.GetComponent<TurnBased_Combat>().enabled = true;
           _collider.enabled = false;
       }
   }



}
