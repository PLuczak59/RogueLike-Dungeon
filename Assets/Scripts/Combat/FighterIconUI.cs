using UnityEngine;
using UnityEngine.UI;

public class FighterIconUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage;
    public Image borderImage;
    public Character linkedFighter;

    private Button button;
    private Color aliveColor;
    private Color deadColor = Color.gray;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnIconClicked);
        }
    }

    public void Initialize(Character fighter, Color color)
    {
        linkedFighter = fighter;
        aliveColor = color;
        iconImage.color = aliveColor;
        borderImage.enabled = false;
    }

    private void OnIconClicked()
    {
        if (linkedFighter != null && linkedFighter.Stats != null && linkedFighter.Stats.IsAlive)
        {
            CombatManager.Instance.OnFighterIconClicked(linkedFighter);
        }
    }

    public void UpdateVisual()
    {
        if (linkedFighter != null && linkedFighter.Stats != null && !linkedFighter.Stats.IsAlive)
        {
            iconImage.color = deadColor;
            button.interactable = false;
        }
    }

    public void SetActiveTurn(bool active)
    {
        borderImage.enabled = active;
    }

    public void SetClickable(bool clickable)
    {
        button.interactable = clickable && linkedFighter != null && linkedFighter.Stats != null && linkedFighter.Stats.IsAlive;
    }
}
