using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TurnState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}


public class TurnBased_Combat : MonoBehaviour
{
[Header("Turn State")]
public TurnState state;

[Header("Sprites/UI Images")]
public SpriteRenderer playerSprite;
public SpriteRenderer enemySprite;
public Image playerImage;
public Image enemyImage;

[Header("Units")]
public Unit playerUnit;
public Unit enemyUnit;

public Unit playerUnitPrefab;
public Unit enemyUnitPrefab;

[Header("UI Elements")]
public TextMeshProUGUI dialogueText;

public Canvas battleCanvas;

public BattleHud playerHUD;
public BattleHud enemyHUD;


public 

    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.START;
        StartCoroutine(setupBattle());
    }

    IEnumerator setupBattle()
    {
        playerUnitPrefab = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        
        enemyUnit.unitName = enemyUnitPrefab.unitName;
        enemyUnit.damage = enemyUnitPrefab.damage;
        enemyUnit.maxHealth = enemyUnitPrefab.maxHealth;
        enemyUnit.currentHealth = enemyUnitPrefab.currentHealth;

        playerUnit.unitName = playerUnitPrefab.unitName;
        playerUnit.damage = playerUnitPrefab.damage;
        playerUnit.maxHealth = playerUnitPrefab.maxHealth;
        playerUnit.currentHealth = playerUnitPrefab.currentHealth;

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";


        playerImage.sprite = playerSprite.sprite;
        enemyImage.sprite = enemySprite.sprite;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = TurnState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHealth);
        dialogueText.text = "You attack the " + enemyUnit.unitName + " for " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
              state = TurnState.WON;
              dialogueText.text = "You defeated the " + enemyUnit.unitName + "!";  
              EndBattle();  
        }
        else
        {
            state = TurnState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = "The " + enemyUnit.unitName + " attacks!";
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHealth);
        dialogueText.text = "The " + enemyUnit.unitName + " attacks you for " + enemyUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
              state = TurnState.LOST;
              dialogueText.text = "You were defeated by the " + enemyUnit.unitName + "...";  
              EndBattle();  
        }
        else
        {
            state = TurnState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == TurnState.WON)
        {
            dialogueText.text = "You won the battle!";
            battleCanvas.enabled = false;
        }
        else if(state == TurnState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void onAttackButton()
    {
        if (state != TurnState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

        public void onHealButton()
    {
        if (state != TurnState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(30);
        playerHUD.SetHP(playerUnit.currentHealth);
        dialogueText.text = "You heal yourself for 30 HP!";

        yield return new WaitForSeconds(2f);

        state = TurnState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}
