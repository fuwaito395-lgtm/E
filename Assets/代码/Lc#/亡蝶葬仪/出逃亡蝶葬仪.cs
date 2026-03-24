using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static 怪物实例;

[CreateAssetMenu(menuName = "L/goout/亡蝶葬仪")]
public class 亡蝶葬仪goout : 怪物漫游Base
{
    public override 怪物漫游 CreateRuntime怪物漫游(怪物实例 nowL)
    {
        return new 亡蝶葬仪gooutRuntime(this, nowL);
    }

    public class 亡蝶葬仪gooutRuntime : 怪物漫游
    {
        public 亡蝶葬仪gooutRuntime(亡蝶葬仪goout so, 怪物实例 nowL) : base(so, nowL)
        {

        }
        public override void Onhitrange(GameObject pt, 怪物实例 movedata)
        {
            var sp = movedata.关联对象.GetComponent<SpriteRenderer>();
            float monsterX = movedata.关联对象.transform.position.x;

            if (movedata.nowatt == movedata.数据.attacks[2])
            {
                GameObject obj = Instantiate(L插件.instance.范围att, movedata.关联对象.transform.position, Quaternion.identity);

                通用攻击子弹 atk = obj.GetComponent<通用攻击子弹>();
                if (atk != null)
                {
                    atk.初始化(
                        movedata.nowatt.damage,
                        movedata.nowatt._Damage,
                        movedata,
                        6,
                        4f,
                        LayerMask.GetMask("Employee"),
                        movedata.数据.extrasp,
                        x: movedata.nowatt.afterrange,
                        y: 15f
                        
                    );
                }

                return;
            }
            foreach (var r in movedata.roomren)
            {
                var ob = 初始化.instance.getobjectfromrenid(r.Data.bianhao);
                float targetX = ob.transform.position.x;

                
                if (r.Data.CurrentRoomId != movedata.lnowroom) continue;// 必须在同房间

                float dx = targetX - monsterX; // 方向距离

                if (sp.flipX == false)
                {
                    
                    if (dx > 0 && dx < movedata.nowatt.afterrange)// 朝右：dx 必须 > 0
                    {
                        var perfet = ob.GetComponent<员工perfer>();
                        perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage, perfet, this.parent, null);
                        if (movedata.nowatt == movedata.数据.attacks[1])
                        {
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    
                    if (dx < 0 && -dx < movedata.nowatt.afterrange)// 朝左：dx 必须 < 0
                    {
                        var perfet = ob.GetComponent<员工perfer>();
                        perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage, perfet, this.parent, null);
                        if (movedata.nowatt == movedata.数据.attacks[1])
                        {
                            continue;
                        }
                        break;
                    }
                }
            }
        }
        public override void calispost(GameObject pt, 怪物实例 movedata)
        {
            if (movedata == null) return;
            if (movedata.关联对象 == null) return;
            if (movedata.nowatt == null) return;

            var spr = movedata.关联对象.GetComponent<SpriteRenderer>();
            movedata.stoptime += movedata.nowatt.post;

            
            if (movedata.nowatt.nextatt != null&&UnityEngine.Random.Range(0,17)!=5)
            {
                movedata.nowatt = movedata.nowatt.nextatt;
                movedata.allattttimel = allatttime.windup;
            }
            else
            {
                spr.sprite = movedata.数据.aftergoout;
                movedata.allattttimel = allatttime.none;
                movedata.nowatt = null;
            }
            // 清理攻击状态
            movedata.targetren = null;
            movedata.Tragetrenobj = null;
            movedata.needchangetarget = true;
            movedata.needchangetarget = true;
        }
        
        
    }
}