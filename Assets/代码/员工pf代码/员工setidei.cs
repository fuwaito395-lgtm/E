using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class 员工setidei : MonoBehaviour
{
    [HideInInspector] private 员工perfer Datapfset;
    [HideInInspector] private MapSystem.Room nowroom;
    [HideInInspector] private List<MapSystem.Room> map;
    [HideInInspector] public bool ismoving=false;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public int moveorstop;
    [HideInInspector] public float timer;
    [HideInInspector] private float center;
    [HideInInspector] private float halfWidth;
    [HideInInspector] SpriteRenderer sr;
    [HideInInspector] public 怪物实例 targetl;
    private void Awake()
    {
        Datapfset = GetComponent<员工perfer>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        map = MapSystem.Instance.Rooms;
    }
    public void ideiupdate()
    {
        if(Datapfset.Data==null||tct.Instance.timec==0)
        {
            return;
        }
       
        nowroom = map[Datapfset.Data.CurrentRoomId];
        center = (nowroom.Left + nowroom.Right) / 2f;
        halfWidth = (nowroom.Right - nowroom.Left) / 2f;

        if (Mathf.Abs(transform.position.x - center) > halfWidth - 2f)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, (nowroom.Left+ nowroom.Right)/2,Datapfset.Data.movesp *tct.Instance.timec *Time.deltaTime);//水平移动
            transform.position = pos;
            target.x = transform.position.x;
            setflip(center,transform.position.x);
            //Debug.Log("2");
        }
        else if (!ismoving && timer <= 0f)
        {
            moveorstop = getrandon(0, 2);
            if (moveorstop == 0)
            {
                timer = getrandon(2, 6);//获得停留时间
            }
            if(moveorstop==1)
            {
                target.x = getrandon((int)nowroom.Left + 1, (int)nowroom.Right - 1);//获得目标position
                setflip(target.x, transform.position.x);//反转图片？
                ismoving = true;
                
            }
        }else
        {
            
            if(Mathf.Abs(transform.position.x - target.x) < 1f)//达到目标？
            {
                if(ismoving==true)
                {
                    ismoving = false;
                    timer += 4;
                }
                timer -= (Time.deltaTime * tct.Instance.timec);
            }
            else
            {
                Vector3 pos2 = transform.position;
                pos2.x = Mathf.MoveTowards(pos2.x, target.x, Datapfset.Data.movesp * tct.Instance.timec * Time.deltaTime);//水平移动
                transform.position = pos2;
                //Debug.Log("1");
            }
        }
        
    }

    public void resetidei()
    {
        target.x = getrandon((int)nowroom.Left + 1, (int)nowroom.Right - 1); 
        ismoving = false;
        timer = 0f;
    }
    public int getrandon(int a,int b)
    {
        return Random.Range(a, b); ;
    }

    public void setflip(float _target,float nowpo)
    {
        if(_target<nowpo)
        {
            sr.flipX = false;
        }else
        {
            sr.flipX = true;
        }
    }
}
