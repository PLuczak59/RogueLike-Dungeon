using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyMember
{
    public GameObject prefab;
    public CharacterStats stats;
}

public class PartyManager : MonoBehaviour
{
    [Header("Party")]
    public List<PartyMember> party = new List<PartyMember>();

    public bool IsInitialized => party != null && party.Count > 0;

    public void InitializeFromPrefabs(GameObject[] heroPrefabs)
    {
        if (IsInitialized)
            return;

        party = new List<PartyMember>();

        if (heroPrefabs == null)
            return;

        foreach (var prefab in heroPrefabs)
        {
            if (prefab == null)
                continue;

            var hero = prefab.GetComponent<Hero>();
            if (hero == null)
            {
                Debug.LogWarning($"[PartyManager] Prefab hero sans composant Hero: {prefab.name}");
                continue;
            }

            var stats = new CharacterStats(
                hero.heroName,
                hero.baseMaxHP,
                hero.baseAttack,
                hero.baseDefense,
                hero.baseInitiative
            );

            party.Add(new PartyMember { prefab = prefab, stats = stats });
        }
    }

    public void UpdateFromCharacters(List<Character> allies)
    {
        if (party == null || allies == null)
            return;

        for (int i = 0; i < party.Count && i < allies.Count; i++)
        {
            if (allies[i]?.Stats == null || party[i] == null)
                continue;

            party[i].stats = allies[i].Stats.Clone();
        }
    }
}

