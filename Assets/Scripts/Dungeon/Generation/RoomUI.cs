using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI roomTypeText;
    
    public GameObject GUIView;
    public Button continueButton;
    public Button nextFloorButton;

    public void ShowRoom(RoomData room, DungeonManager manager)
    {
        // S'assurer que l'UI est visible
        gameObject.SetActive(true);
        
        roomNameText.text = room.roomName;
        roomTypeText.text = room.type.ToString();
        descriptionText.text = room.description;

        continueButton.onClick.RemoveAllListeners();
        nextFloorButton.onClick.RemoveAllListeners();

        continueButton.onClick.AddListener(manager.ContinueExploration);
        nextFloorButton.onClick.AddListener(manager.GoToNextFloor);
    }
    
    /// <summary>
    /// Affiche l'UI avec un message personnalisé et configure les boutons
    /// </summary>
    public void ShowChoiceUI(string title, string description, DungeonManager manager)
    {
        // S'assurer que l'UI est visible
        gameObject.SetActive(true);
        
        if (GUIView != null)
        {
            GUIView.SetActive(true);
            Debug.Log("[RoomUI] GUIView activé pour ShowChoiceUI");
        }
        else
        {
            Debug.LogWarning("[RoomUI] GUIView est null ! Impossible d'afficher l'interface de choix.");
        }
        
        roomNameText.text = title;
        roomTypeText.text = "Choix";
        descriptionText.text = description;

        continueButton.onClick.RemoveAllListeners();
        nextFloorButton.onClick.RemoveAllListeners();

        // Vérifier s'il y a encore des salles disponibles dans l'étage actuel
        bool hasUnvisitedRooms = manager.currentFloor != null && manager.currentFloor.HasUnvisitedRooms();
        
        // Configurer le bouton Continue
        if (hasUnvisitedRooms)
        {
            continueButton.interactable = true;
            continueButton.onClick.AddListener(() => {
                HideChoiceUI();
                manager.ContinueExploration();
            });
        }
        else
        {
            continueButton.interactable = false;
            Debug.Log("[RoomUI] Bouton Continue désactivé - Plus de salles disponibles dans cet étage");
        }
        
        // Configurer le bouton Next Floor
        nextFloorButton.onClick.AddListener(() => {
            HideChoiceUI();
            manager.GoToNextFloor();
        });
    }
    
    /// <summary>
    /// Cache l'UI de choix
    /// </summary>
    public void HideChoiceUI()
    {
        GUIView.SetActive(false);
    }
}
