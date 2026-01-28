using UnityEngine;
using System.Collections.Generic;

public class HealManager : MonoBehaviour
{
    public PartyManager partymanager;

    public void ActivateHealing(RoomType roomType)
    {
        Debug.Log($"[HealManager] ActivateHealing appelé avec roomType: {roomType}");
        
        if (partymanager == null)
        {
            Debug.LogError("[HealManager] PartyManager est NULL !");
            return;
        }
        
        if (!partymanager.IsInitialized)
        {
            Debug.LogError("[HealManager] PartyManager n'est pas initialisé !");
            return;
        }
        
        Debug.Log($"[HealManager] PartyManager OK avec {partymanager.party.Count} membres");

        switch (roomType)
        {
            case RoomType.Event: // CampFire - Soins
                HealAliveMembers();
                break;
            case RoomType.Rest: // Fountain - Résurrection
                ReviveDeadMembers();
                break;
            default:
                Debug.LogWarning($"[HealManager] Type de salle non supporté pour les soins: {roomType}");
                break;
        }
    }

    private void HealAliveMembers()
    {
        Debug.Log("[HealManager] Début HealAliveMembers - Soins des membres vivants");
        
        int healedCount = 0;
        
        foreach (var member in partymanager.party)
        {
            if (member?.stats == null) 
            {
                Debug.LogWarning("[HealManager] Membre ou stats null détecté");
                continue;
            }
            
            Debug.Log($"[HealManager] Vérification {member.stats.characterName}: {member.stats.currentHP}/{member.stats.maxHP} HP, IsAlive: {member.stats.IsAlive}");
            
            if (member.stats.IsAlive && member.stats.currentHP < member.stats.maxHP)
            {
                int healAmount = member.stats.maxHP - member.stats.currentHP;
                Debug.Log($"[HealManager] Soins de {member.stats.characterName}: +{healAmount} HP");
                member.stats.Heal(healAmount);
                healedCount++;
                Debug.Log($"[HealManager] Après soins {member.stats.characterName}: {member.stats.currentHP}/{member.stats.maxHP} HP");
            }
        }
        
        if (healedCount > 0)
        {
            Debug.Log($"[HealManager] {healedCount} membre(s) soigné(s) au maximum au feu de camp !");
        }
        else
        {
            Debug.Log("[HealManager] Tous les membres vivants sont déjà à pleine santé.");
        }
    }

    private void ReviveDeadMembers()
    {
        Debug.Log("[HealManager] Activation de la fontaine - Résurrection des membres morts");
        
        int revivedCount = 0;
        
        foreach (var member in partymanager.party)
        {
            if (member?.stats == null) continue;
            
            if (!member.stats.IsAlive)
            {
                member.stats.currentHP = member.stats.maxHP;
                revivedCount++;
                Debug.Log($"[HealManager] {member.stats.characterName} a été ressuscité avec {member.stats.maxHP} HP (maximum) !");
            }
        }
        
        if (revivedCount > 0)
        {
            Debug.Log($"[HealManager] {revivedCount} membre(s) ressuscité(s) au maximum à la fontaine !");
        }
        else
        {
            Debug.Log("[HealManager] Aucun membre à ressusciter.");
        }
    }

    public void FullPartyHeal()
    {
        Debug.Log("[HealManager] Soin complet du groupe");
        
        foreach (var member in partymanager.party)
        {
            if (member?.stats == null) continue;
            
            if (!member.stats.IsAlive)
            {
                member.stats.currentHP = member.stats.maxHP;
                Debug.Log($"[HealManager] {member.stats.characterName} ressuscité et complètement soigné !");
            }

            else if (member.stats.currentHP < member.stats.maxHP)
            {
                member.stats.Heal(member.stats.maxHP - member.stats.currentHP);
            }
        }
    }

    public void LogPartyStatus()
    {
        Debug.Log("[HealManager] État actuel du groupe:");
        
        foreach (var member in partymanager.party)
        {
            if (member?.stats == null) continue;
            
            string status = member.stats.IsAlive ? "Vivant" : "Mort";
            Debug.Log($"- {member.stats.characterName}: {member.stats.currentHP}/{member.stats.maxHP} HP ({status})");
        }
    }
}
