using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Floor
{
	public int floorIndex;
	public List<RoomData> availableRooms;

	private List<RoomInstance> _roomInstances = new();
	public List<RoomInstance> roomInstances => _roomInstances;

	public void Init(){
		_roomInstances.Clear();
		Debug.Log($"[Floor] Initialisation de l'étage {floorIndex} avec {availableRooms.Count} salles disponibles:");
		foreach(var room in availableRooms){
			_roomInstances.Add(new RoomInstance(room));
			Debug.Log($"  - {room.roomName} (Type: {room.type})");
		}
	}

	public bool HasUnvisitedRooms(){
		return _roomInstances.Exists(r => !r.isVisited);
	}

	public RoomData GetRandomUnvisitedRoom(){
		var unvisited = _roomInstances.FindAll(r => !r.isVisited);

		if(unvisited.Count == 0){
			Debug.Log("[Floor] Aucune salle non visitée disponible");
			return null;
		}

		var selected = unvisited[UnityEngine.Random.Range(0, unvisited.Count)];
		selected.isVisited = true;
		return selected.data;
	}
}
