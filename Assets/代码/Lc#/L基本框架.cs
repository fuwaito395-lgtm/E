using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static class_damage;
using static MapSystem;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static 人数据列表;

[CreateAssetMenu(menuName = "怪物数据")]
public class L基本框架 : ScriptableObject//静态数据
{
    public int 怪物Id;
    public int harm;
    public string 显示名;
    
    public int totalwork;
    public float[] 暴怒 = new float[5];
    public float[] 纵欲 = new float[5];
    public float[] 怠惰 = new float[5];
    public float[] 贪婪 = new float[5];
    public float[] 空虚 = new float[5];
    public float[] 傲慢 = new float[5];
    public float[] 嫉妒 = new float[5];
    public int[] 工作结果 = new int[3];
    public class_damage.damegatype damegatyp;
    public float baseworksp;
    public int mindamega;
    public int maxdamega;
    public 工作机制SO[] 默认机制列表;
    public int 出逃max;
    public int maxlhp;
    public AttackSO[] attacks;
    public 怪物漫游Base[] 漫游;
    public float intialmovespeedl;
    public int maxprop;
    public GameObject 视图Prefab; // 仅视觉用
    public Sprite beforegoout = null;
    public Sprite aftergoout=null;
    public Sprite[] extrasp;
}

[System.Serializable]
public class attacktpye
{
    public enum AttackKind { Melee, Ranged } // 可扩展
    public AttackKind attackKind = AttackKind.Melee;
    public float attackRange = 1.2f; // 世界单位
    public float attackCooldown = 1.2f; // 攻击间隔（秒）
    public int attackDamage = 10; // 数值
    public int movespeed = 0;
    public Sprite attackBeforeSprite; // 攻击前定格图
    public Sprite attackAfterSprite; // 攻击后定格图
}
[System.Serializable]
public class 怪物实例
{
    public L基本框架 数据;
    public 收容所记录 所在收容所;
    public float nowhpl;

    public List<工作机制运行时> 机制运行时列表 = new List<工作机制运行时>();
    public List<怪物漫游> 怪物漫游运行时 = new List<怪物漫游>();

    public int 连续失败次数 = 0;
    public int 连续成功次数 = 0;
    public float stoptime = 0;
    public int nowLtype = 0;
    public int now出逃;
    public bool 已出逃 = false;
    public int lnowroom;
    public bool startraom = true;
    public AttackSO nowatt = null;
    public List<Room> lpath = new List<Room>();
    public int pathIndex;
    public bool needchangetarget = true;
    public GameObject 关联对象;
    public 员工perfer targetren = null;
    public float movespeedl;
    public GameObject Tragetrenobj = null;
    public List<员工perfer> roomren = new List<员工perfer>();
    public allatttime allattttimel;
    public GameObject 血条;

    [HideInInspector] public TextMeshPro bloodtext;
    [HideInInspector] public TextMeshPro nametext;
    [HideInInspector] public 血条显示 bloodphycis;

    
    public 房间 当前房间 = null;
    

    public 怪物实例(L基本框架 数据, 收容所记录 rec)
    {
        this.数据 = 数据;
        this.lnowroom = rec != null ? rec.房间Id : 0;
        now出逃 = 数据 != null ? 数据.出逃max : 0;

        if (数据 != null && 数据.默认机制列表 != null)
        {
            foreach (var so in 数据.默认机制列表)
            {
                if (so != null)
                {
                    机制运行时列表.Add(so.CreateRuntime());
                }
            }
        }
    }

    public void 绑定房间(房间 room)
    {
        当前房间 = room;
    }

    public void 解绑房间(房间 room = null)
    {
        if (room != null && 当前房间 != room) return;
        当前房间 = null;
    }

    
    public void 刷新roomren缓存()
    {
        roomren.Clear();
        for (int i = 当前房间.当前员工.Count - 1; i >= 0; i--)
        {
            var emp = 当前房间.当前员工[i];
            if (emp == null)
            {
                当前房间.当前员工.RemoveAt(i);
                continue;
            }

            if (emp.Data != null)
                roomren.Add(emp);
        }
        if(targetren!=null)
        {
            if (targetren == null
                || targetren.Data == null
                || targetren.Data.hp <= 0
                || !IsTargetValid(targetren))
            {
                targetren = null;
                Tragetrenobj = null;
            }
        }
    }
    public virtual void UpdateTick(float dt)
    {
        if (tct.Instance.timec == 0)
        {
            return;
        }
        if (nowhpl <= 0)
        {
            reset();
            return;
        }
        
       
        if (!已出逃)
        {
            return;
        }

        if (stoptime > 0)
        {
            if (stoptime < 0) stoptime = 0;

            if (怪物漫游运行时 != null && nowLtype >= 0 && nowLtype < 怪物漫游运行时.Count && 怪物漫游运行时[nowLtype] != null)
            {
                怪物漫游运行时[nowLtype].caltime(关联对象, this);
            }
            return;
        }
        var roam = 当前漫游;
        if (roam == null)
            return;

        if (!IsTargetValid(targetren))
        {
            targetren = null;
        }
        if (allattttimel == allatttime.windup)
        {
            roam.caliswindup(关联对象, this);
            return;
        }

        if (allattttimel == allatttime.post)
        {
            roam.calispost(关联对象, this);
            needchangetarget = true;
            return;
        }

        if (roomren == null || roomren.Count == 0)
        {
            if(targetren==null)
            {
                targetren= roam.Onfindtargetren(关联对象, this);
                Tragetrenobj = roam.Ongettarrenob(关联对象, this);
            }
            roam.calroomrennull(关联对象, this);
            return;
        }else
        {
            if ((targetren == null || needchangetarget )&& roam.查询目标员工(roomren, this) !=null)
            {
                targetren = roam.查询目标员工(roomren, this);
                Tragetrenobj = roam.Ongettarrenob(关联对象, this);
            }
        }

        if (nowatt == null)
        {
            roam.getnowatt(关联对象, this);
            if (nowatt == null)
            {
                Debug.LogWarning("没有选到任何攻击");
            }
        }
        
        if (allattttimel == allatttime.none)
        {
            roam.calisnone(关联对象, this);
        }
    }

