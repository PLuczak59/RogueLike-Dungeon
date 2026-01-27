using UnityEngine;

public class RoomInstance
{
    public RoomData data;
    public bool isVisited;

    public RoomInstance(RoomData data){
        this.data = data;
        isVisited = false;
    }
}
