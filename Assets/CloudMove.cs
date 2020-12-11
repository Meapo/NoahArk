using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float endPosX;
    public float movSpeed;
    RectTransform[] rects;
    // Start is called before the first frame update
    void Start()
    {
        rects = GetComponentsInChildren<RectTransform>();
        Debug.Log(rects.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0;i<rects.Length-1;i++)
        {
            rects[i + 1].Translate(new Vector3( movSpeed * Time.deltaTime, 0,0));
            if(rects[i+1].anchoredPosition.x>endPosX)
            {
                //Debug.Log(tran.sizeDelta.x);
                rects[i+1].Translate(new Vector3(-rects[i+1].sizeDelta.x * 2, 0));
            }
        }
    }
}