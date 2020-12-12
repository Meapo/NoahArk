using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用来控制关卡选择界面的云层的移动，挂载在云层图片的父物体上
/// 一共两个云层照片滚动播放
/// </summary>
public class CloudMove : MonoBehaviour
{
    public float endPosX;
    public float movSpeed;
    RectTransform[] rects;
    // Start is called before the first frame update
    void Start()
    {
        rects = GetComponentsInChildren<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0;i<rects.Length-1;i++)
        {
            rects[i + 1].Translate(new Vector3( movSpeed * Time.deltaTime, 0,0));
            if(rects[i+1].anchoredPosition.x>endPosX)
            {
                rects[i+1].Translate(new Vector3(-rects[i+1].sizeDelta.x * 2, 0));
            }
        }
    }
}