using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static 焦化少女1;
[CreateAssetMenu(menuName = "L/工作求知的稻草人")]
public class 工作求知的稻草人 : 工作机制SO
{
    public override 工作机制运行时 CreateRuntime()
    {
        return new 工作求知的稻草人1(this);
    }
    public class 工作求知的稻草人1 : 工作机制运行时
    {
        private 工作求知的稻草人 _cfg;
        public 工作求知的稻草人1(工作求知的稻草人 cfg) : base(cfg)
        {
            _cfg = cfg;
        }
        public override void onlastworkfinish(员工perfer 员工, 收容所记录 nowL, 员工setwork work)
        {
            if (员工.Data.谨慎 > 2 || work.工作差中良优 <= 2)
            {
                nowL.怪物数据实例.出逃计数(1);
            }
        }
        
    }
}

