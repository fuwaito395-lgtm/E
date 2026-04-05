using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static class_damage;
using static MapSystem;
using static Unity.Burst.Intrinsics.X86.Avx;
using static 人数据列表;

public class 房间 : MonoBehaviour
{
    public List<员工perfer> 当前员工 = new List<员工perfer>();
    public List<怪物实例> linroom = new List<怪物实例>();

    public void 注册员工(员工perfer e)
    {
        if (e == null) return;
        if (!当前员工.Contains(e)) 当前员工.Add(e);
    }

    public void 移除员工(员工perfer e)
    {
        if (e == null) return;
        当前员工.Remove(e);
    }

    public void 注册l(怪物实例 l)
    {
        if (l == null) return;
        if (!linroom.Contains(l)) linroom.Add(l);
    }

    public void 移除l(怪物实例 l)
    {
        if (l == null) return;
        linroom.Remove(l);
    }

    public void e进入(员工perfer e)
    {
        if (e == null) return;

        if (!当前员工.Contains(e))
            当前员工.Add(e);

        Debug.Log($"[房间.e进入] 房间 {name} 收到员工进入，怪物数 = {linroom.Count}");

        for (int i = linroom.Count - 1; i >= 0; i--)
        {
            var l = linroom[i];
            if (l == null)
            {
                linroom.RemoveAt(i);
                continue;
            }

            l.刷新roomren缓存();
        }
        if(linroom.Count>0) e.收到怪物进入通知(linroom[0]);

    }

    public void e离开(员工perfer e)
    {
        if (e == null) return;

        if (当前员工.Remove(e))
        {
            Debug.Log($"[房间.e离开] 房间 {name} 收到员工离开，怪物数 = {linroom.Count}");

            for (int i = linroom.Count - 1; i >= 0; i--)
            {
                var l = linroom[i];
                if (l == null)
                {
                    linroom.RemoveAt(i);
                    continue;
                }
                Debug.Log("刷新");
                l.刷新roomren缓存();
            }
        }
    }

    public void 怪物进入(怪物实例 monster)
    {
        Debug.Log($"[房间.怪物进入] 房间 {name} 收到怪物进入，员工数 = {当前员工.Count}, 怪物 = {(monster == null ? "null" : monster.数据?.显示名)}");

        if (monster == null) return;

        if (!linroom.Contains(monster))
            linroom.Add(monster);

        

        for (int i = 当前员工.Count - 1; i >= 0; i--)
        {
            var emp = 当前员工[i];
            if (emp == null)
            {
                当前员工.RemoveAt(i);
                continue;
            }
            
            emp.收到怪物进入通知(monster);
        }

    }

    public void 怪物离开(怪物实例 monster)
    {
        if (monster == null)
        {
            Debug.Log("L=null");
            return;
        }
        linroom.Remove(monster);
            for (int i = 当前员工.Count - 1; i >= 0; i--)
        {
            var emp = 当前员工[i];
            if (emp == null)
            {
                当前员工.RemoveAt(i);
                continue;
            }
            emp.收到怪物离开通知(monster);
        }
    }
}


