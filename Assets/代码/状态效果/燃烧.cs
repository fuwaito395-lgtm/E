
public class 燃烧 : BuffBase
{
    public 燃烧() { Name = "燃烧"; }
    public override void UpdateLogic(员工perfer renowner, 怪物实例 lowner, float deltaTime)
    {
        base.UpdateLogic(renowner,null, deltaTime);// 先执行父类的逻辑
        if (timer >= 5.0f) // 只有超过5秒才触发逻辑
        {
            if(renowner!=null)
            {
                renowner.calfinaldamega(Layers, class_damage.damegatype.physic, renowner, null, null);// 伤害等同于层数
            }
            if(lowner!=null)
            {
                lowner.calfinaldamega(Layers, class_damage.damegatype.physic, null, lowner, null);
            }
            Layers /= 2;
            timer = 0; // 重置计时器
        }
    }
}
