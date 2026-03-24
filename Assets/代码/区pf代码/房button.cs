using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class 房button : MonoBehaviour
{
    public int RoomId;
    void Awake()
    {
        RoomId = transform.GetSiblingIndex();
    }
    void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        点击控制器.Instance.onclickroom(RoomId);
    }
}
