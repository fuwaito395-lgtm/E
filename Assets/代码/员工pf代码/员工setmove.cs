using System;
using System.Collections.Generic;

using UnityEngine;
using static MapSystem;

public class 员工移动 : MonoBehaviour
{
    [HideInInspector] private 员工perfer Datapfmove;

    [HideInInspector] public 员工setidei changetoidei;
    [HideInInspector] public List<Room> path;    // 当前路径
    [HideInInspector] private int pathIndex;  // 路索引

    [HideInInspector] private float targetX;  // 移动目标
    [HideInInspector] private bool ismoving;    // 正在移动
    [HideInInspector] private bool isgoingleft;
    [HideInInspector] private SpriteRenderer sr;
    

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Datapfmove = GetComponent<员工perfer>();
    }

    public void setpath(List<Room> newPath,int lastroomid)
    {
        Datapfmove.Data.lastroomid = lastroomid;
        path = newPath;
        if (Datapfmove.Data == null || path == null || path.Count == 0)
        {
            Debug.Log("Data==null?");
            return;
        }

        if (newPath == null || newPath.Count == 0)
        {
            return;
        }
        pathIndex = 0;
        ismoving = false;
        if(path[pathIndex].isshelter==isshel.yes)
        {
            Room from = path[pathIndex];
            Room to = path[pathIndex + 1];
            TeleportToRoom(
            to,
            (path[pathIndex+1].Right+ path[pathIndex+1].Left)/2
            , from);
            if (Math.Abs(transform.position.x - to.Left) <= Math.Abs(transform.position.x - to.Right))//向右
            {
                if (from.elavator != whereelvator.none &&
                    Math.Abs(to.Y - from.Y) >= 3f &&
                    to.isshelter == isshel.no)//有电梯分支
                {
                    if (from.elavator == whereelvator.left) sr.flipX = false;
                    if (from.elavator == whereelvator.right) sr.flipX = true;
                }
                else//正常
                {
                    sr.flipX = true;
                }
            }
            else//向左
            {
                if (from.elavator != whereelvator.none &&
                    Math.Abs(to.Y - from.Y) >= 3f &&
                    to.isshelter == isshel.no)//有电梯分支
                {
                    if (from.elavator == whereelvator.left) sr.flipX = false;
                    if (from.elavator == whereelvator.right) sr.flipX = true;
                }
                else//正常
                {                   
                    sr.flipX = false;
                }
            }
            pathIndex += 1;
        }

        Datapfmove.Data.currentState = 人数据列表.ren.state.move;
        /*foreach(var check in newPath)
        {
            Debug.Log(check.RoomId);
        }*/
    }

    public void MoveUpdate()
    {
        Room current = path[pathIndex];
        
        if (pathIndex >= path.Count-1)
        {
            if(current.isshelter==isshel.no)
            {
                changetoidei.resetidei();
                Datapfmove.Data.currentState = 人数据列表.ren.state.idie;
            }else
            {
                Datapfmove.Data.currentState = 人数据列表.ren.state.work;
            }
            return;
        }
        Room next = path[pathIndex + 1];
        moveto(current, next,pathIndex);
        
    }

    
    void moveto(Room from,Room to,int nowindex)
    {
        if (!ismoving)
        {
            /*Debug.Log(transform.position.x);
            Debug.Log(to.Left);
            Debug.Log(to.Right);*/
            if (Math.Abs(transform.position.x-to.Left) <= Math.Abs(transform.position.x-to.Right))//向右
            {
                if (from.elavator!= whereelvator.none &&
                    Math.Abs(to.Y - from.Y) >= 3f &&
                    to.isshelter == isshel.no)//有电梯分支
                {
                    if(from.elavator==whereelvator.left)
                    {
                        targetX = from.Left;
                        if (from.elavator == whereelvator.left) sr.flipX = false;
                        if (from.elavator == whereelvator.right) sr.flipX = true;
                    }
                    else
                    {
                        targetX = from.Right;
                        if (from.elavator == whereelvator.left) sr.flipX = false;
                        if (from.elavator == whereelvator.right) sr.flipX = true;
                    }
                    }else if(to.isshelter==isshel.no)//正常
                    {
                        targetX = from.Right;
                        sr.flipX = true;

                    }else//目标为收容所
                    {
                        targetX=((from.Right+from.Left)/2);
                        if(transform.position.x>= targetX)
                        {
                            sr.flipX = false;
                        }else
                        {
                            sr.flipX = true;
                        }
                            
                    }
                }
                else//向左
                {
                    if (from.elavator != whereelvator.none &&
                        Math.Abs(to.Y - from.Y) >= 3f &&
                        to.isshelter == isshel.no)//有电梯分支
                    {
                        if (from.elavator == whereelvator.left)
                        {
                            targetX = from.Left;
                            sr.flipX = false;
                        }
                        else
                        {
                            targetX = from.Right;
                            sr.flipX = true;
                        }
                    }else if(to.isshelter == isshel.no)//正常
                    {
                        targetX = from.Left;
                        sr.flipX = false;
                    }else//目标为收容所
                    {
                        targetX = ((from.Right + from.Left) / 2);
                        if (transform.position.x >= targetX)
                        {
                            sr.flipX = false;
                        }
                        else
                        {
                            sr.flipX = true;
                        }
                    }


                }
                ismoving = true;
            }  
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetX, Datapfmove.Data.movesp * tct.Instance.timec * Time.deltaTime);//水平移动
            transform.position = pos;

            if (Mathf.Abs(transform.position.x - targetX) < 0.05f)// 到达端点
            {
            
                if (Math.Abs(to.Left - transform.position.x) <= Math.Abs(to.Right - transform.position.x))
                {
                    isgoingleft = false;
                }
                else
                {
                    isgoingleft = true;
                }
                float enterX;
                if(isgoingleft)
                {
                    enterX = to.Right;
                }else
                {
                    enterX = to.Left;
                }

                TeleportToRoom(to, enterX,from);
                pathIndex++;
                ismoving = false;
            }
        }

    void TeleportToRoom(MapSystem.Room room, float x, MapSystem.Room oldroom)
    {
        if (Datapfmove == null || Datapfmove.Data == null)
        {
            Debug.LogError("Datapfmove 或 Data 为空");
            return;
        }

        if (room == null)
        {
            Debug.LogError("目标 room 为空");
            return;
        }

        Datapfmove.当前房间.e离开(this.Datapfmove);

        transform.position = new Vector3(x, room.Y, transform.position.z);
        Datapfmove.Data.CurrentRoomId = room.RoomId;


            var roomData = MapSystem.Instance.GetRoom(Datapfmove.Data.CurrentRoomId);
        
            Datapfmove.当前房间 = roomData.ob.GetComponent<房间>();
            if (Datapfmove.当前房间 != null)
            {
                Datapfmove.当前房间.注册员工(Datapfmove);
            }
            else
            {
                Debug.LogError("房间物体上没有挂 房间 组件");
            }
        Datapfmove.当前房间.e进入(this.Datapfmove);
    }
}
