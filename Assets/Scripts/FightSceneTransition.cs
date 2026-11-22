using UnityEngine;
using UnityEngine.SceneManagement;

public class FightSceneTransition : MonoBehaviour
{
    public int sceneBuildIndex;
    public string encounterId = "MotherPigeon";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (BattleEncounterState.IsDefeated(encounterId))
                return;

            BattleEncounterState.CurrentEncounterId = encounterId;
            print("Switching scene to " + sceneBuildIndex);
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
