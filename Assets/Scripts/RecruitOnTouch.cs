using UnityEngine;

public class RecruitOnTouch : MonoBehaviour
{
    public string memberId = "PartyMember2";
    private PartyFollower followerScript;
    private Collider2D triggerCollider;

    void Awake()
    {
        followerScript = GetComponent<PartyFollower>();
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (followerScript != null)
            {
                followerScript.enabled = true;
            }

            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
        }
    }
}