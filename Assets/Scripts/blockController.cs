using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class blockController : MonoBehaviour
{
    public bool isPickedup;

    public bool isAtJigsawBoard;

    public bool isChanged;

    public Vector3 initBlockPos;

    [Tooltip("方块种类")]
    public blockType type;

    LayerMask blockLayerMask;

    LayerMask UILayerMask;

    public GameObject changeBlock;

    gameManager gameManagerInstance;

    MouseInf mouse;


    private void Awake()
    {
        initBlockPos = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManagerInstance = gameManager.instance;
        blockLayerMask = 1 << LayerMask.NameToLayer("block");
        mouse = MouseInf.GetInstance();
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
        if (mouse.isRelay)
        {
            if (isAtJigsawBoard)
            {
                Transform[] transforms = GetComponentsInChildren<Transform>();
                //Vector3 center = transforms[0].position;
                for (int i = 1; i < transforms.Length; i++)
                {
                    if (NextAreaDetect(transforms[i].position))
                    {
                        /*if (!isChanged)
                        {
                            ChangeBlock();
                        }
                        Withdraw();*/

                        #region codeChanged
                        if (!isChanged)
                        {
                            ChangeBlock();
                            break;
                        }
                        else
                        {
                            SaveMove(false,name);
                            Withdraw();
                            break;
                        }
                        #endregion
                    }
                }
            }
        }
    
    }
    private void LateUpdate()
    {
        mouse.isRelay = false;
    }
    void Rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, -90f), Space.Self);
    }

    // 对四个方向的相邻方块进行射线检测
    bool NextAreaDetect(Vector3 point)
    {
        Vector3[] vectors = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Physics.Raycast(point, vectors[i], out hit, gameObject.GetComponentInChildren<BoxCollider>().size.x, blockLayerMask);
            if (hit.collider!=null)
            {
                blockType hitBlockType = hit.collider.gameObject.GetComponentInParent<blockController>().type;
                if (this.type < blockType.elephant)
                {
                    if (this.type == hitBlockType - 1)
                    {
                        return true;
                    }
                }
                else
                {
                    if (hitBlockType==blockType.mouse)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // 碰到相克的方块后撤退
    void ChangeBlock()
    {
        GameObject newObject =  Instantiate<GameObject>(changeBlock, initBlockPos, Quaternion.identity,transform.parent);
        string partten = @"\s.*";
        MatchCollection match = Regex.Matches(name,partten);
        string order=null;
        if (match.Count>0)
        {
            order = match[0].ToString();
        }
        newObject.name = changeBlock.name+order;
        gameManagerInstance.AddPointToList(gameObject, gameManagerInstance.JigsawBoard);
        Destroy(this.gameObject);
        SaveMove(true,newObject.name);
    }

    void Withdraw()
    {
        gameManagerInstance.AddPointToList(gameObject, gameManagerInstance.JigsawBoard);
        transform.position = initBlockPos;
    }

    /// <summary>
    /// 将方块撤回的移动信息存入operation中
    /// </summary>
    void SaveMove(bool isChanged,string name)
    {
        Operation operation = Operation.GetInstance();
        MoveInf move = new MoveInf();
        move.blockName = name;
        move.endBoard = gameManagerInstance.pickupBoard;
        move.sourBoard = gameManagerInstance.JigsawBoard;
        move.sourPos = transform.position;
        move.sourRoa = transform.eulerAngles;
        move.isChanged = isChanged;
        operation.AddMoveInf(move);
    }
}
