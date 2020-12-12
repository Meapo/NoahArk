using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本控制玩家按下ESC后的UI反应，如果其他可关闭UI界面打开时，按下ESC使其关闭
/// 如果有不可关闭UI界面打开时，按下ESC什么都不做，否则默认打开Setting。
/// </summary>
public class EscControlUI : MonoBehaviour
{
    public GameObject SettingUI;
    public OpeningUI openingUI;
    // Start is called before the first frame update
    private void Start()
    {
        openingUI = OpeningUI.GetInstance();
        openingUI.UIDefault = SettingUI;
    }  
        // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(openingUI.UINotCloseByESC==null)
            {
                if(openingUI.UICanCloseByESC==null)
                {
                    openingUI.UIDefault.SetActive(!openingUI.UIDefault.activeSelf);
                }
                else
                {
                    openingUI.UICanCloseByESC.SetActive(false);
                    openingUI.UICanCloseByESC = null;
                }
             
            }
        }
    }
}
