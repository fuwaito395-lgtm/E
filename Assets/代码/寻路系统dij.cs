using System.Collections.Generic;
using UnityEngine;
using static MapSystem;

public class 寻路系统dij : MonoBehaviour
{
    public MapSystem mapSystem;
    public static 寻路系统dij intance;

    [System.Serializable]
    public class pathResult
    {
        public List<Room> lui = new List<Room>();
        public float totalDistance;
        public bool iswork;
        public int lastroomid;
    }
    private void Awake()
    {
        intance = this;
    }
    public pathResult 寻路(int startId, int endId)
    {
        var rooms = mapSystem.Rooms;

        Dictionary<int, float> dist = new Dictionary<int, float>();
        Dictionary<int, int> prev = new Dictionary<int, int>();
        HashSet<int> visited = new HashSet<int>();

        foreach (var r in rooms)
        {
            dist[r.RoomId] = float.MaxValue;
            prev[r.RoomId] = -1;
        }

        dist[startId] = 0;

        while (true)
        {
            int cur = -1;
            float best = float.MaxValue;

            foreach (var kv in dist)
            {
                if (!visited.Contains(kv.Key) && kv.Value < best)
                {
                    best = kv.Value;
                    cur = kv.Key;
                }
            }

            if (cur == -1 || cur == endId)
                break;

            visited.Add(cur);

            var curRoom = rooms.Find(r => r.RoomId == cur);

            foreach (var conn in curRoom.Connectroom)
            {
                var next = conn.ConnectedRoom.RoomId;
                if (visited.Contains(next)) continue;

                
                float cost = curRoom.Right - curRoom.Left;

                float nd = dist[cur] + cost;
                if (nd < dist[next])
                {
                    dist[next] = nd;
                    prev[next] = cur;
                }
            }
        }

        pathResult res = new pathResult();
        int step = endId;
        
        if (prev[step] == -1 && step != startId)
        {
            Debug.Log("不可达");
            return res;
        }
        
        while (step != -1)
        {
            res.lui.Insert(0, rooms.Find(r => r.RoomId == step));
            step = prev[step];
            
        }
        res.lastroomid = startId;
        res.totalDistance = dist[endId];
        return res;
    }
}
