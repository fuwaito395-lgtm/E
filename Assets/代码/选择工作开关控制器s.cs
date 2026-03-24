using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 选择工作开关控制器s : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject closemask;
    private bool isopen = false;
    public static 选择工作开关控制器s instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        panel.SetActive(false);
        closemask.SetActive(false);
    }

    public void OpenPanel()
    {
        if (isopen)
        {
            return;
        }
        StartCoroutine(EnableCloseNextFrame());
        panel.SetActive(true);
        isopen = true;
        
        closemask.SetActive(true);
    }
    IEnumerator EnableCloseNextFrame()
    {
        yield return null; // 等一帧
        
    }
    public void ClosePanel()
    {
        if (!isopen)
        {
            return;
        }

        panel.SetActive(false);
        isopen = false;
        closemask.SetActive(false);
    }
}
