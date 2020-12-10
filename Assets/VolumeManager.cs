using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager
{
    public static VolumeManager instance = new VolumeManager();
    /// <summary>
    /// 背景音乐
    /// </summary>
    public float BGVolume;
    /// <summary>
    /// 音效
    /// </summary>
    public float EFVolume;

    private VolumeManager()
    {
        BGVolume = 1;
        EFVolume = 1;
    }
    public static VolumeManager GetInstance()
    {
        if(instance==null)
        {
            instance = new VolumeManager();
        }
        return instance;
    }
}
