using System.Collections;
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
        gameObject.SetActive(true);
        
        roomNameText.text = room.roomName;
        roomTypeText.text = room.type.ToString();
        descriptionText.text = room.description;

        continueButton.onClick.RemoveAllListeners();
        nextFloorButton.onClick.RemoveAllListeners();

        continueButton.onClick.AddListener(manager.ContinueExploration);
        nextFloorButton.onClick.AddListener(manager.GoToNextFloor);
    }
    
    public IEnumerator ShowChoiceUI(string title, string description, DungeonManager manager)
    {
        Debug.Log($"[RoomUI] ShowChoiceUI called with title: '{title}'");
        Debug.Log($"[RoomUI] gameObject.activeInHierarchy: {gameObject.activeInHierarchy}");
        Debug.Log($"[RoomUI] GUIView is null: {GUIView == null}");
        
        Debug.Log($"[RoomUI] ShowChoiceUI called with title: '{title}'");
        Debug.Log($"[RoomUI] gameObject.activeInHierarchy: {gameObject.activeInHierarchy}");
        Debug.Log($"[RoomUI] GUIView is null: {GUIView == null}");
        
        gameObject.SetActive(true);
        
        if (GUIView != null)
        {
            Debug.Log($"[RoomUI] GUIView.activeInHierarchy before: {GUIView.activeInHierarchy}");
            Debug.Log($"[RoomUI] GUIView.activeInHierarchy before: {GUIView.activeInHierarchy}");
            GUIView.SetActive(true);
        }
        else
        {
            Debug.LogError("[RoomUI] GUIView est null ! Impossible d'afficher l'interface de choix.");
            yield break;
        }
        
        roomNameText.text = title;
        roomTypeText.text = "Choix";
        descriptionText.text = description;

        continueButton.onClick.RemoveAllListeners();
        nextFloorButton.onClick.RemoveAllListeners();

        bool hasUnvisitedRooms = false;
        if (manager.currentFloor != null)
        {
            int unvisitedCount = 0;
            foreach (var room in manager.currentFloor.RoomInstances)
            {
                if (!room.isVisited) unvisitedCount++;
            }
            
            if (title == "Victoire !")
            {
                hasUnvisitedRooms = unvisitedCount > 1;
                Debug.Log($"[RoomUI] Post-combat: {unvisitedCount} salles non visitées, hasUnvisitedRooms: {hasUnvisitedRooms}");
            }
            else
            {
                hasUnvisitedRooms = unvisitedCount > 0;
                Debug.Log($"[RoomUI] Salle normale: {unvisitedCount} salles non visitées, hasUnvisitedRooms: {hasUnvisitedRooms}");
            }
        }
        
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
        
        nextFloorButton.onClick.AddListener(() => {
            HideChoiceUI();
            manager.GoToNextFloor();
        });
    
    }
    public void HideChoiceUI()
    {
        GUIView.SetActive(false);
    }
}
