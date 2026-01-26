using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy : Character
{
    [Header("Enemy Configuration")]
    public string enemyName = "Enemy";
    public int baseMaxHP = 80;
    public int baseAttack = 25;
    public int baseDefense = 40;
    public int baseInitiative = 60;

    public override void Initialize()
    {
        int randomHP = Random.Range(60, 101);
        int randomAttack = Random.Range(20, 40);
        int randomDefense = Random.Range(30, 60);
        int randomInitiative = Random.Range(40, 85);

        stats = new CharacterStats(enemyName, randomHP, randomAttack, randomDefense, randomInitiative);
        Debug.Log($"Ennemi initialisé: {enemyName} - HP:{randomHP} ATK:{randomAttack} DEF:{randomDefense} INI:{randomInitiative}");
    }

    public void SetStats(string name, int hp, int atk, int def, int ini)
    {
        enemyName = name;
        baseMaxHP = hp;
        baseAttack = atk;
        baseDefense = def;
        baseInitiative = ini;
        Initialize();
    }

    public Character ChooseTarget(List<Character> possibleTargets)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
            return null;

        var aliveTargets = possibleTargets
            .Where(t => t != null && t.Stats != null && t.Stats.IsAlive)
            .ToList();

        Character chosenTarget = null;
        if(aliveTargets.Count > 0)
        {
            chosenTarget = aliveTargets[Random.Range(0, aliveTargets.Count)];
        }
        return chosenTarget;
    }
}