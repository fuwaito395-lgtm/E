using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MapSystem;

public class 员工set状态 : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public 员工setidei idei;
    [HideInInspector] public 员工移动 movesr;
    [HideInInspector] public 员工setwork worksr;
    [HideInInspector] private 员工perfer Datapfset;
    [HideInInspector] public 员工setatt at;
    [HideInInspector] public bool isfindsrs=false;
    [HideInInspector] private 收容所记录 room = null;
    
    private void Awake()
    {
        Datapfset = GetComponent<员工perfer>();
    }

    void Update()
    {
        if(Datapfset.Data.currentState == 人数据列表.ren.state.idie)
        {
            
            idei.ideiupdate();

        }else if(Datapfset.Data.currentState ==人数据列表.ren.state.move)
        {
            
            movesr.MoveUpdate();

        }else if(Datapfset.Data.currentState == 人数据列表.ren.state.work)
        {
           
            if (isfindsrs == false)
            {
                foreach (var a in 收容系统.Instance.初始收容所列表)
                {
                    if (Datapfset.Data.CurrentRoomId == a.房间Id)
                    {
                        room = 收容系统.Instance.获取单元(a.房间Id);
                        break;
                    }
                }
                isfindsrs = true;
                if(room==null)
                {
                    Debug.Log("can not find room");
                }
            }
           
            worksr.workupdate(room , Datapfset.Data.works);

        }else if(Datapfset.Data.currentState == 人数据列表.ren.state.die)
        {
            
            var r = Datapfset.Data;

            Datapfset.当前房间.e离开(Datapfset);
            r.CurrentRoomId = -1;

            
            Debug.Log(this.Datapfset.name+" 死亡");
            Destroy(this.gameObject);
        }
        else if (Datapfset.Data.currentState == 人数据列表.ren.state.att)
        {
            
            at.attupdate();
        }
    }
    
    
}
