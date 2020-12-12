using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本为单例脚本，用于记录现在打开的UI，用于脚本之间交互。
/// </summary>
public class OpeningUI
{
    private static OpeningUI instance=new OpeningUI();
    private OpeningUI()
    {
        UICanCloseByESC = null;
        UINotCloseByESC = null;
    }
    public static OpeningUI GetInstance()
    {
        if(instance==null)
        {
            instance = new OpeningUI();
        }
        return instance;
    }
    /// <summary>
    /// 不可以被ESC关闭的脚本
    /// </summary>
    public GameObject UICanCloseByESC;
    /// <summary>
    /// 不可以被ESC关闭的UI
    /// </summary>
    public GameObject UINotCloseByESC;
    /// <summary>
    /// ESC默认控制的UI。
    /// </summary>
    public GameObject UIDefault;
}
