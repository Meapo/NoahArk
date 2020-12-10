﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject setting;
    public void ClickToOpenSetting()
    {
        setting.SetActive(true);
    }
    public void ClickToCloseSetting()
    {
        setting.SetActive(false);
    }
    public void ClickToChangeSetting()
    {
        setting.SetActive(!setting.activeSelf);
    }
    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            setting.SetActive(!setting.activeSelf);
        }
    }*/
}
