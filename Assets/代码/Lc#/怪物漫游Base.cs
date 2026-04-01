using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static MapSystem;
using static UnityEditor.PlayerSettings;
using static 人数据列表;
using static 怪物实例;

public abstract class 怪物漫游Base : ScriptableObject
{
    public string 机制Id;
    public string 说明;

    public virtual 怪物漫游 CreateRuntime怪物漫游(怪物实例 nowL)//外部覆盖--去工作机制运行时(工作机制SO so)
    {
        return new 怪物漫游(this, nowL);
    }

    public virtual string Validate()
    {
        return null;
    }
}

public class 怪物漫游
{
    public 怪物漫游Base 蓝图;
    public 怪物实例 parent;
    public GameObject nowLp;
    public int nowroomid;
    private float targetX; // 移动目标
    private bool ismoving; // 正在移动
    private bool isgoingleft;
    SpriteRenderer spir;

    public 怪物漫游(怪物漫游Base so, 怪物实例 L)//覆盖
    {
        蓝图 = so;
        parent = L;
    }

    /// <summary>
    ///主要函数
    /// </summary>

    public virtual void caltime(GameObject pt, 怪物实例 movedata)
    {
        movedata.stoptime -= (Time.deltaTime * tct.Instance.timec);
        return;
    }

    public virtual void calroomrennull(GameObject pt, 怪物实例 movedata)
    {
        if(movedata == null) return;
        if (movedata.关联对象 == null) return;
        if (movedata.targetren.Data == null) return;
        //Debug.Log("calroomrennull");
        movedata.怪物漫游运行时[movedata.nowLtype].Onmove(movedata.关联对象, movedata, movedata.targetren.Data);
    }
    public virtual void calisnone(GameObject pt, 怪物实例 movedata)
    {
        if (movedata == null) return;
        if (movedata.关联对象 == null) return;
        if (movedata.nowatt == null) return;
        if (movedata.Tragetrenobj == null) return;

        if (movedata.lnowroom != movedata.targetren.Data.CurrentRoomId)
        {
            movedata.needchangetarget = true;
            return;
        }
        
        Vector3 pos = movedata.关联对象.transform.position;
        
        if (movedata.nowatt.typerange == class_damage.attrang.normal)
        {
            movedata.怪物漫游运行时[movedata.nowLtype].normalatt(movedata.关联对象, movedata);
            if ((movedata.nowatt.beforerange > Math.Abs(movedata.关联对象.transform.position.x - movedata.Tragetrenobj.transform.position.x)
                && movedata.lnowroom==movedata.targetren.Data.CurrentRoomId))//在攻击范围
            {

                var spr = movedata.关联对象.GetComponent<SpriteRenderer>();

                spr.sprite = movedata.nowatt.beforeatt;
                movedata.aud.PlayOneShot(movedata.nowatt.beforeaud);
                movedata.stoptime += movedata.nowatt.windup;
                movedata.怪物漫游运行时[movedata.nowLtype].Onwindup();//时机
                movedata.allattttimel = allatttime.windup;
            }
            else
            {
                
                pos.x = Mathf.MoveTowards(pos.x, movedata.Tragetrenobj.transform.position.x, movedata.movespeedl * tct.Instance.timec * Time.deltaTime);//水平移动
                movedata.关联对象.transform.position = pos;
                
            }
        }
    }

    public virtual void caliswindup(GameObject pt, 怪物实例 movedata)
    {
        movedata.allattttimel = allatttime.post;
        movedata.stoptime += movedata.nowatt.hitTime;
        var spr = movedata.关联对象.GetComponent<SpriteRenderer>();

        spr.sprite = movedata.nowatt.afteratt;
        movedata.aud.PlayOneShot(movedata.nowatt.afteraud);
        movedata.怪物漫游运行时[movedata.nowLtype].Onhitrange(movedata.关联对象, movedata);

    }
    
    public virtual void calispost(GameObject pt, 怪物实例 movedata)
    {
        if (movedata == null) return;
        if (movedata.关联对象 == null) return;
        if (movedata.nowatt == null) return;

        var spr = movedata.关联对象.GetComponent<SpriteRenderer>();

        spr.sprite = movedata.数据.aftergoout;
        movedata.stoptime += movedata.nowatt.post;
        if(movedata.nowatt.nextatt!=null)
        {
            movedata.nowatt = movedata.nowatt.nextatt;
            movedata.allattttimel = allatttime.windup;
        }
        else
        {
            movedata.allattttimel = allatttime.none;
            movedata.nowatt = null;
        }
            // 清理攻击状态
        movedata.targetren = null;
        movedata.Tragetrenobj = null;
        movedata.needchangetarget = true;
        movedata.needchangetarget = true;
    }
    public virtual void getnowatt(GameObject pt, 怪物实例 movedata)
    { 

        movedata.nowatt = null;

        int total = 0;
        foreach (var a in movedata.数据.attacks)
        {
            total += Mathf.Max(0, a.prop);
        }

        if (total <= 0)
        {
            return;
        }

        int r = UnityEngine.Random.Range(0, total);

        foreach (var a in movedata.数据.attacks)
        {
            int p = Mathf.Max(0, a.prop);

            if (r < p)
            {
                movedata.nowatt = a;
                return;
            }

            r -= p;
        }
    }
    
