using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
    public void ClickToRestart()
    {
        int sceneLayer = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(sceneLayer);
    }
}
