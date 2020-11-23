using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [SerializeField]
    [Tooltip("拼图板")]
    public GameObject JigsawBoard;

    [SerializeField]
    [Tooltip("放置板")]
    public GameObject pickupBoard;

    // 方格大小
    float size;
    // main camera
    Camera cam;
    // 当前拾取方块
    GameObject pickupItem;
    // 射线检测最远距离，不加距离的话layer过滤容易失效（巨坑）
    float maxRayDistance = 100f;
    // layer mask
    LayerMask jigsawBoardMask, pickupBoardMask, blockBoardMask;

    Operation operation;

    MoveInf move;

    MouseInf mouse;
    public static gameManager instance;
    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        size = grid.cellSize.x;
        jigsawBoardMask = 1 << LayerMask.NameToLayer("jigsawBoard");
        pickupBoardMask = 1 << LayerMask.NameToLayer("pickupBoard");
        blockBoardMask = 1 << LayerMask.NameToLayer("block");

        mouse = MouseInf.GetInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (pickupItem==null)
            {
                // 提取拼图
                Physics.Raycast(ray, out hit, maxRayDistance, blockBoardMask);
                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject.GetComponentInParent<blockController>().gameObject;
                    pickupBlock(hitObject);
                    pickupItem = hitObject;

                    Operation.CreateNewInstance();
                    operation = Operation.GetInstance();
                    move = new MoveInf();
                    move.blockName = pickupItem.name;
                    move.sourPos = pickupItem.transform.position;
                    move.sourRoa = pickupItem.transform.eulerAngles;
                    move.isChanged = false;
                    Undo.instance.AddOperation(operation);
                    // 如果在拼图面板，则需要恢复原来的点集
                    if (pickupItem.GetComponent<blockController>().isAtJigsawBoard)
                    {
                        AddPointToList(pickupItem, JigsawBoard);
                        pickupItem.GetComponent<blockController>().isAtJigsawBoard = false;
                        move.sourBoard = JigsawBoard;
                    }
                    else 
                    {
                        move.sourBoard = JigsawBoard;
                    }
                }
            }
            else
            {
                // 放置拼图
                Physics.Raycast(ray, out hit, maxRayDistance, jigsawBoardMask | pickupBoardMask);
                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (CanDrop(pickupItem, hitObject))
                    {
                        dropBlock(pickupItem);
                        pickupItem.transform.position = hitObject.transform.position + InWhichBoardArea(pickupItem.transform.position, hitObject.transform.position);
                        if (hitObject.layer==LayerMask.NameToLayer("jigsawBoard"))
                        {
                            DeletePointFromList(pickupItem, hitObject);
                            pickupItem.GetComponent<blockController>().isAtJigsawBoard = true;
                            mouse.isRelay = true;
                        }
                        move.endBoard = hitObject;
                        operation.AddMoveInf(move);
                        pickupItem = null;
                    }
                }
                //else
                //{
                //    dropBlock(pickupItem);
                //    pickupItem.transform.position = pickupItem.GetComponent<blockController>().initBlockPos;
                //    pickupItem = null;
                //}
            }
            
        }
    }

    void pickupBlock(GameObject block)
    {
        block.GetComponent<blockController>().isPickedup = true;
        foreach (var item in block.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }
    }

    void dropBlock(GameObject block)
    {
        block.GetComponent<blockController>().isPickedup = false;
        foreach (var item in block.GetComponentsInChildren<Collider>())
        {
            item.enabled = true;
        }
    }

    bool CanDrop(GameObject dropItem, GameObject dropBoard)
    {
        Transform[] transforms = dropItem.GetComponentsInChildren<Transform>();
        if (dropBoard.layer == LayerMask.NameToLayer("jigsawBoard"))
        {
            List<Vector2> rpLst = dropBoard.GetComponent<relativePosList>().relativePosLst;
            Vector3 center = dropBoard.transform.position;
            for (int i = 1; i < transforms.Length; i++)
            {
                if (!rpLst.Contains(InWhichBoardArea(transforms[i].position, center)))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            BoxCollider boxCollider = dropBoard.GetComponent<BoxCollider>();
            for (int i = 1; i < transforms.Length; i++)
            {
                if (!boxCollider.bounds.Contains(transforms[i].position))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void DeletePointFromList(GameObject dropItem, GameObject dropBoard)
    {
        List<Vector2> rpLst = dropBoard.GetComponent<relativePosList>().relativePosLst;
        Transform[] transforms = dropItem.GetComponentsInChildren<Transform>();
        Vector3 center = dropBoard.transform.position;
        for (int i = 1; i < transforms.Length; i++)
        {
            rpLst.Remove(InWhichBoardArea(transforms[i].position, center));
        }
    }

    public void AddPointToList(GameObject dropItem, GameObject dropBoard)
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
    /// <returns>点所在区域的中心点相对面板中心的相对坐标</returns>
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
