using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameReady : MonoBehaviour
{
    public GameObject UI;
    public GameObject Text;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!UI.activeSelf)
                UI.SetActive(true);
            if (Text.activeSelf)
                Text.SetActive(false);
        }
    }
}
