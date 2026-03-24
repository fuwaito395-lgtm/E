using UnityEngine;
[CreateAssetMenu(menuName = "L/一罪与百善")]
public class 完成回复机制SO : 工作机制SO
{
    
    public override 工作机制运行时 CreateRuntime()
    {
        return new 一罪与百善donework(this);
    }

    // 运行时类
    public class 一罪与百善donework : 工作机制运行时
    {
        private 完成回复机制SO _cfg;
        public 一罪与百善donework(完成回复机制SO cfg) : base(cfg)
        {
            _cfg = cfg;
        }
        public override void onlastworkfinish(员工perfer nowren, 收容所记录 nowL, 员工setwork work)
        {
            if(work.工作差中良优==3)
            {
                nowren.Data.mp += 5;
                nowren.calfinaldamega(-5, class_damage.damegatype.mind, nowren, nowL.怪物数据实例, work);
            }
        }
    }
}