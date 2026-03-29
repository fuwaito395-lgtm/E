using UnityEngine;

public abstract class 武器效果Base : ScriptableObject
{
    public string 效果Id;
    public string 说明;

    public virtual 武器效果运行时 CreateRuntime(武器实例 owner)
    {
        return new 武器效果运行时(this, owner);
    }
}

public class 武器效果运行时
{
    public 武器效果Base 蓝图;
    public 武器实例 owner;

    public 武器效果运行时(武器效果Base so, 武器实例 owner)
    {
        蓝图 = so;
        this.owner = owner;
    }

    public virtual void OnAttackStart(怪物实例 targetObject, 员工perfer owner) { }

    public virtual float ModifyDamage(float damage, 怪物实例 targetObject, 员工perfer owner)
    {
        return damage;
    }

    public virtual void OnHitTarget(怪物实例 targetl, float finalDamage, bool hitFriend,  员工perfer owner) 
    {
        
    }

    public virtual void OnAttackEnd(int hitCount, 怪物实例 targetl, 员工perfer owner) { }
}