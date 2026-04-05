using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "怪物资料")]
public class 怪物资料SO : ScriptableObject
{
    public List<资料分类> 分类列表;
}

[System.Serializable]
public class 资料分类
{
    public 资料类型 类型;
    public List<资料条目> 条目列表;
}

[System.Serializable]
public enum 资料类型
{
    基础资料,
    工作资料,
    管理须知,
    敏感信息
}

[System.Serializable]
public class 资料条目
{
    public string[] 文字内容;
    public Sprite 图片内容;
    public int 解锁点数;
    public int current;
}