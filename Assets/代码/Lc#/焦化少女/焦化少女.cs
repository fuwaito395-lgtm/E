using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "L/焦化少女")]
public class 焦化少女1 : 工作机制SO
{
    public override 工作机制运行时 CreateRuntime()
    {
        return new 焦化少女(this);
    }

    // 运行时类
    public class 焦化少女 : 工作机制运行时
    {
        private 焦化少女1 _cfg;
        public 焦化少女(焦化少女1 cfg) : base(cfg)
        {
            _cfg = cfg;
        }
        public override void onlastworkfinish(员工perfer 员工, 收容所记录 nowL, 员工setwork work)
        {
            if (员工.Data.宽容 <= 2|| work.工作差中良优<=2)
            {
                nowL.怪物数据实例.出逃计数(1);
            }
            
        }
    }
}

