using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
	public List<Floor> floors;
	public RoomUI roomUI;
	public GameSceneManager gameSceneManager;

	private int currentFloorIndex = 0;
	private Floor currentFloor;

	private void Start(){
		Debug.LogError("Floor count = " + floors.Count);
		EnterFloor(0);
	}

	private void EnterFloor(int index){
		currentFloor = floors[index];
		currentFloorIndex = index;

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

        gameSceneManager.ActivateSceneForRoomType(room.type);
        roomUI.ShowRoom(room, this);		
	}

	public void ContinueExploration()
    {
		Debug.LogError("ContinueExploration called");
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
