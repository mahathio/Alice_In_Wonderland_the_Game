using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] string fightSceneName = "FightScene";
    [SerializeField] bool triggerOnce = true;

    bool hasTriggered;

    private void OnTriggerEnter(Collider collider)
    {
        if (triggerOnce && hasTriggered) return;

        if (collider.CompareTag("Player"))
        {
            hasTriggered = true;
            SceneManager.LoadScene(fightSceneName);
        }
    }
}