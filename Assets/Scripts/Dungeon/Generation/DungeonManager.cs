using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
	public List<Floor> floors;
	public RoomUI roomUI;
	public GameSceneManager gameSceneManager;

	public int currentFloorIndex { get; private set; } = 0;
	public Floor currentFloor { get; private set; }

	private void Start(){
		Debug.LogError("Floor count = " + floors.Count);
		EnterFloor(0);
	}

	private void EnterFloor(int index){
		currentFloor = floors[index];
		currentFloorIndex = index;

		Debug.Log($"[DungeonManager] Entrée dans l'étage {index}");
		currentFloor.Init();
		EnterRandomRoom();
	}

	public void EnterRandomRoom(){

		 if (gameSceneManager == null)
    {
        Debug.LogError("gameSceneManager est NULL !");
        return;
    }

    if (roomUI == null)
    {
        Debug.LogError("roomUI est NULL !");
        return;
    }

    if (currentFloor == null)
    {
        Debug.LogError("currentFloor est NULL !");
        return;
    }

		RoomData room = currentFloor.GetRandomUnvisitedRoom();

        if (room == null)
        {
            GoToNextFloor();
            return;
        }

        Debug.Log($"[DungeonManager] Entrée dans la salle: {room.roomName} (Type: {room.type})");
        gameSceneManager.ActivateSceneForRoomType(room.type);
        roomUI.ShowRoom(room, this);		
	}

	public void ContinueExploration()
    {
		Debug.Log("[DungeonManager] Continuation de l'exploration demandée par le joueur");
        if (currentFloor.HasUnvisitedRooms())
            EnterRandomRoom();
        else
            GoToNextFloor();
    }

	public void GoToNextFloor(){
		int next = currentFloorIndex + 1;
		if(next >= floors.Count){
			Debug.Log("Dungeon termine !");
			return;
		}

		EnterFloor(next);
	}
}
