using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
	public List<Floor> floors;
	public RoomUI roomUI;

	private int currentFloorIndex = 0;
	private Floor currentFloor;

	private void Start(){
		Debug.Log("Floor count = " + floors.Count);
		EnterFloor(0);
	}

	private void EnterFloor(int index){
		currentFloor = floors[index];
		currentFloorIndex = index;

		currentFloor.Init();
		EnterRandomRoom();
	}

	public void EnterRandomRoom(){
		RoomData room = currentFloor.GetRandomUnvisitedRoom();

		if(room == null){
			GoToNextFloor();
			return;
		}

		roomUI.ShowRoom(room, this);
	}

	public void ContinueExploration()
    {
		Debug.Log("ContinueExploration called");
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
