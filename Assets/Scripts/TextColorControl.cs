using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextColorControl : MonoBehaviour
{
    public float minG;
    public float maxG;
    public float dirG;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        minG /= 255;
        maxG /= 255;
        dirG /= 255;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.color += new Color(0, dirG, 0, 0);
        if((text.color.g>maxG && dirG>0) || (text.color.g < minG && dirG < 0))
        {
            dirG = -dirG;
        }
    }
}
