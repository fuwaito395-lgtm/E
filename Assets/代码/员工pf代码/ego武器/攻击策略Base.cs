using System.Collections.Generic;
using UnityEngine;

public abstract class 攻击策略Base : ScriptableObject
{
    public virtual bool CanStart(员工setatt 员工, 怪物实例 mainTarget, AttackSO attack)
    {
        return 员工 != null
            && mainTarget != null
            && mainTarget.关联对象 != null
            && attack != null;
    }

    public virtual bool IsTargetInRange(员工setatt 员工, 怪物实例 target, AttackSO attack)
    {
        if (员工 == null || target == null || target.关联对象 == null || attack == null)
            return false;

        float dx = Mathf.Abs(员工.transform.position.x - target.关联对象.transform.position.x);
        return dx <= attack.beforerange;
    }

    public virtual List<怪物实例> CollectTargets(员工setatt 员工, 怪物实例 mainTarget, AttackSO attack)
    {
        return new List<怪物实例> { mainTarget };
    }
}