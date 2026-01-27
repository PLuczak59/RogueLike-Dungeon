using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
	public List<Floor> floors;
	public RoomUI roomUI;

	private int currentFloorIndex = 0;
	private Floor currentFloor;

	private void Start(){
		EnterFloor(0);
	}

	private void EnterFloor(int index){
		currentFloor = floors[index];
		currentFloorIndex = index;

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
        if (currentFloor.HasUnvisitedRooms())
            EnterRandomRoom();
        else
            GoToNextFloor();
    }

	public void GoToNextFloor(){
		int next = currentFloorIndex + 1;
		if(next >= floors.Count){
			Debug.Log("Dungeon terminé !");
			return;
		}

		EnterFloor(next);
	}
}