    /// <summary>
    /// typeatt函数
    /// </summary>
    public virtual void normalatt(GameObject pt, 怪物实例 movedata)
    {
        
        if (movedata.关联对象.transform.position.x > movedata.Tragetrenobj.transform.position.x)//ren_____L
        {
            var flip = movedata.关联对象.GetComponent<SpriteRenderer>();
            flip.flipX = true;
        }
        else//L_______ren
        {
            var flip = movedata.关联对象.GetComponent<SpriteRenderer>();
            flip.flipX = false;
        }

        
    }

    //buff函数
    public virtual void checkbuffwhenupdate(GameObject pt, 怪物实例 movedata,float dt)
    {
        foreach (var kvp in movedata.activeBuffs) // 处理所有buff的时间
        {
            kvp.Value.UpdateLogic(null, movedata, dt);
            if (kvp.Value.Layers <= 0) movedata.keysToRemove.Add(kvp.Key);
        }
        foreach (var key in movedata.keysToRemove)
        {
            movedata.activeBuffs[key].OnRemove();
            movedata.activeBuffs.Remove(key);
        }
        movedata.keysToRemove.Clear();
    }
    public virtual void checkbuffwhenatt(GameObject pt, 怪物实例 movedata)
    {
        //Debug.Log("check");
        foreach (var kvp in movedata.activeBuffs) // 处理所有buff的时间
        {
            kvp.Value.OnOwnerAttack(null, movedata);
        }
    }



    /// <summary>
    /// 副函数
    /// </summary>

    public virtual void 收到员工进入通知(员工perfer 员工, 房间 room, 怪物实例 movedata)
    {
        movedata.刷新roomren缓存();
    }

    public virtual void 收到员工离开通知(员工perfer 员工, 房间 room,怪物实例 movedata)
    {
        movedata.刷新roomren缓存();
    }

    public virtual void 收到怪物进入通知(怪物实例 monster, 房间 room)
    {

    }

    public virtual void 收到怪物离开通知(怪物实例 monster, 房间 room)
    {

    }

    public virtual 员工perfer 查询目标员工(List<员工perfer> 候选员工, 怪物实例 movedata)
    {
        if (候选员工 == null) return null;

        for (int i = 候选员工.Count - 1; i >= 0; i--)
        {
            var e = 候选员工[i];
            if (e != null && e.Data != null)
                return e;
        }

        return null;
    }

    public virtual GameObject Ongettarrenob(GameObject pt, 怪物实例 movedata)//拿人的图片
    {
        return movedata.targetren.gameObject;
    }

    
    public virtual 员工perfer Onfindtargetren(GameObject pt, 怪物实例 movedata)//随机找在房间里的人
    {

        var a = 员工父物体.instance.allren.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        
        return a;
        
    }

