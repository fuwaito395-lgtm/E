using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using static 人数据列表;

public class 收容系统 : MonoBehaviour
{
    public static 收容系统 Instance;
    public List<收容所记录> 初始收容所列表;   // Inspector 填好的收容所列表
    public List<L基本框架> 怪物数据库;       // 可用的怪物定义ScriptableObject）
    [HideInInspector] public Transform 怪物父物体;            // 场景下用于存放怪物视图的空物体
    [HideInInspector] public bool 初始化时生成视觉 = false;    // 是否在Start时Instantiate视图
    [HideInInspector] public Sprite[] inharm=new Sprite[5]; 


    //房间Id ->收容单元
    [HideInInspector] private Dictionary<int, 收容所记录> 单元字典 = new Dictionary<int, 收容所记录>();
    [HideInInspector] private Dictionary<int, L基本框架> 数据库按Id = new Dictionary<int, L基本框架>();
    [HideInInspector] private Dictionary<int, 收容所记录> 怪物Id到单元 = new Dictionary<int, 收容所记录>();
    

    private void Awake()
    {
        Instance = this;
        单元字典.Clear(); 数据库按Id.Clear();
        foreach (var m in 怪物数据库)
        {
            if (m != null)
            {
                数据库按Id[m.怪物Id] = m;
            }

        }
        初始化收容所();
    }
   


    private void 初始化收容所()
    {
        单元字典.Clear();
        // 重新确保数据库索引（可选）
        数据库按Id.Clear();
        foreach (var m in 怪物数据库) if (m != null) 数据库按Id[m.怪物Id] = m;
        
        for (int i = 0; i < 初始收容所列表.Count; i++)
        {
            var rec = 初始收容所列表[i];
            单元字典[rec.房间Id] = rec;
            
            if (rec.怪物Id != -1)
            {
                
                if (数据库按Id.TryGetValue(rec.怪物Id, out var so))
                {
                    rec.怪物数据实例 = new 怪物实例(so,rec);
                    if (rec.怪物数据实例 != null)
                    {
                        怪物Id到单元[rec.怪物数据实例.数据.怪物Id] = rec;
                    }
                    rec.怪物数据实例.所在收容所 = rec;
                    
                    if (初始化时生成视觉 && so.视图Prefab != null)
                    {
                        rec.怪物数据实例.生成怪物视图(rec, so);
                    }
                }
                else
                {
                    Debug.LogWarning($"初始化收容所：数据库无怪物Id {rec.怪物Id}");
                    rec.怪物数据实例 = null;
                    
                }
            }
            else
            {
                rec.怪物数据实例 = null;
                
            }
            var _maxwork = MapSystem.Instance.GetRoom(rec.房间Id).ob.transform.Find("maxwork");
            rec.maxwork = _maxwork.GetComponent<TextMeshPro>();
            rec.maxwork.text = rec.怪物数据实例.数据.totalwork.ToString();

            var _nowwork = MapSystem.Instance.GetRoom(rec.房间Id).ob.transform.Find("nowwork");
            rec.nowwork = _nowwork.GetComponent<TextMeshPro>();
            rec.nowwork.text = "0";

            var _harm= MapSystem.Instance.GetRoom(rec.房间Id).ob.transform.Find("harm");
            rec.harmsp = _harm.GetComponent<SpriteRenderer>();
            if (rec.怪物数据实例.数据.harm == 2) rec.harmsp.sprite = inharm[1];
            else if (rec.怪物数据实例.数据.harm == 3) rec.harmsp.sprite = inharm[2];
            else if (rec.怪物数据实例.数据.harm == 4) rec.harmsp.sprite = inharm[3];
            else if (rec.怪物数据实例.数据.harm == 5) rec.harmsp.sprite = inharm[4];
            
            var _nowgoout = MapSystem.Instance.GetRoom(rec.房间Id).ob.transform.Find("nowgoout");
            rec.nowgooutindex = _nowgoout.GetComponent<TextMeshPro>();
            rec.nowgooutindex.text = rec.怪物数据实例.数据.出逃max.ToString();
            if (rec.怪物数据实例.数据.出逃max == -1) rec.nowgooutindex.text = "N";
        }
    }

    public void 条满()
    {
        
        foreach(var i in 初始收容所列表)
        {
           i.怪物数据实例.出逃计数(1);
        }
    }
    
    
    // 通过房间Id 获取单元
    
    public 收容所记录 获取单元(int 房间Id)
    {
        单元字典.TryGetValue(房间Id, out var u);
        return u;
    }
    public 怪物实例 GetInstanceByRoom(int roomId)
    {
        var rec = 获取单元(roomId);
        return rec != null ? rec.怪物数据实例 : null;
    }

    public List<怪物实例> GetAllInstances()
    {
        var list = new List<怪物实例>();
        foreach (var kv in 单元字典)
        {
            if (kv.Value != null && kv.Value.怪物数据实例 != null)
            {
                list.Add(kv.Value.怪物数据实例);
            }
        }
        return list;
    }
    public int getroomidbylid(int id)
    {
        return 怪物Id到单元[id].房间Id;
    }
    
    public int GetRoomIdByInstance(怪物实例 inst)
    {
        if (inst == null) return -1;
        foreach (var kv in 单元字典)
        {
            var rec = kv.Value;
            if (rec != null && rec.怪物数据实例 == inst)
                return kv.Key; 
        }
        return -1;
    }
    public List<人数据列表.ren> GetEmployeesInRoom(int roomId)
    {
        var list = new List<人数据列表.ren>();
        if (人数据列表.intance == null || 人数据列表.intance.yuanggong == null) return list;

        foreach (var r in 人数据列表.intance.yuanggong)
        {
            if (r == null) continue;
            if (r.CurrentRoomId == roomId)
            {
                list.Add(r);
            }
        }
        return list;
    }
}
