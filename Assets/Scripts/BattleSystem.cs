using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, PARTY1TURN, PARTY2TURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    [Header("Prefabs & Battle Stations")]
    public GameObject playerPrefab;
    public GameObject partyMemberOnePrefab;
    public GameObject partyMemberTwoPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform party1BattleStation;
    public Transform party2BattleStation;
    public Transform enemyBattleStation;

    private Unit playerUnit;
    private Unit party1Unit;
    private Unit party2Unit;
    private Unit enemyUnit;

    [Header("UI")]
    public TMP_Text dialogueText;
    public Button attackButton;
    public Button healButton;

    [Header("Combat & Scene Settings")]
    public int healAmount = 10;
    public string returnSceneName = "SampleScene";
    public string[] defeatIdsOnWin = { "MotherPigeon", "PencilBird" };

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        if (playerPrefab != null && playerBattleStation != null)
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
            playerUnit = playerGO.GetComponent<Unit>();
        }

        if (partyMemberOnePrefab != null && party1BattleStation != null)
        {
            GameObject party1GO = Instantiate(partyMemberOnePrefab, party1BattleStation);
            party1Unit = party1GO.GetComponent<Unit>();
        }

        if (partyMemberTwoPrefab != null && party2BattleStation != null)
        {
            GameObject party2GO = Instantiate(partyMemberTwoPrefab, party2BattleStation);
            party2Unit = party2GO.GetComponent<Unit>();
        }

        if (enemyPrefab != null && enemyBattleStation != null)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit>();
        }

        string foeName = enemyUnit != null ? enemyUnit.unitName : "a foe";
        if (dialogueText != null)
        {
            dialogueText.text = "You encountered: " + foeName;
        }

        SetButtonsInteractable(false);
        yield return new WaitForSeconds(1.5f);

        StartPlayerSideTurn();
    }

    void StartPlayerSideTurn()
    {
        if (IsAlive(playerUnit))
        {
            state = BattleState.PLAYERTURN;
            ShowTurnPrompt(playerUnit, "Player");
        }
        else if (IsAlive(party1Unit))
        {
            state = BattleState.PARTY1TURN;
            ShowTurnPrompt(party1Unit, "Party Member 1");
        }
        else if (IsAlive(party2Unit))
        {
            state = BattleState.PARTY2TURN;
            ShowTurnPrompt(party2Unit, "Party Member 2");
        }
        else
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    void AdvanceAfterPlayerAction()
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (IsAlive(party1Unit))
            {
                state = BattleState.PARTY1TURN;
                ShowTurnPrompt(party1Unit, "Party Member 1");
                return;
            }
            if (IsAlive(party2Unit))
            {
                state = BattleState.PARTY2TURN;
                ShowTurnPrompt(party2Unit, "Party Member 2");
                return;
            }
        }
        else if (state == BattleState.PARTY1TURN)
        {
            if (IsAlive(party2Unit))
            {
                state = BattleState.PARTY2TURN;
                ShowTurnPrompt(party2Unit, "Party Member 2");
                return;
            }
        }

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    bool IsAlive(Unit u)
    {
        return u != null && u.currentHP > 0;
    }

    void ShowTurnPrompt(Unit activeUnit, string fallbackName)
    {
        string actorName = activeUnit != null ? activeUnit.unitName : fallbackName;
        if (dialogueText != null)
        {
            dialogueText.text = actorName + ", choose an action:";
        }
        SetButtonsInteractable(true);
    }

    Unit GetActiveUnit()
    {
        if (state == BattleState.PLAYERTURN) return playerUnit;
        if (state == BattleState.PARTY1TURN) return party1Unit;
        if (state == BattleState.PARTY2TURN) return party2Unit;
        return null;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN && state != BattleState.PARTY1TURN && state != BattleState.PARTY2TURN)
            return;

        StartCoroutine(DoAttack(GetActiveUnit()));
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN && state != BattleState.PARTY1TURN && state != BattleState.PARTY2TURN)
            return;

        StartCoroutine(DoHeal(GetActiveUnit()));
    }

    IEnumerator DoAttack(Unit attacker)
    {
        SetButtonsInteractable(false);

        string attackerName = attacker != null ? attacker.unitName : "Ally";
        int dmg = attacker != null ? attacker.damage : 8;
        bool isDead = enemyUnit != null && enemyUnit.TakeDamage(dmg);

        if (dialogueText != null)
        {
            dialogueText.text = $"{attackerName} attacks for {dmg} damage!";
        }

        yield return new WaitForSeconds(1.2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            AdvanceAfterPlayerAction();
        }
    }

    IEnumerator DoHeal(Unit healer)
    {
        SetButtonsInteractable(false);

        if (healer != null)
        {
            healer.Heal(healAmount);
        }

        string healerName = healer != null ? healer.unitName : "Ally";
        if (dialogueText != null)
        {
            dialogueText.text = $"{healerName} heals for {healAmount} HP!";
        }

        yield return new WaitForSeconds(1.2f);

        AdvanceAfterPlayerAction();
    }

    IEnumerator EnemyTurn()
    {
        string foeName = enemyUnit != null ? enemyUnit.unitName : "Enemy";
        int dmg = enemyUnit != null ? enemyUnit.damage : 6;

        if (dialogueText != null)
        {
            dialogueText.text = $"{foeName} attacks!";
        }

        yield return new WaitForSeconds(1.0f);

        Unit target = IsAlive(playerUnit) ? playerUnit : (IsAlive(party1Unit) ? party1Unit : party2Unit);
        bool targetDefeated = target != null && target.TakeDamage(dmg);

        yield return new WaitForSeconds(1.0f);

        if (targetDefeated && !IsAlive(playerUnit) && !IsAlive(party1Unit) && !IsAlive(party2Unit))
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            StartPlayerSideTurn();
        }
    }

    void EndBattle()
    {
        SetButtonsInteractable(false);

        if (state == BattleState.WON)
        {
            if (defeatIdsOnWin != null)
            {
                foreach (string id in defeatIdsOnWin)
                {
                    BattleEncounterState.MarkDefeated(id);
                }
            }

            if (dialogueText != null)
            {
                dialogueText.text = "Victory! Returning to exploration...";
            }

            StartCoroutine(ReturnToExplorationAfterDelay(1.5f));
        }
        else
        {
            if (dialogueText != null)
            {
                dialogueText.text = "Defeat! Your party has fallen.";
            }
        }
    }

    IEnumerator ReturnToExplorationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(returnSceneName);
    }

    void SetButtonsInteractable(bool canAct)
    {
        if (attackButton != null) attackButton.interactable = canAct;
        if (healButton != null) healButton.interactable = canAct;
    }
}