    public 怪物漫游 当前漫游
    {
        get
        {
            if (怪物漫游运行时 == null) return null;
            if (nowLtype < 0 || nowLtype >= 怪物漫游运行时.Count) return null;
            return 怪物漫游运行时[nowLtype];
        }
    }


    public void 生成怪物视图(收容所记录 rec, L基本框架 so)
    {
        if (so == null)
        {
            Debug.LogError("[生成怪物视图] so == null");
            return;
        }

        if (so.视图Prefab == null)
        {
            Debug.LogError($"[生成怪物视图] 怪物 {so.显示名} 的 视图Prefab 未设置");
            return;
        }

        Vector3 pos = Vector3.zero;

        if (MapSystem.Instance != null && rec != null)
        {
            var center = MapSystem.Instance.GetRoomCenterPosition(rec.房间Id);
            pos = new Vector3(center.x + 6f, center.y + 2f, -1f);
        }
        else
        {
            pos = 出逃父物体.instance != null ? 出逃父物体.instance.transform.position : Vector3.zero;
        }

        关联对象 = UnityEngine.Object.Instantiate(so.视图Prefab, pos, Quaternion.identity, 出逃父物体.instance.transform);

        var spr = 关联对象.GetComponent<SpriteRenderer>();

        if (this.数据.beforegoout != null)
        {
            spr.sprite = this.数据.beforegoout;
        }

        movespeedl = so.intialmovespeedl;
    }

    public void 标记出逃()
    {
        if (已出逃) return;
        lnowroom = 收容系统.Instance.getroomidbylid(this.数据.怪物Id);
        已出逃 = true;
        Debug.Log(数据.显示名 + " 标记为出逃");

        foreach (var s in 数据.漫游)
        {
            if (s != null)
            {
                怪物漫游运行时.Add(s.CreateRuntime怪物漫游(this));
            }
        }
        if (this.数据.aftergoout!=null)
        {
            var spr = 关联对象.GetComponent<SpriteRenderer>();
            if (this.数据.aftergoout != null)
            {
                spr.sprite = this.数据.aftergoout;
            }
        }

        已出逃 = true;
        nowhpl = 数据.maxlhp;
        更新出逃L的update.Instance.Spawn怪物(this);

        Vector3 bloodpos = new Vector3(
            关联对象.transform.position.x,
            关联对象.transform.position.y - 5,
            -1
        );
        stoptime += 15f;
        
        血条 = UnityEngine.Object.Instantiate(L插件.instance.blood, bloodpos, Quaternion.identity, 关联对象.transform);

        var blood = 血条.transform.Find("blood");
        bloodtext = blood.GetComponent<TextMeshPro>();
        bloodtext.text = 数据.maxlhp.ToString();

        var gooutname = 血条.transform.Find("Lname");
        nametext=gooutname.GetComponent<TextMeshPro>();
        nametext.text = 数据.显示名;

        bloodphycis = 血条.transform.Find("血条显示").GetComponent<血条显示>();

        当前房间 = MapSystem.Instance.GetRoom(所在收容所.房间Id).ob.GetComponent<房间>();
    }

    public void 出逃计数(int v = 1)
    {
        if (已出逃) return;

        now出逃 -= v;
        Debug.Log($"[实例] {数据.显示名} 出逃点数减少 => {now出逃}");
        this.所在收容所.nowgooutindex.text =now出逃.ToString();
        if (now出逃 == 0)
        {
            标记出逃();
        }
    }

    public void reset()
    {
        this.所在收容所.nowgooutindex.text = this.数据.出逃max.ToString();
        Debug.Log(this.数据.name + "镇压成功");
        now出逃 = 数据.出逃max;
        已出逃 = false;
        UnityEngine.Object.Destroy(this.关联对象);
        生成怪物视图(收容系统.Instance.获取单元(收容系统.Instance.getroomidbylid(this.数据.怪物Id)), this.数据);
        //更新出逃L的update.Instance.Remove怪物(this);
        lpath  = new List<Room>();
        pathIndex = 0;
        更新出逃L的update.Instance.removel(this);
        当前房间.怪物离开(this);
    }

