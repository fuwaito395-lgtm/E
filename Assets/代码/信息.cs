using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class 信息 : MonoBehaviour
{
    public static 信息 instance;
    public TextMeshProUGUI 本能;
    public TextMeshProUGUI 洞察;
    public TextMeshProUGUI 沟通;
    public TextMeshProUGUI 压迫;
    public Image pt;
    public TextMeshProUGUI name1;
    public TextMeshProUGUI maxd;
    public TextMeshProUGUI mind;
    public TextMeshProUGUI damagetp;
    public TextMeshProUGUI harm;
    public TextMeshProUGUI 优;
    public TextMeshProUGUI 良;
    public TextMeshProUGUI 中;
    public TextMeshProUGUI 差;
    public TextMeshProUGUI 管理0;
    public TextMeshProUGUI 管理1;
    public TextMeshProUGUI 管理2;

    public TextMeshProUGUI 暴怒;
    public TextMeshProUGUI 纵欲;
    public TextMeshProUGUI 怠惰;
    public TextMeshProUGUI 贪婪;
    public TextMeshProUGUI 空虚;
    public TextMeshProUGUI 傲慢;
    public TextMeshProUGUI 嫉妒;
    private void Awake()
    {
        instance = this;
    }
    public void refreshdata(int lroomid)
    {
        var data = 收容系统.Instance.GetInstanceByRoom(lroomid).数据;
        var edata = data.资料;
        var fdata = edata.分类列表[0];
        refresh基本(data,edata,fdata);
        var sdata= edata.分类列表[1];
        refresh管理(data, edata, sdata,0);
        var tdata = edata.分类列表[2];
        refresh工作(data, edata, tdata);
    }
    public void refresh工作(L基本框架 data, 怪物资料SO edata, 资料分类 tdata)
    {
        TextMeshProUGUI[] texts = new TextMeshProUGUI[]
        {
        暴怒, 纵欲, 怠惰, 贪婪, 空虚, 傲慢, 嫉妒
        };

        float[][] values = new float[][]
        {
        data.暴怒,
        data.纵欲,
        data.怠惰,
        data.贪婪,
        data.空虚,
        data.傲慢,
        data.嫉妒
        };

        for (int i = 0; i < 7; i++)
        {
            
            if (tdata.条目列表[i].current != 1)
            {
                texts[i].text = "";
                continue;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int j = 0; j < 5; j++)
            {
                sb.Append((values[i][j]).ToString("F0"));
                sb.Append("%");

                if (j < 4)
                    sb.Append("\n");
            }

            texts[i].text = sb.ToString();
        }
    }
    public void refresh管理(L基本框架 data, 怪物资料SO edata, 资料分类 sdata,int index)
    {
        管理0.text = "";
        管理1.text = "";
        管理2.text = "";
        for (int i = 0; i < 3; i++)
        {
            int curIndex =index+i;
            if (curIndex >= sdata.条目列表.Count)
                continue;
            if(sdata.条目列表[curIndex].current - 1==-1)
            {
                continue;
            }
            switch (i)
            {
                case 0: 管理0.text = (curIndex+1).ToString()+") "+sdata.条目列表[curIndex].文字内容[sdata.条目列表[curIndex].current-1]; break;
                case 1: 管理1.text = (curIndex + 1).ToString() + ") " + sdata.条目列表[curIndex].文字内容[sdata.条目列表[curIndex].current-1]; break;
                case 2: 管理2.text = (curIndex + 1).ToString() + ") " + sdata.条目列表[curIndex].文字内容[sdata.条目列表[curIndex].current-1]; break;
            }
        }
    }
    public void refresh基本(L基本框架 data,怪物资料SO edata,资料分类 fdata)
    {
        for (int i = 0; i < 6; i++)
        {
            if (fdata.条目列表[i].current==1)
            {
                switch (i)
                {
                    case 0: pt.overrideSprite = fdata.条目列表[i].图片内容; break;
                    case 1: name1.text = data.显示名; break;
                    case 2: mind.text = data.mindamega.ToString(); maxd.text = data.maxdamega.ToString(); break;
                    case 3:
                        {
                            switch (data.damegatyp)
                            {
                                case class_damage.damegatype.physic:
                                    damagetp.text = "物理";
                                    damagetp.color = Color.red;
                                    break;

                                case class_damage.damegatype.mind:
                                    damagetp.text = "精神";
                                    damagetp.color = Color.white;
                                    break;

                                case class_damage.damegatype.erosion:
                                    damagetp.text = "侵蚀";
                                    damagetp.color = Color.black;
                                    break;

                                case class_damage.damegatype.soul:
                                    damagetp.text = "灵魂";
                                    damagetp.color = Color.blue;
                                    break;

                                default:
                                    damagetp.text = "认知";
                                    damagetp.color = new Color(1f, 0.85f, 0.2f);//金色
                                    break;
                            }
                            break;
                        }
                    case 4:
                        {
                            switch (data.harm)
                            {
                                case 1:
                                    harm.text = "Zayin";
                                    harm.color = new Color(0f, 1f, 0f);
                                    break;

                                case 2:
                                    harm.text = "Teth";
                                    harm.color = new Color(0f, 0f, 1f);
                                    break;

                                case 3:
                                    harm.text = "He";
                                    harm.color = new Color(1f, 1f, 0f);
                                    break;

                                case 4:
                                    harm.text = "Waw";
                                    harm.color = new Color(0.5f, 0f, 0.6f);
                                    break;

                                case 5:
                                    harm.text = "Aleph";
                                    harm.color = new Color(1f, 0f, 0f);
                                    break;

                                default:
                                    harm.text = "ALEPH";
                                    harm.color = new Color(0.5f, 0f, 0f);
                                    break;
                            }
                            break;
                        }
                    case 5:
                        {
                            优.text = "优 : " + (data.工作结果[2] + 1).ToString() + " --- " + data.totalwork.ToString();
                            良.text = "良 : " + (data.工作结果[1] + 1).ToString() + " --- " + data.工作结果[2].ToString();
                            中.text = "中 : " + (data.工作结果[0] + 1).ToString() + " --- " + data.工作结果[1].ToString();
                            差.text = "差 : " + "0" + " --- " + data.工作结果[0].ToString();
                            break;
                        }
                }
            }
        }
    }
}
