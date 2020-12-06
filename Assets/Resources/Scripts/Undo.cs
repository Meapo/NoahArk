using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo : MonoBehaviour
{ 
    public static Undo instance;

    public Stack<Operation> operations = new Stack<Operation>();

    public GameObject relayBoard;

    gameManager manager;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        manager = gameManager.instance;
    }
    public void AddOperation(Operation operation)
    {
        operations.Push(operation);
    }
    private Undo() { }
    public void ClickToUndo()
    {
        if(operations.Count>0)
        {
            Operation operation = operations.Pop();

            while(operation.operation.Count>0)
            {
                MoveInf move = operation.operation.Pop();
                UndoMove(move);
            }
        }
    }
    void UndoMove(MoveInf move)
    {
        GameObject block;
        if(move.isChanged)
        {
            GameObject preBlock = relayBoard.transform.Find(move.blockName+'_').gameObject;
            Vector3 iniPos = preBlock.GetComponent<blockController>().initBlockPos;
            GameObject blockPrefabs = (GameObject)Resources.Load("Prefabs/AnimalBlock/" + move.blockName);
            block = GameObject.Instantiate(blockPrefabs, iniPos, Quaternion.identity, preBlock.transform.parent);
            GameObject.Destroy(preBlock);
            block.GetComponent<blockController>().isChanged = false;
            block.name = move.blockName;
        }
        else
        {
            block = relayBoard.transform.Find(move.blockName).gameObject;
        }
        blockController controller = block.GetComponent<blockController>();
        if(move.endBoard==manager.JigsawBoard)
        {
            manager.AddPointToList(block, manager.JigsawBoard);
            controller.isAtJigsawBoard = false;
        }
        block.transform.position = move.sourPos;
        block.transform.eulerAngles = move.sourRoa;
        if(move.sourBoard==manager.JigsawBoard)
        {
            manager.DeletePointFromList(block, manager.JigsawBoard);
            controller.isAtJigsawBoard = true;
        }
    }
}
