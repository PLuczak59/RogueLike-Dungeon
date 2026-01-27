using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Room")]
public class RoomData : ScriptableObject
{
	public string roomName;

	[TextArea(3, 6)]
	public string description;

	public RoomType type;
}
