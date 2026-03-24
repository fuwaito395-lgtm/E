using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MapSystem;
using static 人数据列表;

public class 更新出逃L的update : MonoBehaviour
{
    public static 更新出逃L的update Instance;

    public List<怪物实例> allMonsters = new List<怪物实例>();
    public Dictionary<int, List<怪物实例>> lplace = new Dictionary<int, List<怪物实例>>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float dt =( Time.deltaTime*tct.Instance.timec);
        for (int i = 0; i < allMonsters.Count; i++)
        {
            var m = allMonsters[i];
            if (m != null) m.UpdateTick(dt);
        }
    }
    public void Spawn怪物(怪物实例 l)
    {
        allMonsters.Add(l);
    }
    public void removel(怪物实例 l)
    {
        allMonsters.Remove(l);
    }

}