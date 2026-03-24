using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static class_damage;

public class 伤害图片控制器 : MonoBehaviour
{
    public static 伤害图片控制器 instance;

    [Serializable]
    public class DamageIconData
    {
        public damegatype type;   // 伤害类型枚举
        public Sprite icon;       // 对应图标
    }

   
    public DamagePopup popupPrefab;

    
    public List<DamageIconData> iconList = new List<DamageIconData>();

    
    public Canvas uiCanvas;

    private Camera mainCam;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }
    public void Show(damegatype type, int damage, Vector3 worldPos)
    {
        Sprite icon = GetIcon(type);

        DamagePopup popup = Instantiate(popupPrefab, worldPos, Quaternion.identity);
        popup.Init(icon, damage);
    }

    private Sprite GetIcon(damegatype type)
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].type == type)
                return iconList[i].icon;
        }
        return null;
    }
}
