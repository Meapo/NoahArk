using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("关卡开始播放界面")]
    private GameObject startPanel;

    [SerializeField]
    [Tooltip("关卡开始播放时间")]
    private float playTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(stopPlay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator stopPlay()
    {
        yield return new WaitForSeconds(playTime);
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }
    }
}
