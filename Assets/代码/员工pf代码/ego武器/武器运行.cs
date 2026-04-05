using System.Collections.Generic;
using UnityEngine;

public class 武器攻击实例
{
    public enum 攻击阶段
    {
        none,
        windup,
        post,
        cd
    }

    public 武器实例 owner;
    public AttackSO 蓝图;
    public 攻击策略Base 策略蓝图;
    public List<武器效果运行时> 效果运行时列表 = new List<武器效果运行时>();

    public 攻击阶段 stage = 攻击阶段.none;
    public float timer = 0f;

    private 怪物实例 锁定目标 = null;

    private static 攻击策略Base_默认 s_defaultStrategy;
    private static 攻击策略Base 默认策略
    {
        get
        {
            if (s_defaultStrategy == null)
                s_defaultStrategy = ScriptableObject.CreateInstance<攻击策略Base_默认>();
            return s_defaultStrategy;
        }
    }

    public bool IsRunning => stage != 攻击阶段.none;
    public bool isRunning => IsRunning;

    public 武器攻击实例(武器实例 owner, 攻击配置 cfg)
    {
        this.owner = owner;
        蓝图 = cfg.攻击;
        策略蓝图 = cfg.策略;

        if (cfg.效果列表 != null)
        {
            foreach (var e in cfg.效果列表)
            {
                if (e != null)
                    效果运行时列表.Add(e.CreateRuntime(owner));
            }
        }
    }

    private 攻击策略Base Get策略()
    {
        return 策略蓝图 != null ? 策略蓝图 : 默认策略;
    }

    public bool CanStart(员工setatt 员工, 怪物实例 target)
    {
        return Get策略().CanStart(员工, target, 蓝图);
    }

    public bool IsTargetInRange(员工setatt 员工, 怪物实例 target)
    {
        return Get策略().IsTargetInRange(员工, target, 蓝图);
    }

    public List<怪物实例> CollectTargets(员工setatt 员工, 怪物实例 mainTarget)
    {
        return Get策略().CollectTargets(员工, mainTarget, 蓝图);
    }

    public void StartAttack(员工setatt 员工, 怪物实例 target, bool skipWindup = false)
    {
        if (蓝图 == null)
            return;

        锁定目标 = target;

        for (int i = 0; i < 效果运行时列表.Count; i++)
        {
            效果运行时列表[i].OnAttackStart(锁定目标, 员工.OwnerData);
        }

        stage = 攻击阶段.windup;
        timer = skipWindup ? 0f : Mathf.Max(0f, 蓝图.windup);
    }

    /// <summary>
    /// 返回值：如果触发了 nextatt，则返回“下一段攻击运行时”，由员工控制器接管。
    /// </summary>
    public 武器攻击实例 Tick(float dt, 员工setatt 员工)
    {
        if (stage == 攻击阶段.none)
            return null;

        timer -= dt;
        if (timer > 0f)
            return null;

        if (stage == 攻击阶段.windup)
        {
            执行命中(员工);
            return null;
        }

        if (stage == 攻击阶段.post)
        {
            // 当前攻击结束时，优先尝试衔接 nextatt
            if (蓝图 != null && 蓝图.nextatt != null)
            {
                var next = owner != null ? owner.获取攻击运行时(蓝图.nextatt) : null;

                if (next != null)
                {
                    // 当前攻击结束，切换到下一段攻击，并且跳过下一段前摇
                    var carryTarget = 锁定目标;
                    stage = 攻击阶段.none;
                    timer = 0f;
                    锁定目标 = null;

                    next.StartAttack(员工, carryTarget, true);
                    return next;
                }

                Debug.LogWarning($"[武器攻击实例.Tick] nextatt = {蓝图.nextatt.attackId}，但它没有配置进同一把武器的攻击列表里");
            }

            stage = 攻击阶段.cd;
            timer = owner != null && owner.数据 != null ? owner.数据.攻击间隔 : 0f;
            return null;
        }

        if (stage == 攻击阶段.cd)
        {
            stage = 攻击阶段.none;
            锁定目标 = null;
            return null;
        }

        return null;
    }

    private void 执行命中(员工setatt 员工)
    {
        int hitCount = 0;

        // 这里不再因为“目标换房间 / 远离”而打断本次攻击
        // 只要锁定目标本体还在，就执行这次攻击
        if (锁定目标 != null && 锁定目标.关联对象 != null && 锁定目标.nowhpl > 0)
        {
            float damage = 蓝图.damage;

            for (int i = 0; i < 效果运行时列表.Count; i++)
            {
                damage = 效果运行时列表[i].ModifyDamage(damage, 锁定目标, 员工.OwnerData);
            }

            锁定目标.calfinaldamega(
                damage,
                蓝图._Damage,
                员工.OwnerData,
                锁定目标,
                null
            );

            for (int i = 0; i < 效果运行时列表.Count; i++)
            {
                效果运行时列表[i].OnHitTarget(锁定目标, damage, false, 员工.OwnerData);
            }

            hitCount = 1;
        }

        for (int i = 0; i < 效果运行时列表.Count; i++)
        {
            效果运行时列表[i].OnAttackEnd(hitCount, 锁定目标, 员工.OwnerData);
        }

        stage = 攻击阶段.post;
        timer = Mathf.Max(0f, 蓝图.post);
    }
}