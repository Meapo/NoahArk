using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchLevelBt : MonoBehaviour
{
    //起始X位置，该值为第一关的背景图x坐标，它是最大的，之后移动要减
    public int BGBeginX;

    public RectTransform BGImage;

    public Transform levelSelect;

    public Button upButton;

    public Button downButton;
    [SerializeField]
    private int levelNum;
    [SerializeField]
    private int moveDir; 
    GameInfo gameInfo;
    int nowLevel;
    [Tooltip("移动到终点所需要的次数，每帧移动一次")]
    public int moveTimes;
    public int needTimes;
    float movDis;

    [SerializeField]
    int movCnt;
    // Start is called before the first frame update
    void Start()
    {
        gameInfo = GameInfo.GetInstance();
        nowLevel = gameInfo.nowLevel;
        for (int i = 0; i < levelSelect.childCount; i++)
        {
            Transform trans = levelSelect.GetChild(i);
            if (i == nowLevel - 1)
            {
                trans.gameObject.SetActive(true);
            }
            else
            {
                trans.gameObject.SetActive(false);
            }
        }
        ChangeInteractive();
        movDis = BGImage.sizeDelta.x / 4/moveTimes;
        moveDir = 0;
        movCnt = 0;
        needTimes = 0;
    }
    private void FixedUpdate()
    {
        if(movCnt<needTimes)
        {
            BGImage.Translate(new Vector3(moveDir * movDis, 0));
            movCnt++;
        }
        if(movCnt==needTimes)
        {
            moveDir = 1;
        }
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
        Transform nowButton = levelSelect.GetChild(nowLevel - 1);
        nowLevel--;
        Transform nextButton = levelSelect.GetChild(nowLevel - 1);
        nowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        ChangeInteractive();
        
        if (moveDir >= 0)
        {
            needTimes += moveTimes;
        }
        else
        {
            needTimes -= moveTimes;
            if (needTimes < movCnt)
            {
                needTimes = 2 * movCnt - needTimes;
                moveDir = -moveDir;
            }
        }
    }
    public void ClickToUp()
    {
        Transform nowButton = levelSelect.GetChild(nowLevel - 1);
        nowLevel++;
        Transform nextButton = levelSelect.GetChild(nowLevel - 1);
        nowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        ChangeInteractive();
        if (moveDir <= 0)
        {
            needTimes += moveTimes;
            moveDir = -1;
        }
        else
        {
            needTimes -= moveTimes;
            if (needTimes < movCnt)
            {
                needTimes = 2 * movCnt - needTimes;
                moveDir = -moveDir;

            }
        }
    }

}
