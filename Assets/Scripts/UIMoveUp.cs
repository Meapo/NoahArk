using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMoveUp : MonoBehaviour
{
    // Start is called before the first frame update
    bool isMoveUp;
    public float moveDis;
    float hasMoveDis;
    public float speed;
    void Start()
    {
        isMoveUp = false;
        hasMoveDis = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasMoveDis<moveDis)
        {
            foreach (Transform trans in transform)
            {
                trans.Translate(new Vector3(0, speed * Time.deltaTime));
            }
            hasMoveDis += speed * Time.deltaTime;
        }
        else
        {
            if(!isMoveUp)
            {
                isMoveUp = true;
                Button[] buttons = GetComponentsInChildren<Button>();
                foreach(Button button in buttons)
                {
                    button.interactable = true;
                }
                ButtonImageInteractive[] bImageInter = GetComponentsInChildren<ButtonImageInteractive>();
                foreach (ButtonImageInteractive btImage in bImageInter)
                {
                    btImage.interactive = true;
                }
            }
        }
    }
}
