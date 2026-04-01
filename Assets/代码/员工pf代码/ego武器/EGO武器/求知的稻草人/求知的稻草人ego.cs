using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ehoatt/求知的稻草人")]
public class 求知的稻草人ego : 武器效果Base
{
    public override 武器效果运行时 CreateRuntime(武器实例 owner)
    {
        return new 求知的稻草人ego1(this, owner);
    }
    public class 求知的稻草人ego1 : 武器效果运行时
    {
        private 求知的稻草人ego _cfg;
        public 求知的稻草人ego1(求知的稻草人ego cfg, 武器实例 owner) : base(cfg, owner)
        {
            _cfg = cfg;
        }

        public override float ModifyDamage(float damage,怪物实例  targetObject,员工perfer owner)
        {
            if (owner.Data.mp <= owner.Data.maxmp / 3) return damage * 2;
            return damage;
        }
        public override void OnHitTarget(怪物实例 targetl, float finalDamage, bool hitFriend, 员工perfer owner)
        {
            targetl.ReceiveBuff("流血", 1);
            //Debug.Log("give流血");
        }
    }
}