
public class 燃烧 : BuffBase
{
    public 燃烧() { Name = "燃烧"; }
    public override void UpdateLogic(员工perfer renowner, float deltaTime)
    {
        base.UpdateLogic(renowner, deltaTime);
        if (timer >= 1.0f) // 只有超过1秒才触发逻辑
        {
            // 伤害等同于层数
            renowner.calfinaldamega(Layers, class_damage.damegatype.physic, renowner, null, null);
            Layers /= 2;
            timer = 0; // 重置计时器
        }
    }
}