    bool IsTargetValid(员工perfer t)
    {
        if (t == null) return false;
        if (t.Data == null) return false;
        if (t.Data.hp <= 0) return false;

        return true;
    }


    /// <summary>
    /// //工作
    /// </summary>
    public virtual void Tick(float dt,员工perfer 员工, 收容所记录 nowL, 员工setwork work)//计时
    {
        foreach (var rt in 机制运行时列表) rt.Tick(dt,  员工, nowL, work);
    }

    public virtual void WorkStart(int 工作类型, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        foreach (var rt in 机制运行时列表) rt.OnWorkStart( 员工, nowL,  work);//通知每个运行时机制开始
        nowL.nowwork.text = "0";
    }

    public virtual void oneWorkFinish(int 工作类型,  bool 本次成功, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        if (本次成功)
        {
            连续成功次数++;
            连续失败次数 = 0;
        }
        else
        {
            连续失败次数++;
            连续成功次数 = 0;
        }

        // 通知机制
        foreach (var rt in 机制运行时列表) rt.OnoneWorkFinish(本次成功, 员工,  nowL, work);
    }

    public virtual float 计算单次成功率(int 工作类型, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        int 等级 = 取得对应属性等级(工作类型, 员工, nowL, work);
        float 基础百分比 = 取得基础百分比(工作类型, 等级, 员工, nowL, work);

        float p = Mathf.Clamp01(基础百分比 / 100f);

        foreach (var rt in 机制运行时列表)
        {
            p = rt.ModifyChance(p, 员工, nowL, work);
        }

        return p * 100f;
    }

    private int 取得对应属性等级(int 工作类型, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        switch (工作类型)
        {
            case 1: return 员工.Data.勇气;
            case 2: return 员工.Data.谨慎;
            case 3: return 员工.Data.勤勉;
            case 4: return 员工.Data.自律;
            case 5: return 员工.Data.沉静;
            case 6: return 员工.Data.正义;
            case 7: return 员工.Data.宽容;
            default: return 0;
        }
    }

    private float 取得基础百分比(int 工作类型, int 等级, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        if (等级 < 1 || 等级 > 5) return 0f;

        int index = 等级 - 1;
        
        switch (工作类型)
        {
            case 1: return 取数组值(数据.暴怒, index, 员工, nowL, work);
            case 2: return 取数组值(数据.纵欲, index, 员工, nowL, work);
            case 3: return 取数组值(数据.怠惰, index, 员工, nowL, work);
            case 4: return 取数组值(数据.贪婪, index, 员工, nowL, work);
            case 5: return 取数组值(数据.空虚, index, 员工, nowL, work);
            case 6: return 取数组值(数据.傲慢, index, 员工, nowL, work);
            case 7: return 取数组值(数据.嫉妒, index, 员工, nowL, work);
            default: return 0f;
        }
    }

    private float 取数组值(float[] arr, int index, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
    {
        if (arr == null) return 0f;
        if (index < 0 || index >= arr.Length) return 0f;
        float pro = arr[index];
        foreach (var rt in 机制运行时列表) rt.ModifyChance(pro, 员工, nowL, work);

        return pro;
    }
    public virtual void lastworkfinish(员工perfer 员工, 收容所记录 nowL,员工setwork work)
    {
        foreach (var rt in 机制运行时列表) rt.onlastworkfinish(员工, nowL, work);
        
    }

    public void 添加机制(工作机制SO so)// 便捷 API：添加/删除/替换机制（运行时）
    {
        if (so == null) return;
        机制运行时列表.Add(so.CreateRuntime());
    }

    public void 删除机制ById(string 机制Id)
    {
        机制运行时列表.RemoveAll(rt => rt.蓝图 != null && rt.蓝图.name == 机制Id);
    }

    public void 替换机制(string 老Id, 工作机制SO 新蓝图)
    {
        for (int i = 0; i < 机制运行时列表.Count; i++)
        {
            if (机制运行时列表[i].蓝图 != null && 机制运行时列表[i].蓝图.name == 老Id)
            {
                机制运行时列表[i] = 新蓝图.CreateRuntime();
                return;
            }
        }

        添加机制(新蓝图);
    }

    public enum allatttime
    {
        none,
        windup, //前摇
        post,   //后摇
        hit,
        cd
    }

    /// <summary>
    /// //caldamege
    /// </summary>
    public void calfinaldamega(float damegavol, class_damage.damegatype damegatyp, 员工perfer 员工, 怪物实例 nowL, 员工setwork work)
    {

        damegavol = (int)System.Math.Ceiling(damegavol);
        //后续抗性

        this.nowhpl -= (int)damegavol;
        float ratio = (float)this.nowhpl / this.数据.maxlhp;
        bloodphycis.updatephysic(ratio);
        bloodtext.text = nowhpl.ToString();
        伤害图片控制器.instance.Show(damegatyp, (int)damegavol, this.关联对象.transform.position);
    }



}