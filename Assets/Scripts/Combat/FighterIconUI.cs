using UnityEngine;
using UnityEngine.UI;

public class FighterIconUI : MonoBehaviour
{
    public Character linkedFighter;
    private Button button;
    private Color aliveColor;
    private Color deadColor = Color.gray;
    private Image activeImage;
    public Image iconImage;

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
        ActivateCharacterImage();
    }

    private void ActivateCharacterImage()
    {
        DesactivateAllTaggedImages();

        string imageTag = GetImageTagForCharacter(linkedFighter);
        if (string.IsNullOrEmpty(imageTag))
        {
            return;
        }

        Image[] allImages = GetComponentsInChildren<Image>(true);
        foreach (Image img in allImages)
        {
            if (img.CompareTag(imageTag))
            {
                img.gameObject.SetActive(true);
                activeImage = img;
                break;
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

    private void DesactivateAllTaggedImages()
    {
        string[] tags = { "HealerImg", "MageImg", "TankImg", "AssassinImg", "EnemyImg" };
        Image[] allImages = GetComponentsInChildren<Image>(true);

        foreach (Image img in allImages)
        {
            foreach (string tag in tags)
            {
                if (img.CompareTag(tag))
                {
                    img.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    private void OnIconClicked()
    {
        if (linkedFighter?.Stats?.IsAlive == true)
        {
            CombatManager.Instance.OnFighterIconClicked(linkedFighter);
        }
    }

    public void UpdateIconUI()
    {
        bool isAlive = linkedFighter.Stats.IsAlive;

        if (activeImage != null)
        {
            activeImage.color = isAlive ? Color.white : deadColor;
        }

        if (iconImage != null)
        {
            iconImage.color = isAlive ? aliveColor : deadColor;
        }

        button.interactable = isAlive;
    }

    public void SetActiveTurn(bool active)
    {
        if (activeImage != null)
        {
            activeImage.color = active ? new Color(1f, 1f, 0.5f, 1f) : Color.white;
        }
    }

    public void SetClickable(bool clickable)
    {
        button.interactable = clickable && linkedFighter?.Stats?.IsAlive == true;
    }
}
