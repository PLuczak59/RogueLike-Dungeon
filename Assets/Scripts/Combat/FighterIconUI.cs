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
    private Slider healthBar;
    private Image healthBarFill;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnIconClicked);
        }

        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        Transform healBarTransform = transform.Find("HealBar");
        if (healBarTransform != null)
        {
            healthBar = healBarTransform.GetComponent<Slider>();
            if (healthBar == null)
            {
                Transform fillArea = healBarTransform.Find("Fill Area");
                if (fillArea != null)
                {
                    Transform fill = fillArea.Find("Fill");
                    if (fill != null)
                    {
                        healthBarFill = fill.GetComponent<Image>();
                    }
                }
            }
        }
    }

    public void Initialize(Character fighter, Color color)
    {
        linkedFighter = fighter;
        aliveColor = color;
        ActivateCharacterImage();
        InitializeHealthBarValues();
    }

    private void InitializeHealthBarValues()
    {

        if (healthBar != null)
        {
            healthBar.maxValue = linkedFighter.Stats.maxHP;
            healthBar.value = linkedFighter.Stats.currentHP;
        }
        else if (healthBarFill != null)
        {
            healthBarFill.fillAmount = linkedFighter.Stats.HPPercent;
        }
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
        string[] tags = { "HealerImg", "MageImg", "TankImg", "AssassinImg", "EnemyImg", "DeadImg" };
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

        if (isAlive)
        {
            ActivateCharacterImage();
        }
        else
        {
            ActivateDeadImage();
        }

        if (iconImage != null && !isAlive)
        {
            iconImage.color = deadColor;
        }

        UpdateHealthBar();

        button.interactable = isAlive;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = linkedFighter.Stats.currentHP;
        }
        else if (healthBarFill != null)
        {
            healthBarFill.fillAmount = linkedFighter.Stats.HPPercent;
        }
    }

    private void ActivateDeadImage()
    {
        DesactivateAllTaggedImages();

        Image[] allImages = GetComponentsInChildren<Image>(true);
        foreach (Image img in allImages)
        {
            if (img.CompareTag("DeadImg"))
            {
                img.gameObject.SetActive(true);
                activeImage = img;
                break;
            }
        }
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
