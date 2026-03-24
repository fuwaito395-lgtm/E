using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public static MapSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    [System.Serializable]
    public class Room
    {
        public int RoomId;// 房间编号
        
        
        public List<Connection> Connectroom = new List<Connection>(); // 相连的房间


        [HideInInspector] public float Left;    // 走廊左边 X
        [HideInInspector] public float Right;   // 走廊右边 X
        [HideInInspector] public float Y;       // 高度
        [HideInInspector] public float Distance;//距离
        [HideInInspector] public float CenterX;
        public whereelvator elavator;
        public isshel isshelter;
        [HideInInspector] public GameObject ob;

    }
    public enum isshel
    {
        no,
        yes
    }

    public enum whereelvator// 一个走廊只能有一个电梯
    { 
        none,
        left,
        right,
    }

    [System.Serializable]
    public class Connection
    {
        public Room ConnectedRoom;//相连房间
        
    }
    public List<Room> Rooms = new List<Room>();
    public void giveleftright(float leftx, float rightx, float objecty, int num,GameObject _ob)
    {
        if(num>=Rooms.Count)
        {
            return;
        }
        Rooms[num].Left = leftx;
        Rooms[num].Right = rightx;
        Rooms[num].Y = objecty;
        Rooms[num].CenterX = (Rooms[num].Left + Rooms[num].Right) * 0.5f;
        Rooms[num].ob = _ob;
    }
    public Room GetRoom(int id)
    {
        return Rooms.Find(r => r.RoomId == id);
    }
    public Vector3 GetRoomCenterPosition(int id)
    {
        var r = GetRoom(id);
        if (r == null) return Vector3.zero;
        return new Vector3(r.Left, r.Y, 0f);
    }

}
