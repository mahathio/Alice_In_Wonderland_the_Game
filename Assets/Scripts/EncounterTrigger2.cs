using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterTrigger2 : MonoBehaviour
{
    [SerializeField] string fightSceneName = "QueenOfHearts";
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