using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonImageInteractive : MonoBehaviour
{
    public float extendWidth;
    public float extendHeight;
    public float normalColor;
    public float pressColor;
    public float selectColor;
    public bool interactive;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        normalColor /= 255;
        pressColor /= 255;
        selectColor /= 255;
        interactive = false;
    }
    private void FixedUpdate()
    {
        if(interactive)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 distance = mousePosition - transform.position;
            if (Mathf.Abs(distance.x) < extendWidth && Mathf.Abs(distance.y) < extendWidth)
            {
                if (Input.GetMouseButtonDown(0))//鼠标点击在UI上
                {
                    image.color = new Color(pressColor, pressColor, pressColor);
                }
                else//鼠标选择在UI上
                {
                    image.color = new Color(selectColor, selectColor, selectColor, 1);
                }
            }
            else//鼠标不在UI上
            {
                image.color = new Color(normalColor, normalColor, normalColor, 1);
            }
        }
       

    }
    
}
