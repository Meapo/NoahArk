using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [Tooltip("拼图板")]
    public GameObject JigsawBoard;

    [Tooltip("放置板")]
    public GameObject pickupBoard;

    [SerializeField]
    [Tooltip("结束界面")]
    private GameObject endPanel;

    [SerializeField]
    [Tooltip("分数txt")]
    private Text gradeText;

    [SerializeField]
    [Tooltip("结束评分界面")]
    private GameObject endUI;

    [SerializeField]
    [Tooltip("评分图")]
    private Image levelImage;

   
    // 方格大小
    float size;
    // main camera
    Camera cam;
    // 当前拾取方块
    GameObject pickupItem;
    // 射线检测最远距离，不加距离的话layer过滤容易失效（巨坑）
    float maxRayDistance = 100f;
    // layer mask
    LayerMask jigsawBoardMask, pickupBoardMask, blockBoardMask, UILayerMask;

    Operation operation;

    MoveInf move;

    MouseInf mouse;

    GradeLevel curLevel;

    bool isMouseLeftDown = false;
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
        UILayerMask = 1 << LayerMask.NameToLayer("UI");
        mouse = MouseInf.GetInstance();

        endPanel.SetActive(false);
    }

    // Update is called once per frame


    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isMouseLeftDown = true;
        }
    }
    void FixedUpdate()
    {
        if (isMouseLeftDown)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (pickupItem==null)
            {
                // 提取拼图
                Physics.Raycast(ray, out hit, maxRayDistance, blockBoardMask | UILayerMask);
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
                Physics.Raycast(ray, out hit, maxRayDistance, jigsawBoardMask | pickupBoardMask | UILayerMask);
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
                        }
                        mouse.isRelay = true;
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
            isMouseLeftDown = false;
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

    public void OnClickEndButton()
    {
        endPanel.SetActive(true);
        OpeningUI openingUI = OpeningUI.GetInstance();
        openingUI.UICanCloseByESC = endPanel;
    } 

    public void OnClickConfirmButton()
    {
        // 存档

        // 读取下一关
        int blockNum, blockAtJigsawBoardCount = 0;
        blockController[] controllers = pickupBoard.GetComponentsInChildren<blockController>();
        blockNum = controllers.Length;
        foreach (var controller in controllers)
        {
            if (controller.isAtJigsawBoard)
            {
                blockAtJigsawBoardCount++;
            }
        }
        string path = "UI/level";
        if (blockAtJigsawBoardCount == blockNum)
        {
            curLevel = GradeLevel.greate;
        }
        else if (blockAtJigsawBoardCount >= blockNum * 2 / 3)
        {
            curLevel = GradeLevel.good;
        }
        else
        {
            curLevel = GradeLevel.bad;
        }
        path += curLevel;
        levelImage.sprite = Resources.Load<Sprite>(path);
        gradeText.text = GetGradeLevelString(curLevel);
        endUI.SetActive(true);
        endPanel.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public string GetGradeLevelString(GradeLevel gradeLevel)
    {
        switch(gradeLevel)
        {
            case GradeLevel.bad:
                return "不太好！";
            case GradeLevel.good:
                return "还不错！";
            case GradeLevel.greate:
                return "好极了！";
        }
        return "Invalid Grade Level!";
    }

    public void OnClickCancelButton()
    {
        endPanel.SetActive(false);
        OpeningUI openingUI = OpeningUI.GetInstance();
        openingUI.UICanCloseByESC = null;
    }
}
