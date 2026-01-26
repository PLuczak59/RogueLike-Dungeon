using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public CharacterStats stats;

    public CharacterStats Stats => stats;

    // Méthode virtuelle pour l'initialisation
    public virtual void Initialize()
    {
    }
}
