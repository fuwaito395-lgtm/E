using UnityEngine;

public abstract class 工作机制SO : ScriptableObject
{
    public string 机制Id;
    public string 说明;

    public virtual 工作机制运行时 CreateRuntime()//外部覆盖--去工作机制运行时(工作机制SO so)
    {
        return new 工作机制运行时(this);
    }

    public virtual string Validate() 
    { 
        return null; 
    }
}

public class 工作机制运行时
{
    public 工作机制SO 蓝图;
    public float 运行时计时 = 0f;
    public bool 已触发 = false;
    public float 覆盖系数 = 1f; // 可以被外部调整

    public 工作机制运行时(工作机制SO so)//覆盖
    {
        蓝图 = so;
        覆盖系数 = 1f;
    }

    // 每帧或按时间调用
    public virtual void Tick(float dt, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        运行时计时 += dt;
    }

    // 计算并返回调整后的概率（输入/输出 0..1）
    public virtual float ModifyChance(float 当前概率, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        return Mathf.Clamp01(当前概率 * 覆盖系数);
    }

    public virtual void OnWorkStart(员工perfer 员工, 收容所记录 nowL, 员工setwork work) { }
    public virtual void OnoneWorkFinish(bool success, 员工perfer 员工, 收容所记录 nowL, 员工setwork work) { }
    public virtual void onlastworkfinish(员工perfer 员工, 收容所记录 nowL, 员工setwork work) 
    { 
        
    }
}