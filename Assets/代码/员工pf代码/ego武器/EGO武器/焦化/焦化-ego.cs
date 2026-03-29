using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ehoatt/焦化")]
public class 焦化ego : 武器效果Base
{
    public override 武器效果运行时 CreateRuntime(武器实例 owner)
    {
        return new 焦化ego1(this,owner);
    }
    public class 焦化ego1: 武器效果运行时
    {
        private 焦化ego _cfg;
        public 焦化ego1(焦化ego cfg, 武器实例 owner) : base(cfg,owner)
        {
            _cfg = cfg;
        }

        public override void OnAttackEnd(int hitCount, 怪物实例 m,员工perfer owner)
        {
            //Debug.Log("add buff");
            m.ReceiveBuff("燃烧", 8);
        }
    }
}
