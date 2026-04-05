using UnityEngine;

public class 员工setwork : MonoBehaviour
{
    [HideInInspector] public 员工移动 changemove;
    [HideInInspector] private 员工perfer Datapfwork; // 单次工作计时
    [HideInInspector] public float worktime = 0f;
    [HideInInspector] int 尝试剩余 = -1; // 当前尝试还剩余多少次（在开始工作时初始化）
    [HideInInspector] public int 工作差中良优;
    public int totaltry = 0;  // 当前工作周期的总尝试次数
    public int totalsecc = 0; // 成功计数（本周期）
    public float returntotalwork = 0.6f;
    [HideInInspector] public 员工set状态 resetworkroom;

    // 记录是否已对怪物调用过“工作开始钩子”（避免重复调用）
    private bool 已调用工作开始 = false;

    private void Awake()
    {
        Datapfwork = GetComponent<员工perfer>();
    }

    public void workupdate(收容所记录 a, float workspeed)
    {
        if (a == null || a.怪物数据实例 == null || a.怪物数据实例.数据 == null)
        {
            claerandreturn();
            return;
        }

        if (尝试剩余 == -1)
        {
            int readnumber = a.怪物数据实例.数据.totalwork;
            totaltry = readnumber;
            尝试剩余 = totaltry;
            totalsecc = 0;
            worktime = 0f;
            已调用工作开始 = false;
        }

        float dt = Time.deltaTime * (tct.Instance != null ? tct.Instance.timec : 1f);// 把帧时间传给怪物实例，让其内部机制tick
        a.怪物数据实例.Tick(dt,Datapfwork, a, this);//双层计时

        
        if (!已调用工作开始)// 工作开始钩子只调用一次
        {
            a._倒计时.endupdate();
            Datapfwork.sr.flipX = false;
            a.怪物数据实例.WorkStart(Datapfwork.Data.nextwork, Datapfwork, a, this);
            已调用工作开始 = true;
        }

        if (尝试剩余 <= 0)
        {
            doneandreturn(a);
            return;
        }

        if (worktime <= 0f)
        {
            
            float successrate = a.怪物数据实例.计算单次成功率(Datapfwork.Data.nextwork, Datapfwork, a, this);
            float 成功阈 = successrate / 100f;
            bool 本次成功 = (Random.value < 成功阈);

            if (本次成功)
            {
                totalsecc++;
                a.nowwork.text = totalsecc.ToString();
            }
            else
            {
                int damagevalue = Random.Range(
                    a.怪物数据实例.数据.mindamega,
                    a.怪物数据实例.数据.maxdamega + 1);
                getdamage(damagevalue, Datapfwork, a,this);
            }

            a.怪物数据实例.oneWorkFinish(Datapfwork.Data.nextwork,本次成功, Datapfwork, a, this);

            尝试剩余 -= 1;
            
            worktime = a.怪物数据实例.数据.baseworksp; // 单次基础时间
        }

        worktime -= dt;

        if (尝试剩余 <= 0)
        {
            doneandreturn(a);
        }
    }
    private void doneandreturn(收容所记录 a)
    {
        if (totalsecc <= a.怪物数据实例.数据.工作结果[0])
        {
            工作差中良优 = 0;
            Debug.Log($"员工 {gameObject.name}：总体工作,差");
        }
        else if (totalsecc <= a.怪物数据实例.数据.工作结果[1])
        {
            工作差中良优 = 1;
            Debug.Log($"员工 {gameObject.name}：总体工作,中");
        }
        else if (totalsecc <= a.怪物数据实例.数据.工作结果[2])
        {
            工作差中良优 = 2;
            Debug.Log($"员工 {gameObject.name}：总体工作,良");
        }
        else
        {
            工作差中良优 = 3;
            Debug.Log($"员工 {gameObject.name}：总体工作,优");
        }
        a.怪物数据实例.数据.ds[this.Datapfwork.Data.seworktype-1] += totalsecc;
        a.怪物数据实例.lastworkfinish(Datapfwork,a,this);
        claerandreturn();
    }

    private void claerandreturn()
    {
        尝试剩余 = -1;
        totaltry = 0;
        totalsecc = 0;
        worktime = 0f;
        已调用工作开始 = false;
        工作差中良优 = 1;
        resetworkroom.isfindsrs = false;
        if (Datapfwork != null && Datapfwork.Data != null)
        {
            Datapfwork.Data.currentState = 人数据列表.ren.state.move;
            changemove.setpath(
                寻路系统dij.intance.寻路(Datapfwork.Data.CurrentRoomId, Datapfwork.Data.lastroomid).lui,
                Datapfwork.Data.lastroomid
            );
        }
    }

    private void getdamage(int value,  员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        Datapfwork.calfinaldamega(value, nowL.怪物数据实例.数据.damegatyp,员工,nowL.怪物数据实例,work);
    }
}