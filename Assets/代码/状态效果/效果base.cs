
public abstract class BuffBase
{
    public string Name;
    public int Layers;
    protected float timer = 0f; // 累积时间

    public virtual void UpdateLogic(员工perfer renowner, 怪物实例 lowner, float deltaTime)
    {
        timer += deltaTime;
    }

    public virtual void OnApply(员工perfer renowner, 怪物实例 lowner) { }
    public virtual void OnOwnerAttack(员工perfer renowner, 怪物实例 lowner) { }
    public virtual void OnRemove() { }
    public void ResetTimer() { timer = 0f; }
}