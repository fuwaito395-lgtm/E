using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MapSystem;
using static 人数据列表;

[CreateAssetMenu(menuName = "L/goout/焦化少女")]
public class 焦化少女goout : 怪物漫游Base
{
    public override 怪物漫游 CreateRuntime怪物漫游(怪物实例 nowL)
    {
        return new 焦化少女gooutRuntime(this, nowL);
    }

    public class 焦化少女gooutRuntime : 怪物漫游
    {
        public 焦化少女gooutRuntime(焦化少女goout so, 怪物实例 nowL): base(so, nowL)
        {

        }
        public override void Onhitrange(GameObject pt, 怪物实例 movedata)
        {
            foreach (var r in movedata.roomren)
            {
                
                var ob = 初始化.instance.getobjectfromrenid(r.Data.bianhao);
                if (movedata.nowatt.afterrange > Math.Abs(movedata.关联对象.transform.position.x - ob.transform.position.x)
                    && r.Data.CurrentRoomId == movedata.lnowroom)
                {
                    
                    var perfet = ob.GetComponent<员工perfer>();
                    perfet.calfinaldamega(movedata.nowatt.damage, movedata.nowatt._Damage, perfet, this.parent,null);
                    perfet.ReceiveBuff("燃烧",25);
                }
            }
            movedata.nowhpl = 0;
        }
        public override 员工perfer 查询目标员工(List<员工perfer> 候选员工, 怪物实例 movedata)
        {
            movedata.怪物漫游运行时[movedata.nowLtype].calroomrennull(movedata.关联对象, movedata);
            return null;
        }
    }
}