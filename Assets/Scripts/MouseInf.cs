using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来记录一些鼠标的操作信息，来控制不同类之间的交互
/// </summary>
public class MouseInf 
{
    static MouseInf instance = new MouseInf();
    private MouseInf() 
    {
        isRelay = false;
    }

    public static MouseInf GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// 是否放下方块
    /// </summary>
    public bool isRelay;
}
