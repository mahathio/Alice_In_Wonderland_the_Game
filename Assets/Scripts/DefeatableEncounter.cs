using UnityEngine;

public class DefeatableEncounter : MonoBehaviour
{
    public string encounterId = "MotherPigeon";

    void Start()
    {
        if (BattleEncounterState.IsDefeated(encounterId))
        {
            gameObject.SetActive(false);
        }
    }
}
