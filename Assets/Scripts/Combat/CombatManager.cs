using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Positions for characters")]
    public Transform[] allyPositions;
    public Transform[] enemyPositions;


    [Header("Prefabs")]
    public GameObject heroPrefab;
    public GameObject enemyPrefab;
    public GameObject iconPrefab;


    [Header("UI Containers")]
    public Transform allyIconContainer;
    public Transform enemyIconContainer;


    [Header("Colors")]
    public Color allyColor = Color.green;
    public Color enemyColor = Color.red;

    private List<Character> allies = new List<Character>();
    private List<Character> enemies = new List<Character>();
    private List<FighterIconUI> allyIcons = new List<FighterIconUI>();
    private List<FighterIconUI> enemyIcons = new List<FighterIconUI>();
    private List<Character> turnOrder = new List<Character>();
    private int currentTurnIndex;
    private Character currentCharacter;
    private bool waitingForTargetSelection;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject heroObj = Instantiate(heroPrefab, allyPositions[i].position, allyPositions[i].rotation);
            Hero hero = heroObj.GetComponent<Hero>();
            if (hero != null)
            {
                hero.Initialize();
                allies.Add(hero);
                CreateIcon(hero, allyColor, allyIconContainer, allyIcons);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, enemyPositions[i].position, enemyPositions[i].rotation);
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.enemyName = $"Enemy {i + 1}";
                enemy.Initialize();
                enemies.Add(enemy);
                CreateIcon(enemy, enemyColor, enemyIconContainer, enemyIcons);
            }
        }

        StartCoroutine(CombatLoop());
    }

    private void CreateIcon(Character character, Color color, Transform container, List<FighterIconUI> iconList)
    {
        GameObject iconObj = Instantiate(iconPrefab, container);
        FighterIconUI icon = iconObj.GetComponent<FighterIconUI>();
        if (icon != null)
        {
            icon.Initialize(character, color);
            iconList.Add(icon);
        }
    }

    private IEnumerator CombatLoop()
    {
        while (true)
        {
            CalculateTurnOrder();

            foreach (Character fighter in turnOrder)
            {
                if (!fighter.Stats.IsAlive)
                    continue;

                currentCharacter = fighter;
                HighlightcurrentCharacter(fighter);

                if (allies.Contains(fighter))
                {
                    yield return StartCoroutine(PlayerTurn(fighter));
                }
                else
                {
                    yield return StartCoroutine(EnemyTurn(fighter));
                }

                UpdateAllIcons();

                if (CheckVictory())
                    yield break;
            }
        }
    }

    private void CalculateTurnOrder()
    {
        List<Character> allFighters = new List<Character>();
        allFighters.AddRange(allies);
        allFighters.AddRange(enemies);

        turnOrder = allFighters
            .Where(f => f != null && f.Stats != null && f.Stats.IsAlive)
            .OrderByDescending(f => f.Stats.initiative)
            .ToList();
    }

    private IEnumerator PlayerTurn(Character ally)
    {
        SetEnemyIconsClickable(true);
        waitingForTargetSelection = true;

        while (waitingForTargetSelection)
            yield return null;

        SetEnemyIconsClickable(false);
    }

    private IEnumerator EnemyTurn(Character enemy)
    {
        yield return new WaitForSeconds(1f);

        Character target = ChooseRandomTarget(allies);
        if (target != null)
        {
            Attack(enemy, target);
        }
    }

    private Character ChooseRandomTarget(List<Character> possibleTargets)
    {
        List<Character> aliveTargets = possibleTargets
            .Where(t => t != null && t.Stats != null && t.Stats.IsAlive)
            .ToList();

        if (aliveTargets.Count == 0)
            return null;

        return aliveTargets[Random.Range(0, aliveTargets.Count)];
    }

    public void OnFighterIconClicked(Character clicked)
    {
        if (!waitingForTargetSelection)
            return;

        if (!enemies.Contains(clicked))
            return;

        Attack(currentCharacter, clicked);
        waitingForTargetSelection = false;
    }

    private void Attack(Character attacker, Character target)
    {
        if (attacker == null || target == null || !target.Stats.IsAlive)
            return;

        int damage = CalculateDamage(attacker.Stats.attack, target.Stats.defense);
        target.Stats.TakeDamage(damage);

        Debug.Log($"{attacker.Stats.characterName} attaque {target.Stats.characterName} pour {damage} dégâts !");
    }

    private int CalculateDamage(int attack, int defense)
    {
        float defenseMultiplier = 1f - (defense / 100f);
        int damage = Mathf.RoundToInt(attack * defenseMultiplier);
        return Mathf.Max(1, damage); 
    }

    private void HighlightcurrentCharacter(Character fighter)
    {
        // Désactiver toutes les bordures
        foreach (var icon in allyIcons)
            icon.SetActiveTurn(false);
        foreach (var icon in enemyIcons)
            icon.SetActiveTurn(false);

        // Activer la bordure du fighter actuel
        FighterIconUI fighterIcon = GetIconForFighter(fighter);
        if (fighterIcon != null)
            fighterIcon.SetActiveTurn(true);
    }

    private FighterIconUI GetIconForFighter(Character fighter)
    {
        if (allies.Contains(fighter))
        {
            int index = allies.IndexOf(fighter);
            if (index >= 0 && index < allyIcons.Count)
                return allyIcons[index];
        }
        else if (enemies.Contains(fighter))
        {
            int index = enemies.IndexOf(fighter);
            if (index >= 0 && index < enemyIcons.Count)
                return enemyIcons[index];
        }
        return null;
    }

    private void SetEnemyIconsClickable(bool clickable)
    {
        foreach (var icon in enemyIcons)
            icon.SetClickable(clickable);
    }

    private void UpdateAllIcons()
    {
        foreach (var icon in allyIcons)
            icon.UpdateVisual();
        foreach (var icon in enemyIcons)
            icon.UpdateVisual();
    }

    private bool CheckVictory()
    {
        bool allEnemiesDead = enemies.All(e => e == null || !e.Stats.IsAlive);
        bool allAlliesDead = allies.All(a => a == null || !a.Stats.IsAlive);

        if (allEnemiesDead)
        {
            Debug.Log("VICTOIRE ! Tous les ennemis sont vaincus !");
            return true;
        }

        if (allAlliesDead)
        {
            Debug.Log("DÉFAITE ! Tous les alliés sont vaincus !");
            return true;
        }

        return false;
    }
}
