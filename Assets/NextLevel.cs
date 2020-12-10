using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour
{
    Button button;
    int nowLevel;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        nowLevel = SceneManager.GetActiveScene().buildIndex;
        if (nowLevel+1>SceneManager.sceneCount)
        {
            button.interactable = false;
        }
    }
    public void ClickToLoadNextLevel()
    {
        SceneManager.LoadSceneAsync(nowLevel + 1);
    }
}
