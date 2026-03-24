using UnityEngine;

[CreateAssetMenu(menuName = "员工/武器数据")]
public class 武器SO : ScriptableObject
{
    public int 武器Id;
    public string 显示名;

    public float 基础伤害 = 10f;
    public float 攻击范围 = 1.5f;
    public float 实际攻击范围 = 1.5f;
    public float 攻击间隔 = 1f;

    public bool 是否范围伤害 = false;
    public float 范围半径 = 1.5f;

    public bool 是否误伤友方 = false;

    public class_damage.damegatype 伤害类型;

    public 武器效果Base[] 默认效果列表;
}