using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    // Attributs publics
    public string characterName;
    public int maxHP;
    public int currentHP;
    public int attack;
    [Range(0, 100)]
    public int defense;
    public int initiative;

    // Properties
    public bool IsAlive => currentHP > 0;
    public float HPPercent => maxHP > 0 ? (float)currentHP / maxHP : 0f;

    // Constructeur
    public CharacterStats(string name, int hp, int atk, int def, int ini)
    {
        characterName = name;
        maxHP = hp;
        currentHP = hp;
        attack = atk;
        defense = Mathf.Clamp(def, 0, 100);
        initiative = ini;
    }

    // Méthode pour prendre des dégâts
    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CharacterStats] {characterName} : Tentative de dégâts invalides ({amount})");
            return;
        }

        int previousHP = currentHP;
        currentHP = Mathf.Max(0, currentHP - amount);
        int actualDamage = previousHP - currentHP;

        Debug.Log($"[CharacterStats] {characterName} subit {actualDamage} dégâts ! HP: {previousHP} -> {currentHP}/{maxHP}");

        if (!IsAlive)
        {
            Debug.Log($"[CharacterStats] {characterName} est vaincu !");
        }
    }

    // Méthode pour se soigner
    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CharacterStats] {characterName} : Tentative de soin invalide ({amount})");
            return;
        }

        int previousHP = currentHP;
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        int actualHealing = currentHP - previousHP;

        if (actualHealing > 0)
        {
            Debug.Log($" [CharacterStats] {characterName} récupère {actualHealing} HP ! HP: {previousHP} -> {currentHP}/{maxHP}");
        }
        else
        {
            Debug.Log($"[CharacterStats] {characterName} est déjà à pleine vie ! ({currentHP}/{maxHP})");
        }
    }
}
