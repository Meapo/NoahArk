using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameReady : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
