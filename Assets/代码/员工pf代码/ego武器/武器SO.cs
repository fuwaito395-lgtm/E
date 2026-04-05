using UnityEngine;

[CreateAssetMenu(menuName = "员工/武器数据")]
public class 武器SO : ScriptableObject
{
    public int 武器Id;
    public string 显示名;

    public float 攻击间隔 = 1f;
    public bool 是否误伤友方 = false;

    public 攻击配置[] 攻击列表;
}