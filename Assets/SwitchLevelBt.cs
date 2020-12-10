using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchLevelBt : MonoBehaviour
{
    public Transform levelSelect;

    public Button upButton;

    public Button downButton;
    [SerializeField]
    private int levelNum;
    
    GameInfo gameInfo;
    int nowLevel;
    // Start is called before the first frame update
    void Start()
    {
        gameInfo = GameInfo.GetInstance();
        nowLevel = gameInfo.nowLevel;
        for (int i = 0; i < levelSelect.childCount; i++)
        {
            Transform trans = levelSelect.GetChild(i);
            if(i==nowLevel-1)
            {
                trans.gameObject.SetActive(true);
            }
            else
            {
                trans.gameObject.SetActive(false);
            }
        }
        ChangeInteractive();
    }
    void ChangeInteractive()
    {
        if (nowLevel == 1)
        {
            downButton.interactable = false;
        }
        else
        {
            downButton.interactable = true;
        }
        if (nowLevel == levelNum)
        {
            upButton.interactable = false;
        }
        else
        {
            upButton.interactable = true;
        }
    }
    public void ClickToDown()
    {
        Transform nowButton = levelSelect.GetChild(nowLevel-1);
        nowLevel--;
        Transform nextButton = levelSelect.GetChild(nowLevel-1);
        nowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        ChangeInteractive();
    }
    public void ClickToUp()
    {
        Transform nowButton = levelSelect.GetChild(nowLevel - 1);
        nowLevel++;
        Transform nextButton = levelSelect.GetChild(nowLevel - 1);
        nowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        ChangeInteractive();
    }
}
