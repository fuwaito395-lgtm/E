using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "员工/攻击策略/默认")]
public class 攻击策略Base_默认 : 攻击策略Base
{
    public override bool IsTargetInRange(员工setatt 员工, 怪物实例 target, AttackSO attack)
    {
        if (员工 == null || target == null || target.关联对象 == null || attack == null)
            return false;

        float dx = Mathf.Abs(员工.transform.position.x - target.关联对象.transform.position.x);
        return dx <= attack.beforerange;
    }

    public override List<怪物实例> CollectTargets(员工setatt 员工, 怪物实例 mainTarget, AttackSO attack)
    {
        return new List<怪物实例> { mainTarget };
    }
}