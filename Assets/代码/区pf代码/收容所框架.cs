using UnityEngine;
using TMPro; 
[System.Serializable]
public class 收容所记录
{
    public int 房间Id;
    public int 怪物Id; // 空或 null 表示空收容所
    public bool 已解锁 = true;
    
    [HideInInspector] public 怪物实例 怪物数据实例; // 实例，null表示空
    [HideInInspector] public TextMeshPro maxwork;
    [HideInInspector] public TextMeshPro nowwork;
    [HideInInspector] public SpriteRenderer harmsp;
    [HideInInspector] public TextMeshPro nowgooutindex;
}



