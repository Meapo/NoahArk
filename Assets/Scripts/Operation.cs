using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 操作类，用方块的移动信息组成，用于撤回操作
/// </summary>
public class Operation 
{
    public Stack<MoveInf> operation = new Stack<MoveInf>();
    public static Operation instance=null;
    private Operation() { }

    public static void CreateNewInstance()
    {
        instance = new Operation();
    }
    public static Operation GetInstance()
    {
        if(instance==null)
        {
            instance = new Operation();
        }
        return instance;
    }

    public void AddMoveInf(MoveInf move)
    {
        operation.Push(move);
    }
}   
