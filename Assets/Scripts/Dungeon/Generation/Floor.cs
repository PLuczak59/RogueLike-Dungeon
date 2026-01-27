using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Floor
{
	public int floorIndex;
	public List<RoomData> availableRooms;

	private List<RoomData> visitedRooms = new();

	public bool HasUnvisitedRooms(){
		return visitedRooms.Count < availableRooms.Count;
	}

	public RoomData GetRandomUnvisitedRoom(){
		List<RoomData> unvisited = availableRooms.FindAll(r => !visitedRooms.Contains(r));

		if(unvisited.Count == 0){
			return null;
		}

		RoomData selected = unvisited[UnityEngine.Random.Range(0, unvisited.Count)];
		visitedRooms.Add(selected);
		return selected;
	}
}
