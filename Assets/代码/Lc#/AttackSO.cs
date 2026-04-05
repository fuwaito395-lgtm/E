using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L/att")]
[System.Serializable]
public class AttackSO : ScriptableObject
{
    public string attname="";
    public bool isred=false;
    public string attackId;
    public string description;
    public int prop;

    public float windup = 0.08f;     // 前摇(s)
    public float hitTime = 0.12f;    // 相对开始的命中偏移(s)
    public float post = 0.08f;       // 后摇(s)
    public float beforerange = 1.2f;       // 范围
    public float afterrange = 1.2f;       // 范围
    public int damage = 10;          // 伤害
    public class_damage.damegatype _Damage;
    public bool IsflipX = true;
    public class_damage.attrang typerange;
    public AttackSO nextatt = null;
    public Sprite beforeatt;
    public Sprite afteratt;
    public AudioClip beforeaud;
    public AudioClip afteraud;
}

