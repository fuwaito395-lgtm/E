using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 人数据列表 : MonoBehaviour
{
    public static 人数据列表 intance;
    public void Awake()
    {
        intance = this;
    }
    [System.Serializable]
    public class ren
    {
        //基础属性
        [HideInInspector] public GameObject 关联对象;
        public int bianhao;//编号
        public Sprite tuptx;//图片头像
        
        public int startplace;//开始区域
        public int maxhp;//勇气-暴怒
        public int hp;
        public int maxmp;//谨慎-纵欲
        public int mp;
        public int movesp;//勤勉-懒惰
        public int works;//自律-贪婪
        public int emotion; //沉静-空虚
        public int attpower;//正义-傲慢
        public int lhda;//宽容-嫉妒

        public int CurrentRoomId; // 当前房间编号
        public int lastroomid;//上个房间编号
        
        public int 勇气;
        public int 谨慎;
        public int 勤勉;
        public int 自律;
        public int 沉静;
        public int 正义;
        public int 宽容;

        //E.G.O
        public int egoatt;//ego武器列表代号
        public int egoarmor;//ego护甲列表代号
        public int ego;//ego核心(被动)
        public int nextwork;
        public int seworktype;
        //effect

        //List<effect> _effect=new List<effect>

        //状态
        public enum state
        {
            move,
            work,
            att,
            mp0,
            idie,
            die
        }
        public state currentState = state.idie;
        // 放到 人数据列表 类中
        [HideInInspector] public List<怪物实例> linroom = null;
        public 怪物实例 ltarget = null;


    }
    public List<ren> yuanggong = new List<ren>();
    
    public Dictionary<int, List<ren>> roomIndex = new Dictionary<int, List<ren>>();
    
}
