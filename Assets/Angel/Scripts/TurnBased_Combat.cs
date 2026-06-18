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

[SerializeField] private int ultimateCharge;

[Header("UI Elements")]
public TextMeshProUGUI dialogueText;

public Canvas battleCanvas;

public BattleHud playerHUD;
public BattleHud enemyHUD;

public GameObject GameoverScreen;

[Header("Combat Buttons")]
[SerializeField] private Button attackButton;
[SerializeField] private Button healButton;
[SerializeField] private Button UltimateButton;
[SerializeField] GameObject ButtonsUi;

    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
          if(state == TurnState.START)
            {
                StartCoroutine(setupBattle());
            }   
    }
    
    #region BattleStates
    IEnumerator setupBattle()
    {
        playerUnitPrefab = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        InputProvider inputProvider = GameObject.FindGameObjectWithTag("Player").GetComponent<InputProvider>();
        inputProvider.enabled = false;
     inputProvider.rb.velocity = Vector2.zero;
         inputProvider.rb.angularVelocity = 0f;
         inputProvider.DirX = 0f;
            

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
        enemyHUD.SetHP(enemyUnit.currentHealth);

        yield return new WaitForSeconds(1f);

        state = TurnState.PLAYERTURN;
        PlayerTurn();
    }

        void EndBattle()
    {
        if(state == TurnState.WON)
        {
            InputProvider inputProvider = GameObject.FindGameObjectWithTag("Player").GetComponent<InputProvider>();
            inputProvider.enabled = true;
            inputProvider.rb.velocity = Vector2.zero;
            inputProvider.rb.angularVelocity = 0f;
            inputProvider.DirX = 0f;
            dialogueText.text = "You won the battle!";
            enemyHUD.healthSlider.value = 1;
            battleCanvas.enabled = false;
            state = TurnState.START;
            this.GetComponent<TurnBased_Combat>().enabled = false;
            StopAllCoroutines();
        }
        else if(state == TurnState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }
    #endregion
    
    #region Player

    IEnumerator UltamineAttack()
    {
        int randomMultiplers = Random.Range(2,4);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage * randomMultiplers);
        attackButton.interactable = false;
        healButton.interactable = false;
        enemyHUD.SetHP(enemyUnit.currentHealth);
        dialogueText.text = "Unleashing ultimate, did " + (playerUnit.damage * randomMultiplers) + " damage";
        ultimateCharge = 0;

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
    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(30);
        playerHUD.SetHP(playerUnit.currentHealth);
        attackButton.interactable = false;
        healButton.interactable = false;
        dialogueText.text = "You heal yourself for 30 HP!";

        yield return new WaitForSeconds(2f);
        
        ultimateCharge++;
        state = TurnState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        attackButton.interactable = false;
        healButton.interactable = false;
        enemyHUD.SetHP(enemyUnit.currentHealth);
        dialogueText.text = "You attack the " + enemyUnit.unitName + " for " + playerUnit.damage + " damage!";
        ultimateCharge++;

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

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
        attackButton.interactable = true;
        healButton.interactable = true;
        if(ultimateCharge >= 3)
        {
            UltimateButton.interactable = true;
        }
        else
        {
            UltimateButton.interactable = false;
        }


    }

    #endregion

    #region Enemy
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
              battleCanvas.enabled = false;
              GameoverScreen.SetActive(true);
              ButtonsUi.SetActive(false);
              attackButton.interactable = false;
              UltimateButton.interactable = false;
              healButton.interactable = false;
        }
        else
        {
            state = TurnState.PLAYERTURN;
            PlayerTurn();
        }
    }
    #endregion

    #region Buttons
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

    public void OnUltimateButton()
    {
        if(state != TurnState.PLAYERTURN)
             return;
        StartCoroutine(UltamineAttack());
        

    }
    #endregion

}
