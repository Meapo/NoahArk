using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandbookBt : MonoBehaviour
{
    public GameObject handbook;
    public GameObject pickupBoard;

    public GameObject bg;

    blockController[] blockControllers;

    SpriteRenderer renderer1;

    MouseInf mouse;
    /// <summary>
    /// 用于保持放置板状态的实时更新
    /// </summary>
    bool onClick;
    float a;
    private void Start()
    {
        blockControllers = pickupBoard.GetComponentsInChildren<blockController>();
        renderer1 = bg.GetComponent<SpriteRenderer>();
        a = renderer1.color.a;
        mouse = MouseInf.GetInstance();
    }
    public void Update()
    {
        if(onClick && mouse.isRelay)
        {

            blockControllers = pickupBoard.GetComponentsInChildren<blockController>();
            foreach (blockController controller in blockControllers)
            {
                if (!controller.isAtJigsawBoard)
                {
                    SpriteRenderer[] renderers = controller.gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer renderer in renderers)
                    {
                        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
                    }
                }
            }
        }
    }
    private void LateUpdate()
    {
        mouse.isRelay = false;
    }
    public void ClickToLoadHandbook()
    {
        if (handbook.activeSelf)
        {
            renderer1.color = new Color(renderer1.color.r, renderer1.color.g, renderer1.color.b, a);
            foreach (blockController controller in blockControllers)
            {
                if (!controller.isAtJigsawBoard)
                {
                    SpriteRenderer[] renderers = controller.gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer renderer in renderers)
                    {
                        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                    }
                }
            }
            onClick = false;
        }
        else
        {
            renderer1.color = new Color(renderer1.color.r, renderer1.color.g, renderer1.color.b, 0);
            foreach (blockController controller in blockControllers)
            {
                if (!controller.isAtJigsawBoard)
                {
                    SpriteRenderer[] renderers = controller.gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer renderer in renderers)
                    {
                        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
                    }
                }
            }
            onClick = true;
        }
        handbook.SetActive(!handbook.activeSelf);
    }
}
