using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class relativePosList : MonoBehaviour
{
    public List<Vector2> relativePosLst;

    static public relativePosList instance;

    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public void SetValue()
    {
        relativePosLst = new List<Vector2>();
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            relativePosLst.Add(transforms[i].position - transform.position);
        }
    }


}
