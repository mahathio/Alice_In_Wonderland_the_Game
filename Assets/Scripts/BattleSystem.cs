using UnityEngine;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject partyMemberOnePrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform party1BattleStation;
    public Transform enemyBattleStation;

    void Start()
    {
        state = BattleState.START;
        SetuoBattle();
    }

    void SetuoBattle()
    {
        Instantiate(playerPrefab, playerBattleStation);
        Instantiate(partyMemberOnePrefab, party1BattleStation);
        Instantiate(enemyPrefab, enemyBattleStation);
    }
}
