using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscControlUI : MonoBehaviour
{
    public GameObject UI;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UI.SetActive(!UI.activeSelf);
        }
    }
}
