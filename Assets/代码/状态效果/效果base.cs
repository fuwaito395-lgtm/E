
public abstract class BuffBase
{
    public string Name;
    public int Layers;
    protected float timer = 0f; // 累积时间

    public virtual void UpdateLogic(员工perfer renowner, float deltaTime)
    {
        timer += deltaTime;
    }

    public virtual void OnApply(员工perfer renowner) { }
    public virtual void OnOwnerAttack(员工perfer renowner) { }
    public virtual void OnRemove() { }
    public void ResetTimer() { timer = 0f; }
}