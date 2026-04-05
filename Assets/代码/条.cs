
using UnityEngine;

public class 条 : MonoBehaviour
{
    public static 条 instance;
    SpriteRenderer sr;
    private double x=0.5;
    public int totallht;
    private void Awake()
    {
        totallht = 0;
        instance = this;
        sr = GetComponent<SpriteRenderer>();
    }
    public void whenworkdone()
    {
        x = this.transform.localScale.x+0.5;
        if (x == 1)
        {
            Debug.Log("条满-"+totallht);
            totallht += 2;
            收容系统.Instance.条满(totallht);
        }
        else
        {
            this.transform.localScale = new Vector3((float)x, 1, 1);
        }
        
        
    }
}
