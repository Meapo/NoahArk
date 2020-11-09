using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockController : MonoBehaviour
{
    public bool isPickedup;

    public bool isAtJigsawBoard;

    public Vector3 initBlockPos;

    [SerializeField]
    [Tooltip("方块种类")]
    private blockType type;


    // Start is called before the first frame update
    void Start()
    {
        initBlockPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedup)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rotate();
            }
        }
    }

    void Rotate()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        Vector3 center = transforms[0].position;
        for (int i = 1; i < transforms.Length; i++)
        {
            Vector3 offset = transforms[i].position - center;
            float x = center.x - offset.y;
            float y = center.y + offset.x;
            transforms[i].position = new Vector3(x, y);
        }
    }
}
