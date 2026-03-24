using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 白条显示 : MonoBehaviour
{
    SpriteRenderer sr;
    Vector3 originalLocalScale;
    float spriteWidthLocal;
    float leftLocalX;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalLocalScale = transform.localScale;


        spriteWidthLocal = sr.sprite.bounds.size.x;

        float curWidth = spriteWidthLocal * originalLocalScale.x;
        leftLocalX = transform.localPosition.x - curWidth * 0.5f;
    }

    public void updatemind(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        float newScaleX = Mathf.Max(0.0001f, originalLocalScale.x * ratio); // 避免 0 导致问题
        transform.localScale = new Vector3(newScaleX, originalLocalScale.y, originalLocalScale.z);
        float newWidth = spriteWidthLocal * newScaleX;
        transform.localPosition = new Vector3(leftLocalX + newWidth * 0.5f, transform.localPosition.y, transform.localPosition.z);
    }
}