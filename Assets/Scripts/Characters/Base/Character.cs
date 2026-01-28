using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterStats stats;
    [SerializeField] protected Renderer visualRenderer;

    public CharacterStats Stats => stats;
    public bool IsAlive => (stats != null && stats.IsAlive);

    public void SetStats(CharacterStats newStats)
    {
        stats = newStats;
    }

    public abstract void Initialize();

    protected virtual void Awake()
    {
        visualRenderer = GetComponent<Renderer>();
    }

    public virtual void Attack(Character target)
    {
        if (!IsAlive || target == null || !target.IsAlive)
        {
            return;
        }

        int baseDamage = stats.attack + Random.Range(-5, 6);
        float defReduction = 1f - (target.Stats.defense / 200f);
        defReduction = Mathf.Clamp01(defReduction);
        int finalDamage = Mathf.Max(1, Mathf.CeilToInt(baseDamage * defReduction));

        target.TakeDamage(finalDamage);

        Debug.Log($"{stats.characterName} attaque {target.Stats.characterName} pour {finalDamage} dégâts!");
    }

    public virtual void TakeDamage(int damage)
    {
        if (stats == null)
        {
            return;
        }

        stats.TakeDamage(damage);

        if (!IsAlive)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"☠️ {stats.characterName} est K.O. !");
        gameObject.SetActive(false);
    }
}
