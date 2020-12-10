using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelpManager : MonoBehaviour
{
    public GameObject pickupBoard;
    private blockController mouseController;
    public bool isEnd;
    public TeachState teachState;
    public TextAsset txt;
    private string[] lines;
    private int lineInd;
    public GameObject godImage;
    public GameObject noahImage;
    public GameObject catImage;
    public Text text;
    public GameObject textImg;
    public GameObject panel;
    private bool isStartShowEnd;
    // Start is called before the first frame update
    void Start()
    {
        lines = txt.text.Split('\n');
        lineInd = 0;
        isStartShowEnd = false;
        teachState = TeachState.NotTeaching;
        StartCoroutine(startShow(lines[lineInd++]));
    }

    // Update is called once per frame
    void Update()
    {
        blockController[] Controllers = pickupBoard.GetComponentsInChildren<blockController>();
        foreach (var controller in Controllers)
        {
            if (controller.type == blockType.mouse)
            {
                mouseController = controller;
            }
        }
        switch (teachState)
        {
            case TeachState.NotTeaching:
                if (Input.anyKeyDown && isStartShowEnd)
                {
                    if (lineInd != lines.Length)
                    {
                        ShowLines(lines[lineInd++]);
                    }
                    else
                    {
                        isEnd = true;
                        panel.SetActive(false);
                    }
                }
                break;
            case TeachState.TeachingCatScareMouse:
                if (mouseController.isChanged)
                {
                    teachState = TeachState.NotTeaching;
                    panel.SetActive(true);
                    if (lineInd != lines.Length)
                    {
                        ShowLines(lines[lineInd++]);
                    }
                    else
                    {
                        isEnd = true;
                        panel.SetActive(false);
                    }
                }
                break;
            case TeachState.TeachingMouseAvoidCat:
                if (mouseController.isAtJigsawBoard)
                {
                    teachState = TeachState.NotTeaching;
                    panel.SetActive(true);
                    if (lineInd != lines.Length)
                    {
                        ShowLines(lines[lineInd++]);
                    }
                    else
                    {
                        isEnd = true;
                        panel.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }

    void ShowLines(string str)
    {
        if (str=="")
        {
            return;
        }
        if (str.Substring(0, 2) == "诺亚")
        {
            godImage.SetActive(false);
            noahImage.SetActive(true);
            catImage.SetActive(false);

        }
        else if (str.Substring(0, 2) == "上帝")
        {
            godImage.SetActive(true);
            noahImage.SetActive(false);
            catImage.SetActive(false);
        }
        else if (str.Substring(0, 1) == "猫")
        {
            godImage.SetActive(false);
            noahImage.SetActive(false);
            catImage.SetActive(true);
        }
        else if (str.Substring(0, 3) == "Sta")
        {
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 180f/255f);
            ShowLines(lines[lineInd++]);
            return;        
        }
        else if (str.Substring(0, 3) == "C&M")
        {
            Teach(TeachState.TeachingCatScareMouse);
        }
        else if (str.Substring(0, 3) == "C|M")
        {
            Teach(TeachState.TeachingMouseAvoidCat);
        }
        showString(str);
    }
    
    void Teach(TeachState state)
    {
        teachState = state;
        panel.SetActive(false);
    }

    void showString(string str)
    {
        text.text = str;
    }

    IEnumerator startShow(string str)
    {
        panel.SetActive(true);
        textImg.SetActive(false);
        panel.GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(1f);
        textImg.SetActive(true);
        ShowLines(str);
        isStartShowEnd = true;
    }
}
