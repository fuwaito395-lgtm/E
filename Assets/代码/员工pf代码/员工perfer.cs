using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class 员工perfer : MonoBehaviour
{

    
    public 员工setatt setat;
    public BuffManager buff;
    [HideInInspector] public 人数据列表.ren Data;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public 员工移动 movec;
    [HideInInspector] public 员工set状态 set;
    [HideInInspector] public 血条显示 bloodphycis;
    [HideInInspector] public 白条显示 mindphycis;
    public 房间 当前房间;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void datafollow(人数据列表.ren data)
    {
        Data = data;
        sr.sprite = data.tuptx;

        var roomData = MapSystem.Instance.GetRoom(Data.startplace);
        if (roomData != null && roomData.ob != null)
        {
            当前房间 = roomData.ob.GetComponent<房间>();
            if (当前房间 != null)
            {
                当前房间.注册员工(this);
            }
            else
            {
                Debug.LogError("初始房间没有 房间 组件");
            }
        }
        else
        {
            Debug.LogError("初始房间不存在");
        }
    }
    //--------------------

    public void calfinaldamega(float damegavol, class_damage.damegatype damegatyp, 员工perfer 员工, 怪物实例 nowL, 员工setwork work)
    {
       
        damegavol = (int)System.Math.Ceiling(damegavol);

        //后续抗性

        伤害图片控制器.instance.Show(damegatyp, (int)damegavol, 员工.transform.position);
        if (damegatyp== class_damage.damegatype.physic)
        {
            Data.hp -= (int)damegavol;
            if (Data.hp > Data.maxhp)
            {
                Data.hp = Data.maxhp;
            }
            float ratio = (float)Data.hp / Data.maxhp;
            bloodphycis.updatephysic(ratio);
            if(Data.hp<=0)
            {
                
                Data.currentState = 人数据列表.ren.state.die;
            }
        }else if (damegatyp == class_damage.damegatype.mind)
        {
            Data.mp -= (int)damegavol;
            if(Data.mp> Data.maxmp)
            {
                Data.mp = Data.maxmp;
            }
            float ratio = (float)Data.mp / Data.maxmp;
            mindphycis.updatemind(ratio);
        }
        else if (damegatyp == class_damage.damegatype.erosion)
        {
            Data.mp -= (int)damegavol;
            Data.hp -= (int)damegavol;
            if (Data.mp > Data.maxmp)
            {
                Data.mp = Data.maxmp;
            }
            if (Data.hp > Data.maxhp)
            {
                Data.hp = Data.maxhp;
            }
            float ratio = (float)Data.mp / Data.maxmp;
            mindphycis.updatemind(ratio);
            ratio = (float)Data.hp / Data.maxhp;
            bloodphycis.updatephysic(ratio);
            if (Data.hp <= 0)
            {
                Data.currentState = 人数据列表.ren.state.die;
            }
        }
    }
    public void 收到怪物进入通知(怪物实例 monster)
    {
        if (this.Data.currentState == 人数据列表.ren.state.move) return;
        this.Data.ltarget = monster;
        Debug.Log(monster.nametext);
        Data.currentState = 人数据列表.ren.state.att;
    }
    public void 收到怪物离开通知(怪物实例 monster)
    {
        if(this.Data.ltarget == monster)
        {
            if(当前房间.linroom.Count==0)
            {
                Data.currentState = 人数据列表.ren.state.idie;
            }else
            {
                Data.ltarget = 当前房间.linroom[0];
            }
        }
    }
    public void ReceiveBuff(string buffName, int layers)
    {
        BuffBase newBuff = null;
        switch (buffName)
        {
            case "燃烧":
                newBuff = new 燃烧 { Name = "燃烧", Layers = layers };
                break;
            case "流血":
                newBuff = new 流血 { Name = "流血", Layers = layers };
                break;
        }

        if (newBuff != null && buff != null)
        {
            buff.AddBuff(newBuff);
        }
    }

}

