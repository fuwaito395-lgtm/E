using System.Collections.Generic;
using UnityEngine;

public class 武器实例
{
    public 武器SO 数据;
    public 员工perfer 拥有者;
    public List<武器效果运行时> 效果运行时列表 = new List<武器效果运行时>();

    public 武器实例(武器SO so, 员工perfer owner)
    {
        数据 = so;
        拥有者 = owner;

        if (数据 != null && 数据.默认效果列表 != null)
        {
            foreach (var e in 数据.默认效果列表)
            {
                if (e != null)
                {
                    效果运行时列表.Add(e.CreateRuntime(this));
                }
            }
        }
    }

    public void OnAttackStart()
    {
        foreach (var e in 效果运行时列表)
        {
            e.OnAttackStart();
        }
    }

    public float 计算最终伤害(float baseDamage)
    {
        float dmg = baseDamage;
        foreach (var e in 效果运行时列表)
        {
            dmg = e.ModifyDamage(dmg);
        }
        return dmg;
    }

    public void 通知命中(GameObject targetObject, float finalDamage, bool hitFriend)
    {
        foreach (var e in 效果运行时列表)
        {
            e.OnHitTarget(targetObject, finalDamage, hitFriend);
        }
    }

    public void OnAttackEnd(int hitCount)
    {
        foreach (var e in 效果运行时列表)
        {
            e.OnAttackEnd(hitCount);
        }
    }
}