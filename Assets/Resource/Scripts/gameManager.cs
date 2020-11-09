using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObject JigsawBoard;

    float size;
    Camera cam;
    GameObject pickupItem;
    Vector3 lastPickupPos;
    float maxRayDistance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        size = grid.cellSize.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (pickupItem==null)
            {
                Physics.Raycast(ray, out hit, maxRayDistance, 1 << LayerMask.NameToLayer("block"));
                if (hit.collider != null)
                {
                    pickupBlock(hit.collider.gameObject);
                    pickupItem = hit.collider.gameObject;
                    // 如果在拼图面板，则需要恢复原来的点集
                    if (pickupItem.GetComponent<blockController>().isAtJigsawBoard)
                    {
                        AddPointToList(pickupItem, JigsawBoard);
                        pickupItem.GetComponent<blockController>().isAtJigsawBoard = false;
                    }
                }
            }
            else
            {
                Physics.Raycast(ray, out hit, maxRayDistance, 1 << LayerMask.NameToLayer("jigsawBoard"));
                if (hit.collider != null && CanDrop(pickupItem, hit.collider.gameObject))
                {
                    dropBlock(pickupItem);
                    pickupItem.transform.position = hit.collider.gameObject.transform.position + InWhichBoardArea(pickupItem.transform.position, hit.collider.gameObject.transform.position);
                    DeletePointFromList(pickupItem, hit.collider.gameObject);
                    pickupItem.GetComponent<blockController>().isAtJigsawBoard = true;
                    pickupItem = null;
                }
                else if (hit.collider == null)
                {
                    dropBlock(pickupItem);
                    pickupItem.transform.position = pickupItem.GetComponent<blockController>().initBlockPos;
                    pickupItem = null;
                }
            }
            
        }
    }

    void pickupBlock(GameObject block)
    {
        block.GetComponent<blockController>().isPickedup = true;
        block.GetComponent<Collider>().enabled = false;
    }

    void dropBlock(GameObject block)
    {
        block.GetComponent<blockController>().isPickedup = false;
        block.GetComponent<Collider>().enabled = true;
    }

    bool CanDrop(GameObject dropItem, GameObject dropBoard)
    {
        List<Vector2> rpLst = dropBoard.GetComponent<relativePosList>().relativePosLst;
        Transform[] transforms = dropItem.GetComponentsInChildren<Transform>();
        Vector3 center = dropBoard.transform.position;
        for (int i = 1; i < transforms.Length; i++)
        {
            if (!rpLst.Contains(InWhichBoardArea(transforms[i].position, center)))
            {
                return false;
            }
        }
        Debug.Log("can drop");
        return true;
    }

    void DeletePointFromList(GameObject dropItem, GameObject dropBoard)
    {
        List<Vector2> rpLst = dropBoard.GetComponent<relativePosList>().relativePosLst;
        Transform[] transforms = dropItem.GetComponentsInChildren<Transform>();
        Vector3 center = dropBoard.transform.position;
        for (int i = 1; i < transforms.Length; i++)
        {
            rpLst.Remove(InWhichBoardArea(transforms[i].position, center));
        }
    }

    void AddPointToList(GameObject dropItem, GameObject dropBoard)
    {
        List<Vector2> rpLst = dropBoard.GetComponent<relativePosList>().relativePosLst;
        Transform[] transforms = dropItem.GetComponentsInChildren<Transform>();
        Vector3 center = dropBoard.transform.position;
        for (int i = 1; i < transforms.Length; i++)
        {
            rpLst.Add(InWhichBoardArea(transforms[i].position, center));
        }
    }

    /// <summary>
    /// 计算在面板哪块区域
    /// </summary>
    /// <param name="pos">点的世界坐标</param>
    /// <param name="boardCenter">面板的中心世界坐标</param>
    /// <returns>点所在区域相对面板中心的相对坐标</returns>
    Vector3 InWhichBoardArea(Vector3 pos, Vector3 boardCenter)
    {
        float x = Mathf.Abs(pos.x - boardCenter.x), y = Mathf.Abs(pos.y - boardCenter.y);
        int sgnX = (pos.x - boardCenter.x) > 0 ? 1 : -1;
        int sgnY = (pos.y - boardCenter.y) > 0 ? 1 : -1;
        x = sgnX * size / 2 * (Mathf.Ceil(x / size) * 2 - 1);
        y = sgnY * size / 2 * (Mathf.Ceil(y / size) * 2 - 1);

        return new Vector3(x, y);
    }
}
