
using System.Diagnostics;

public class 流血 : BuffBase
{
    public 流血() { Name = "流血"; }
    public override void UpdateLogic(员工perfer renowner, 怪物实例 lowner, float deltaTime)
    {
        base.UpdateLogic(renowner, null, deltaTime);// 先执行父类的逻辑
        if (timer >= 10.0f) // 只有超过10秒才触发逻辑
        {
            Layers = 0;
            timer = 0; // 重置计时器
        }
    }
    public override void OnOwnerAttack(员工perfer renowner, 怪物实例 lowner)
    {
        Debug.Write("OnOwnerAttack");
        base.OnOwnerAttack(renowner, lowner);
        if (renowner != null)
        {
            renowner.calfinaldamega(Layers, class_damage.damegatype.physic, renowner, null, null);// 伤害等同于层数
        }
        if (lowner != null)
        {
            lowner.calfinaldamega(Layers, class_damage.damegatype.physic, null, lowner, null);
        }
        Layers /= 2;
        
    }
}
