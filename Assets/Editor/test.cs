using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class test : MonoBehaviour
{
    static test()
    {
        EditorApplication.update += Update;
    }

    #region 可视变量
    static public Grid grid;

    #endregion

    #region 成员变量
    static List<Vector2> posList;

    static Camera cam;

    static float size;
    #endregion

    private void Start()
    {
        posList = new List<Vector2>();
        cam = Camera.main;
        size = grid.cellSize.x;
    }


    // Update is called once per frame
    static void Update()
    {
        
    }
    private void OnGUI()
    {
        Debug.Log(Event.current);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Ray ray = test.cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("board"));
            Vector3 relativePos = hit.point;
            Vector3 center = InWhichArea(relativePos);
            Debug.Log(center);
        }
    }



    static Vector3 InWhichArea(Vector3 pos)
    {
        float x = Mathf.Abs(pos.x), y  = Mathf.Abs(pos.y);
        int sgnX = pos.x > 0 ? 1 : -1;
        int sgnY = pos.y > 0 ? 1 : -1;
        x = sgnX * size / 2 * Mathf.Ceil(x / size);
        y = sgnY * size / 2 * Mathf.Ceil(y / size);

        return new Vector3(x, y);
    }
}
