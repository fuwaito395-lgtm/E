using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static class_damage;

[CreateAssetMenu(menuName = "L/goout/求知的稻草人")]
public class 求知的稻草人goout : 怪物漫游Base
{
    public override 怪物漫游 CreateRuntime怪物漫游(怪物实例 nowL)
    {
        return new 求知的稻草人gooutRuntime(this, nowL);
    }

    public class 求知的稻草人gooutRuntime : 怪物漫游
    {
        
    public 求知的稻草人gooutRuntime(求知的稻草人goout so, 怪物实例 nowL) : base(so, nowL)
        {

        }
    public override void Onhitrange(GameObject pt, 怪物实例 movedata)
    {
        var sp = movedata.关联对象.GetComponent<SpriteRenderer>();
        float monsterX = movedata.关联对象.transform.position.x;

        foreach (var r in movedata.roomren)
        {
            var ob = 初始化.instance.getobjectfromrenid(r.Data.bianhao);
            float targetX = ob.transform.position.x;

            // 必须在同房间
            if (r.Data.CurrentRoomId != movedata.lnowroom) continue;

            float dx = targetX - monsterX; // 方向距离

            if (sp.flipX == false)
            {
                if (dx > 0 && dx < movedata.nowatt.afterrange)
                {
                   if (movedata.nowatt == movedata.数据.attacks[2])
                   {
                       movedata.nowhpl += 50;
                   }
                    var perfet = ob.GetComponent<员工perfer>();
                    perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage, perfet, this.parent, null);
                    break;
                }
            }
            else
            { 
                
                if (dx < 0 && -dx < movedata.nowatt.afterrange)
                {
                    if (movedata.nowatt == movedata.数据.attacks[2])
                    {
                       movedata.nowhpl += 50;
                            movedata.calfinaldamega(-50, class_damage.damegatype.physic, null, movedata, null);
                    }
                    var perfet = ob.GetComponent<员工perfer>();
                    perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage, perfet, this.parent, null);
                    break;
                }
            }
        }
    }
    public override void getnowatt(GameObject pt, 怪物实例 movedata)
        {
            
            
            if (movedata.targetren != null && movedata.targetren.Data.hp < 20)// 低血强制第二招
            {
                movedata.nowatt = movedata.数据.attacks[2];
                return;
            }

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
    }
}