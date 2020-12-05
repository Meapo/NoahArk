using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [Tooltip("关卡加一")]
    public int level;
    public void ClickToLoadScene()
    {
        SceneManager.LoadSceneAsync(level);
    }
    
}
