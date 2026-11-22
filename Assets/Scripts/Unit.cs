using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName = "Hero";
    public int maxHP = 30;
    public int currentHP = 30;
    public int damage = 8;

    private void Awake()
    {
        currentHP = Mathf.Clamp(currentHP > 0 ? currentHP : maxHP, 1, maxHP);
    }

    public bool TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        return currentHP <= 0;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }
}