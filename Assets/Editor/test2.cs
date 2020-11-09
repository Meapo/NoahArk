using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    #region 可视变量
    //static public Grid grid;
    public Transform point1;
    public Transform point2;
    #endregion

    #region 成员变量
    List<Vector2> posList;

    Camera cam;

    float size;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        posList = new List<Vector2>();
        cam = Camera.main;
        //size = grid.cellSize.x;
        size = Mathf.Abs(point1.position.x - point2.position.x);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Debug.Log(Event.current.type);
        //Ray ray = cam.ScreenPointToRay(Event.current.mousePosition);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer("board"));
        //if (hit.collider==null)
        //{
        //    Debug.Log("No hit");
        //}
        //Vector3 relativePos = hit.point - transform.position;
        //Vector3 center = InWhichArea(relativePos) + transform.position;
    }

    Vector3 InWhichArea(Vector3 pos)
    {
        float x = Mathf.Abs(pos.x), y = Mathf.Abs(pos.y);
        int sgnX = pos.x > 0 ? 1 : -1;
        int sgnY = pos.y > 0 ? 1 : -1;
        x = sgnX * size / 2 * Mathf.Ceil(x / size);
        y = sgnY * size / 2 * Mathf.Ceil(y / size);

        return new Vector3(x, y);
    }
}
