
using UnityEngine;
using UnityEngine.EventSystems;

public class 员工点击button : MonoBehaviour
{
    
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        员工perfer s = GetComponent<员工perfer>();
        员工移动 movec = GetComponent<员工移动>();
        Debug.Log("成功点击员工");
        点击控制器.Instance.onclickren(s, movec);       
    }
}
