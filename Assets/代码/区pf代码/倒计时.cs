using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class 倒计时 : MonoBehaviour
{
    public SpriteRenderer sr;
    float Timermax=0;
    float Timer=0;
    public TextMeshPro tm;
    private bool isrunning = false;
    public void startupdate(float totaltime)
    {
        Timer = totaltime;
        Timermax = totaltime;
        
        isrunning = true;
    }
    void Update()
    {
        if (!isrunning) return;
        Timer -= Time.deltaTime * tct.Instance.timec;

        if(Timer<0)
        {
            notdonework();
            endupdate();
            return;
        }
        float ratio = Timer / Timermax;


        sr.transform.localScale = new Vector3(ratio*15, 1, 1);
        tm.text = Mathf.CeilToInt(Timer).ToString();
    }
    public void endupdate()
    {
        Timer = 0;
        Timermax = 0;
        this.gameObject.SetActive(false);
        tm.text = "";
        isrunning = false;
    }
    public void notdonework()
    {
        var a = this.gameObject.GetComponentInParent<收容所button>();
        收容系统.Instance.GetInstanceByRoom(a.RoomId).出逃计数(100);
    }
}
