using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "L/工作亡蝶葬仪")]
public class 工作亡蝶葬仪 : 工作机制SO
{
    public override 工作机制运行时 CreateRuntime()
    {
        return new 工作亡蝶葬仪1(this);
    }
    public class 工作亡蝶葬仪1 : 工作机制运行时
    {
        private 工作亡蝶葬仪 _cfg;
        public 工作亡蝶葬仪1(工作亡蝶葬仪 cfg) : base(cfg)
        {
            _cfg = cfg;
        }
        public override void onlastworkfinish(员工perfer 员工, 收容所记录 nowL, 员工setwork work)
        {
            if (员工.Data.谨慎 < 2 || work.工作差中良优 <= 2)
            {
                nowL.怪物数据实例.出逃计数(1);
            }
        }
        public override float ModifyChance(float 当前概率, 员工perfer 员工, 收容所记录 nowL, 员工setwork work)
        {
            float pro = 当前概率;
            if (员工.Data.seworktype==4)
            {
                pro += 50f;
            }
            
            return pro;
        }
    }
}



