
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using static MapSystem;
using static 人数据列表;

public class 初始化 : MonoBehaviour
{

    public static 初始化 instance;
    public 人数据列表 renlist;
    public GameObject renpf;
    private Dictionary<int, GameObject> renidtoobject = new Dictionary<int, GameObject>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //人数据列表.intance.RebuildIndex();
        //更新出逃L的update.Instance.lplacereset();

        for (int i=0;i<renlist.yuanggong.Count;i++ )
        {
            人数据列表.intance.yuanggong[i].bianhao = i;

            人数据列表.intance.yuanggong[i].勇气 = (人数据列表.intance.yuanggong[i].maxhp + 29 ) / 30;
            人数据列表.intance.yuanggong[i].谨慎 = (人数据列表.intance.yuanggong[i].maxmp + 29) / 30;
            人数据列表.intance.yuanggong[i].勤勉 = (人数据列表.intance.yuanggong[i].movesp+ 29) / 30;
            人数据列表.intance.yuanggong[i].自律 = (人数据列表.intance.yuanggong[i].works + 29) / 30;
            人数据列表.intance.yuanggong[i].正义 = (人数据列表.intance.yuanggong[i].attpower + 29) / 30;
            人数据列表.intance.yuanggong[i].沉静 = (人数据列表.intance.yuanggong[i].lhda + 29) / 30;
            人数据列表.intance.yuanggong[i].宽容 = (人数据列表.intance.yuanggong[i].emotion + 29) / 30;
            var data = renlist.yuanggong[i];
            //Debug.Log(员工父物体.instance);
            GameObject g = UnityEngine.Object.Instantiate(
                renpf,
                new Vector3(0, 0, 0),
                Quaternion.identity,
                员工父物体.instance.transform
            );

            员工perfer view = g.GetComponent<员工perfer>();//获得员工代码
            view.Data.关联对象 = g;
            员工父物体.instance.allren.Add(view);
            view.datafollow(data);
            renidtoobject[i] = g;
            g.transform.position = new Vector3(
                (Instance.Rooms[data.startplace].Left+ Instance.Rooms[data.startplace].Right)/2,
                Instance.Rooms[data.startplace].Y,
                -2
            );
        }
    }
    public GameObject getobjectfromrenid(int id)
    {
        return renidtoobject[id];
    }

}
