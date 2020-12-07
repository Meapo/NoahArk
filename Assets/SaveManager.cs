using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }
}
