using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public 员工perfer pf;
    public Dictionary<string, BuffBase> activeBuffs = new Dictionary<string, BuffBase>();
    private List<string> keysToRemove = new List<string>();

    void Update()
    {
        keysToRemove.Clear();
        float dt = Time.deltaTime;

       
        foreach (var kvp in activeBuffs) // 处理所有buff的时间
        {
            kvp.Value.UpdateLogic(pf, dt);
            if (kvp.Value.Layers <= 0) keysToRemove.Add(kvp.Key);
        }

        foreach (var key in keysToRemove)
        {
            activeBuffs[key].OnRemove();
            activeBuffs.Remove(key);
        }
    }

    
    public void NotifyAttack()
    {
        if (activeBuffs.TryGetValue("流血", out BuffBase b))
        {
            b.OnOwnerAttack(pf);
        }
    }
    public void AddBuff(BuffBase newBuff)
    {
        if (activeBuffs.ContainsKey(newBuff.Name))
        {
            // 1. 增加层数
            activeBuffs[newBuff.Name].Layers += newBuff.Layers;
            // 2. 统一重置计时器（让老的层数也获得新的寿命）
            activeBuffs[newBuff.Name].ResetTimer();
        }
        else
        {
            activeBuffs.Add(newBuff.Name, newBuff);
            newBuff.OnApply(pf); // 第一次获得时触发
        }
    }

}
