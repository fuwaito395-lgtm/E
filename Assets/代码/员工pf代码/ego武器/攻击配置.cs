using UnityEngine;

[System.Serializable]
public class 攻击配置
{
    public AttackSO 攻击;

    [Header("这个攻击自己的判定策略")]
    public 攻击策略Base 策略;

    [Header("这个攻击自己的效果")]
    public 武器效果Base[] 效果列表;
}