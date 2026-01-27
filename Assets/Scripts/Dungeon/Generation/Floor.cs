using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Floor
{
	public int floorIndex;
	public List<RoomData> availableRooms;

	private List<RoomInstance> roomInstances = new();

	public void Init(){
		roomInstances.Clear();
		foreach(var room in availableRooms){
			roomInstances.Add(new RoomInstance(room));
		}
	}

	public bool HasUnvisitedRooms(){
		return roomInstances.Exists(r => !r.isVisited);
	}

	public RoomData GetRandomUnvisitedRoom(){
		var unvisited = roomInstances.FindAll(r => !r.isVisited);

		if(unvisited.Count == 0){
			return null;
		}

		var selected = unvisited[UnityEngine.Random.Range(0, unvisited.Count)];
		selected.isVisited = true;
		return selected.data;
	}
}
