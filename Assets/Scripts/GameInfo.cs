using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    VolumeManager volumeManager;
    public int nowLevel;
    private GameInfo()
    {
        volumeManager = VolumeManager.GetInstance();
        nowLevel = 1;
    }
    public static GameInfo instance=new GameInfo();
    public static GameInfo GetInstance()
    {
        if(instance==null)
        {
            instance = new GameInfo();
        }
        return instance;
    }

}
