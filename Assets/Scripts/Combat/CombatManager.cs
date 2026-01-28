using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Positions for characters")]
    public Transform[] allyPositions;
    public Transform[] enemyPositions;


    [Header("Prefabs")]
    public GameObject[] heroPrefabs;
    public GameObject enemyPrefab;
    public GameObject iconPrefab;
    public GameObject turnIconPrefab;


    [Header("UI Containers")]
    public Transform allyIconContainer;
    public Transform enemyIconContainer;
    public Transform turnOrderContainer;


    [Header("Colors")]
    public Color allyColor = Color.green;
    public Color enemyColor = Color.red;

    private List<Character> allies = new List<Character>();
    private List<Character> enemies = new List<Character>();
    private List<FighterIconUI> allyIcons = new List<FighterIconUI>();
    private List<FighterIconUI> enemyIcons = new List<FighterIconUI>();
    private List<Character> turnOrder = new List<Character>();
    private List<GameObject> turnOrderIcons = new List<GameObject>();
    private Character currentCharacter;
    private bool waitingForTargetSelection;

    private PartyManager partyManager;
    private Coroutine combatCoroutine;
    private readonly List<GameObject> spawnedCharacters = new List<GameObject>();

    public event Action<bool> CombatEnded;

    private void Awake()
    {
        Instance = this;
    }

    public void StartCombat(PartyManager party)
    {
        partyManager = party;
        CleanupCombat();
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        if (partyManager == null || !partyManager.IsInitialized)
        {
            Debug.LogError("[CombatManager] PartyManager non initialisé. Impossible de démarrer le combat.");
            return;
        }

        for (int i = 0; i < partyManager.party.Count; i++)
        {
            if (i >= allyPositions.Length)
                break;

            PartyMember member = partyManager.party[i];
            if (member == null || member.prefab == null || member.stats == null)
                continue;

            GameObject heroObj = Instantiate(member.prefab, allyPositions[i].position, allyPositions[i].rotation);
            spawnedCharacters.Add(heroObj);

            Hero hero = heroObj.GetComponent<Hero>();
            if (hero == null)
                continue;

            hero.SetStats(member.stats.Clone());
            allies.Add(hero);
            CreateIcon(hero, allyColor, allyIconContainer, allyIcons);
        }

        for (int i = 0; i < 4 && i < enemyPositions.Length; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, enemyPositions[i].position, enemyPositions[i].rotation);
            spawnedCharacters.Add(enemyObj);

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy == null)
                continue;

            enemy.enemyName = $"Enemy {i + 1}";
            enemy.Initialize();
            enemies.Add(enemy);
            CreateIcon(enemy, enemyColor, enemyIconContainer, enemyIcons);
        }

        combatCoroutine = StartCoroutine(CombatLoop());
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
            UpdateTurnOrderDisplay();

            foreach (Character fighter in turnOrder)
            {
                if (!fighter.Stats.IsAlive)
                    continue;

                currentCharacter = fighter;
                HighlightCurrentFighter(fighter);
                UpdateTurnOrderDisplay();

                if (allies.Contains(fighter))
                {
                    yield return StartCoroutine(PlayerTurn(fighter));
                }
                else
                {
                    yield return StartCoroutine(EnemyTurn(fighter));
                }

                UpdateAllIcons();
                UpdateTurnOrderDisplay();

                bool? result = CheckVictory();
                if (result.HasValue)
                {
                    EndCombat(result.Value);
                    yield break;
                }
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
        {
            yield return null;
        }

        SetEnemyIconsClickable(false);
    }

    private IEnumerator EnemyTurn(Character enemy)
    {
        yield return new WaitForSeconds(1f);

        if (enemy is Enemy enemyScript)
        {
            Character target = enemyScript.ChooseRandomTarget(allies);
            if (target != null)
            {
                enemy.Attack(target);
            }
        }
    }

    public void OnFighterIconClicked(Character clicked)
    {
        if (!waitingForTargetSelection || !enemies.Contains(clicked))
        {
            return;
        }

        currentCharacter.Attack(clicked);
        waitingForTargetSelection = false;
    }

    private void HighlightCurrentFighter(Character fighter)
    {
        foreach (var icon in allyIcons)
        {
            icon.SetActiveTurn(false);
        }

        foreach (var icon in enemyIcons)
        {
            icon.SetActiveTurn(false);
        }

        FighterIconUI fighterIcon = GetIconForFighter(fighter);
        if (fighterIcon != null)
        {
            fighterIcon.SetActiveTurn(true);
        }
    }

    private FighterIconUI GetIconForFighter(Character fighter)
    {
        int index = allies.IndexOf(fighter);
        if (index >= 0 && index < allyIcons.Count)
        {
            return allyIcons[index];
        }

        index = enemies.IndexOf(fighter);
        if (index >= 0 && index < enemyIcons.Count)
        {
            return enemyIcons[index];
        }

        return null;
    }

    private void SetEnemyIconsClickable(bool clickable)
    {
        foreach (var icon in enemyIcons)
        {
            icon.SetClickable(clickable);
        }
    }

    private void UpdateAllIcons()
    {
        foreach (var icon in allyIcons)
        {
            icon.UpdateIconUI();
        }

        foreach (var icon in enemyIcons)
        {
            icon.UpdateIconUI();
        }
    }

    private void UpdateTurnOrderDisplay()
    {
        foreach (var icon in turnOrderIcons)
        {
            if (icon != null)
                Destroy(icon);
        }
        turnOrderIcons.Clear();

        for (int i = 0; i < turnOrder.Count; i++)
        {
            Character fighter = turnOrder[i];
            if (fighter == null || !fighter.IsAlive)
            {
                continue;
            }

            GameObject iconObj = Instantiate(turnIconPrefab, turnOrderContainer);
            ActivateTurnIconImage(iconObj, fighter);
            SetTurnIconNumber(iconObj, i + 1);
            SetTurnIconBackground(iconObj, fighter);
            HighlightTurnIcon(iconObj, fighter == currentCharacter);
            turnOrderIcons.Add(iconObj);
        }
    }

    private void SetTurnIconNumber(GameObject iconObj, int number)
    {
        TMPro.TextMeshProUGUI numberText = iconObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (numberText != null)
        {
            numberText.text = number.ToString();
            return;
        }

        UnityEngine.UI.Text text = iconObj.GetComponentInChildren<UnityEngine.UI.Text>();
        if (text != null)
        {
            text.text = number.ToString();
        }
    }

    private void SetTurnIconBackground(GameObject iconObj, Character fighter)
    {
        UnityEngine.UI.Image backgroundImage = iconObj.GetComponent<UnityEngine.UI.Image>();
        if (backgroundImage != null)
        {
            backgroundImage.color = allies.Contains(fighter) ? allyColor : enemyColor;
        }
    }

    private void HighlightTurnIcon(GameObject iconObj, bool isActive)
    {
        string[] tags = { "HealerImg", "MageImg", "TankImg", "AssassinImg", "EnemyImg" };
        UnityEngine.UI.Image[] allImages = iconObj.GetComponentsInChildren<UnityEngine.UI.Image>();
        Color highlightColor = isActive ? new Color(1f, 1f, 0.5f, 1f) : Color.white;

        foreach (var img in allImages)
        {
            bool isTaggedImage = false;
            foreach (string tag in tags)
            {
                if (img.CompareTag(tag))
                {
                    isTaggedImage = true;
                    break;
                }
            }

            if (isTaggedImage)
            {
                img.color = highlightColor;
            }
        }
    }

    private void ActivateTurnIconImage(GameObject iconObj, Character fighter)
    {
        string[] tags = { "HealerImg", "MageImg", "TankImg", "AssassinImg", "EnemyImg" };
        UnityEngine.UI.Image[] allImages = iconObj.GetComponentsInChildren<UnityEngine.UI.Image>(true);
        foreach (var img in allImages)
        {
            foreach (string tag in tags)
            {
                if (img.CompareTag(tag))
                {
                    img.gameObject.SetActive(false);
                }
            }
        }

        string imageTag = GetImageTagForCharacter(fighter);
        if (!string.IsNullOrEmpty(imageTag))
        {
            foreach (var img in allImages)
            {
                if (img.CompareTag(imageTag))
                {
                    img.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private string GetImageTagForCharacter(Character character)
    {
        if (character is Enemy)
        {
            return "EnemyImg";
        }
        else if (character is Hero hero)
        {
            string heroName = hero.heroName.ToLower();

            if (heroName.Contains("healer"))
            {
                return "HealerImg";
            }
            else if (heroName.Contains("mage"))
            {
                return "MageImg";
            }
            else if (heroName.Contains("tank"))
            {
                return "TankImg";
            }
            else if (heroName.Contains("assassin"))
            {
                return "AssassinImg";
            }
        }

        return "EnemyImg";
    }

    private bool? CheckVictory()
    {
        bool allEnemiesDead = enemies.All(e => e?.Stats?.IsAlive != true);
        bool allAlliesDead = allies.All(a => a?.Stats?.IsAlive != true);

        if (allEnemiesDead)
        {
            Debug.Log("VICTOIRE ! Tous les ennemis sont vaincus !");
            return true;
        }

        if (allAlliesDead)
        {
            Debug.Log("DÉFAITE ! Tous les alliés sont vaincus !");
            return false;
        }

        return null;
    }

    private void EndCombat(bool victory)
    {
        if (partyManager != null)
            partyManager.UpdateFromCharacters(allies);

        CombatEnded?.Invoke(victory);
    }

    private void CleanupCombat()
    {
        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;
        }

        waitingForTargetSelection = false;
        currentCharacter = null;

        foreach (var go in spawnedCharacters)
        {
            if (go != null)
                Destroy(go);
        }
        spawnedCharacters.Clear();

        foreach (var icon in allyIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }
        foreach (var icon in enemyIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }
        allyIcons.Clear();
        enemyIcons.Clear();

        foreach (var icon in turnOrderIcons)
        {
            if (icon != null)
                Destroy(icon);
        }
        turnOrderIcons.Clear();

        allies.Clear();
        enemies.Clear();
        turnOrder.Clear();
    }
}
