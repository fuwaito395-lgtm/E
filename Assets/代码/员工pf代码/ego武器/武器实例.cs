using System.Collections.Generic;
using UnityEngine;

public class 武器实例
{
    public 武器SO 数据;
    public 员工perfer 拥有者;

    public List<武器攻击实例> 攻击运行时列表 = new List<武器攻击实例>();
    private Dictionary<AttackSO, 武器攻击实例> 攻击映射 = new Dictionary<AttackSO, 武器攻击实例>();

    public 武器实例(武器SO so, 员工perfer owner)
    {
        数据 = so;
        拥有者 = owner;

        if (数据 != null && 数据.攻击列表 != null)
        {
            foreach (var cfg in 数据.攻击列表)
            {
                if (cfg == null || cfg.攻击 == null)
                    continue;

                var rt = new 武器攻击实例(this, cfg);
                攻击运行时列表.Add(rt);

                if (!攻击映射.ContainsKey(cfg.攻击))
                    攻击映射.Add(cfg.攻击, rt);
            }
        }
    }

    public 武器攻击实例 获取攻击运行时(AttackSO so)
    {
        if (so == null) return null;

        if (攻击映射.TryGetValue(so, out var rt))
            return rt;

        Debug.LogWarning($"[武器实例.获取攻击运行时] 找不到攻击：{so.attackId}，请确认它是否已经配置进同一把武器的攻击列表中");
        return null;
    }

    public 武器攻击实例 选择攻击()
    {
        if (攻击运行时列表 == null || 攻击运行时列表.Count == 0)
            return null;

        int total = 0;
        for (int i = 0; i < 攻击运行时列表.Count; i++)
        {
            var atk = 攻击运行时列表[i];
            total += Mathf.Max(0, atk.蓝图 != null ? atk.蓝图.prop : 0);
        }

        if (total <= 0)
            return 攻击运行时列表[0];

        int r = Random.Range(0, total);
        for (int i = 0; i < 攻击运行时列表.Count; i++)
        {
            var atk = 攻击运行时列表[i];
            int p = Mathf.Max(0, atk.蓝图 != null ? atk.蓝图.prop : 0);
            if (r < p) return atk;
            r -= p;
        }

        return 攻击运行时列表[0];
    }
}