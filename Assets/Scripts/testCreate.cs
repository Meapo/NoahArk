using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCreate : MonoBehaviour
{
    #region 可视变量
    [SerializeField]
    [Tooltip("是否为编辑模式")]
    private bool isEditor;

    [SerializeField]
    [Tooltip("对应方格")]
    private Grid grid;


    [SerializeField]
    [Tooltip("黑方块")]
    private GameObject blackBlock;
    #endregion

    #region 成员变量
    relativePosList posInstance;
    static Dictionary<Vector2, GameObject> relativePosMap;
    Camera cam;
    float size;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (!isEditor)
        {
            posInstance = relativePosList.instance;
            posInstance.SetValue();
            Destroy(this);
            return;
        }
        relativePosMap = new Dictionary<Vector2, GameObject>();
        cam = Camera.main;
        if (grid!=null)
        {
            size = grid.cellSize.x;
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 center = InWhichArea(hit.point);
            // Debug.Log(center.x.ToString("f3") + ", " + center.y.ToString("f3"));
            if (!relativePosMap.ContainsKey(center))
            {
                // 添加图片
                GameObject gameObject = Instantiate<GameObject>(blackBlock, transform);
                gameObject.transform.localPosition = center;
                // 将点添加到set
                relativePosMap.Add(center, gameObject);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 center = InWhichArea(hit.point);
            if (relativePosMap.ContainsKey(center))
            {
                // 删除图片
                Destroy(relativePosMap[center]);
                // 删除set中的点
                relativePosMap.Remove(center);
            }
        }
    }

    Vector3 InWhichArea(Vector3 pos)
    {
        float x = Mathf.Abs(pos.x - transform.position.x), y = Mathf.Abs(pos.y - transform.position.y);
        int sgnX = (pos.x - transform.position.x) > 0 ? 1 : -1;
        int sgnY = (pos.y - transform.position.y) > 0 ? 1 : -1;
        x = sgnX * size / 2 * (Mathf.Ceil(x / size) * 2 - 1);
        y = sgnY * size / 2 * (Mathf.Ceil(y / size) * 2 - 1);

        return new Vector3(x, y);
    }
}
