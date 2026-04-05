using System.Collections;
using UnityEngine;
using static 寻路系统dij;

public class 点击控制器 : MonoBehaviour
{
    员工移动 callmove;
    int rennowroom=-1;
    public 寻路系统dij dij;
    员工perfer nowren;
    int nowsrsid;
    [SerializeField] GameObject mainpanel;
    [SerializeField] GameObject closemask;
    [SerializeField] GameObject secondpanel;
    [SerializeField] GameObject 资料panel;

    private bool canClose = false;
    int wordtype;
    int secondwordtype;

    public static 点击控制器 Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        mainpanel.SetActive(false);
        closemask.SetActive(false);
        secondpanel.SetActive(false);
    }
    public void onclickren(员工perfer s, 员工移动 movec)
    {
        nowren = s;
        callmove = movec;
        rennowroom = s.Data.CurrentRoomId;

    }

    public void onclickroom(int roomid)
    {
        if (rennowroom == -1||nowren==null)
        {
            Debug.Log("取消选中");
            return;
        }else
        {
            Debug.Log($"点击房间 ss = {roomid}, 当前员工房间 = {rennowroom}");

            pathResult pathResult = dij.寻路(rennowroom, roomid);
            if (pathResult == null || pathResult.lui.Count == 0)
            {
                Debug.LogWarning("无法寻路到目标房间");
            }
            else
            {
                callmove.setpath(pathResult.lui, pathResult.lastroomid);
            }
            rennowroom = -1;
            nowren = null;
        }
    }

    
    public void onclicksrs(int srsid)
    {
        mainpanel.SetActive(true);
        
        closemask.SetActive(true);
        canClose = false;
        nowsrsid = srsid;
        
        var btn = closemask.GetComponent<UnityEngine.UI.Button>();// 禁用closemask的按钮，如果有Button组件
        if (btn != null) btn.interactable = false;

        StartCoroutine(EnableCloseAfterFrames(50)); // 等50帧再允许关闭
    }

    public void onclickmainpanel(int _wordtpye)
    {
        if(nowren==null)
        {
            return;
        }
        wordtype = _wordtpye;
        secondpanel.SetActive(true);
        
    }
    public void onclicksecondpanel(int _secondwordtype)
    {
        if (nowsrsid == -1||nowren==null)
        {
            return;
        }
        secondwordtype = _secondwordtype;
        nowren.Data.nextwork = wordtype;nowren.Data.seworktype = secondwordtype;
        pathResult pathResult = dij.寻路(rennowroom, nowsrsid);
        if (pathResult == null || pathResult.lui.Count == 0)
        {
            Debug.LogWarning("无法寻路到目标房间");
        }
        else
        {
            callmove.setpath(pathResult.lui, pathResult.lastroomid);
        }
        rennowroom = -1;
        nowren = null;
        ClosePanel();

    }
    public void OnClickMask()
    {
        
            secondwordtype = 0;
            wordtype = 0;
            ClosePanel();            
        
    }

    
    //工具
    IEnumerator EnableCloseAfterFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;

        canClose = true;

        var btn = closemask.GetComponent<UnityEngine.UI.Button>();
        if (btn != null) btn.interactable = true;
    }
    
    public void ClosePanel()
    {
        if (!canClose)
        {
            return; // 锁住期间不能关
        }
        mainpanel.SetActive(false);
        closemask.SetActive(false);
        secondpanel.SetActive(false);
        nowsrsid = -1;
    }
    public void onclick资料()
    {
        资料panel.SetActive(true);
        信息.instance.refreshdata(nowsrsid);
    }
}