    public virtual void Onhitrange(GameObject pt, 怪物实例 movedata)
    {
        if (movedata.targetren == null) return;

        var ob = 初始化.instance.getobjectfromrenid(movedata.targetren.Data.bianhao);
        if (ob == null)
        {
            movedata.needchangetarget = true;
            return;
        }
        foreach (var r in movedata.roomren)
        {
            ob = 初始化.instance.getobjectfromrenid(r.Data.bianhao);
            if (movedata.nowatt.afterrange > Math.Abs(movedata.关联对象.transform.position.x - ob.transform.position.x) &&
                movedata.targetren.Data.CurrentRoomId == movedata.lnowroom)
            {
                var perfet = ob.GetComponent<员工perfer>();
                perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage,null,null,null);
            }
        }
    }

    public virtual void Onmove(GameObject pt, 怪物实例 movedata, ren targetren)
    {
        //Debug.Log("onmove");

        if (movedata.lpath == null || movedata.lpath.Count == 0)
        {
        //    Debug.Log(" path.Count == 0");
            movedata.lpath = 寻路系统dij.intance.寻路(movedata.lnowroom, targetren.CurrentRoomId).lui;
            movedata.pathIndex = 0;
            firstmove(pt, movedata, spir);
        }

        Room current = movedata.lpath[movedata.pathIndex];

        if (movedata.pathIndex >= movedata.lpath.Count - 1)
        {
        //    Debug.Log("return " + movedata.pathIndex);
            movedata.lpath = null;
            return;
        }

        Room next = movedata.lpath[movedata.pathIndex + 1];
        //Debug.Log("moveto");

        if (spir == null)
        {
            spir = pt.GetComponent<SpriteRenderer>();
        }

        moveto(current, next, movedata.pathIndex, pt, movedata, spir);
    }

    public void 进入新房间(房间 newRoom, 怪物实例 movedata)
    {
        newRoom.怪物进入(movedata);
    }
    /// <summary>
    /// 时机函数
    /// </summary>
    public virtual void Onwindup() { } // 在不同钩子被调用：

    public virtual void OnRoamStart() { }
    public virtual void OnEnterRoom() { }
    public virtual void OnTargetFound() { }

    /// <summary>
    /// move系统
    /// </summary>
    

    void moveto(Room from, Room to, int nowindex, GameObject pt, 怪物实例 movedata, SpriteRenderer spir)
    {
        if (!ismoving)
        {
            if (Math.Abs(pt.transform.position.x - to.Left) <= Math.Abs(pt.transform.position.x - to.Right))//向右
            {
                if (from.elavator != whereelvator.none && Math.Abs(to.Y - from.Y) >= 3f && to.isshelter == isshel.no)//有电梯分支
                {
                    if (from.elavator == whereelvator.left)
                    {
                        targetX = from.Left;
                        spir.flipX = true;
                       
                    }
                    else
                    {
                        targetX = from.Right;
                        spir.flipX = false;
                    }
                }
                else if (to.isshelter == isshel.no)//正常
                {
                    targetX = from.Right;
                    spir.flipX = false;
                }
            }
            else//向左
            {
                if (from.elavator != whereelvator.none && Math.Abs(to.Y - from.Y) >= 3f && to.isshelter == isshel.no)//有电梯分支
                {
                    if (from.elavator == whereelvator.left)
                    {
                        targetX = from.Left;                   
                        spir.flipX = true;
                    }
                    else
                    {
                        targetX = from.Right;
                        spir.flipX = false;
                    }
                }
                else if (to.isshelter == isshel.no)//正常
                {
                    targetX = from.Left;
                    spir.flipX = true;
                }
            }

            ismoving = true;
        }

        //Debug.Log("startgo");

        Vector3 pos = pt.transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, movedata.movespeedl * tct.Instance.timec * Time.deltaTime);//水平移动
        pt.transform.position = pos;

        if (Mathf.Abs(pt.transform.position.x - targetX) < 0.05f)// 到达端点
        {
            if (Math.Abs(to.Left - pt.transform.position.x) <= Math.Abs(to.Right - pt.transform.position.x))
            {
                isgoingleft = false;
            }
            else
            {
                isgoingleft = true;
            }

            float enterX;
            if (isgoingleft)
            {
                enterX = to.Right;
            }
            else
            {
                enterX = to.Left;
            }

            TeleportToRoom(to, enterX, pt, movedata, spir, from);
            
            ismoving = false;
        }
    }

    void TeleportToRoom(MapSystem.Room room, float x, GameObject pt, 怪物实例 movedata, SpriteRenderer spir, MapSystem.Room oldroom)
    {
        var b = oldroom.ob.GetComponent<房间>();
        b.移除l(movedata);

        movedata.解绑房间(b);//l更新
        b.怪物离开(movedata);

        pt.transform.position = new Vector3(x, room.Y, pt.transform.position.z);
        movedata.lnowroom = room.RoomId;


        movedata.needchangetarget = true;

        var a = room.ob.GetComponent<房间>();
        a.注册l(movedata);

        movedata.绑定房间(a);
        
        movedata.pathIndex += 1;

        movedata.刷新roomren缓存();
        进入新房间(a, movedata);

    }

    void firstmove(GameObject pt, 怪物实例 movedata, SpriteRenderer spir)
    {
        ismoving = false;

        if (movedata.lpath[movedata.pathIndex].isshelter == isshel.yes)
        {
            TeleportToRoom(
                movedata.lpath[movedata.pathIndex + 1],
                (movedata.lpath[movedata.pathIndex + 1].Right + movedata.lpath[movedata.pathIndex + 1].Left) / 2,
                pt,
                movedata,
                spir,
                movedata.lpath[movedata.pathIndex]
            );

            
        }
    }
}