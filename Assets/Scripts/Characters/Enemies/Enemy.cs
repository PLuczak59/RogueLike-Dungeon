using UnityEngine;
using System.Collections.Generic;

public class Enemy : Character
{
    [Header("Enemy Configuration")]
    public string enemyName = "Enemy";

    public override void Initialize()
    {
        int randomHP = Random.Range(60, 100);
        int randomAttack = Random.Range(10, 30);
        int randomDefense = Random.Range(30, 60);
        int randomInitiative = Random.Range(40, 85);

        stats = new CharacterStats(enemyName, randomHP, randomAttack, randomDefense, randomInitiative);

        Debug.Log($"Ennemi créé: {enemyName} - HP:{randomHP} ATK:{randomAttack} DEF:{randomDefense} INI:{randomInitiative}");
    }

    public Character ChooseRandomTarget(List<Character> possibleTargets)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        var aliveTargets = possibleTargets.FindAll(t => t != null && t.IsAlive);

        if (aliveTargets.Count == 0)
        {
            return null;
        }

        return aliveTargets[Random.Range(0, aliveTargets.Count)];
    }
}