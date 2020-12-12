using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoveRenderer : MonoBehaviour
{
    public int dir;
    public float speed;
    public float endPosX;
    float jmpDis;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        jmpDis = renderer.bounds.extents.x * 4;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform trans in transform)
        {
            trans.Translate(new Vector3(speed * Time.deltaTime * dir, 0, 0),Space.World);
            if(dir*(trans.position.x-endPosX)>0)
            {
                trans.Translate(new Vector3(-jmpDis * dir ,0, 0),Space.World);
            }
        }
    }
}
