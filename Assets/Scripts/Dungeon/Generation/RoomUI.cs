using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI descriptionText;

    public Button continueButton;
    public Button nextFloorButton;

    public void ShowRoom(RoomData room, DungeonManager manager)
    {
        roomNameText.text = room.roomName;
        descriptionText.text = room.description;

        continueButton.onClick.RemoveAllListeners();
        nextFloorButton.onClick.RemoveAllListeners();

        continueButton.onClick.AddListener(manager.ContinueExploration);
        nextFloorButton.onClick.AddListener(manager.GoToNextFloor);
    }
}
