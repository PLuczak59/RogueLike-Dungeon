using UnityEngine;

public class Hero : Character
{
    [Header("Hero Configuration")]
    public string heroName = "Hero";
    public int baseMaxHP = 80;
    public int baseAttack = 25;
    public int baseDefense = 40;
    public int baseInitiative = 60;

    public override void Initialize()
    {
        stats = new CharacterStats(heroName, baseMaxHP, baseAttack, baseDefense, baseInitiative);
        Debug.Log($"Héros initialisé: {heroName} - HP:{baseMaxHP} ATK:{baseAttack} DEF:{baseDefense} INI:{baseInitiative}");
    }

    public void SetStats(string name, int hp, int atk, int def, int ini)
    {
        heroName = name;
        baseMaxHP = hp;
        baseAttack = atk;
        baseDefense = def;
        baseInitiative = ini;
        Initialize();
    }
}
