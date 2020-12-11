using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloat : MonoBehaviour
{
    public float moveSpeed;
    int[, ] dir = new int[,] { { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };
    int cnt = 0;
    public int movCnt;
    int index;
    private void Start()
    {
        cnt = 0;
        index = 0;
    }
    private void FixedUpdate()
    {
        Vector2 movDir;
        movDir.x = Random.value*dir[index,0];
        movDir.y = Random.value*dir[index,1];
        transform.Translate(movDir * moveSpeed * Time.deltaTime);
        cnt++;
        if(cnt>=movCnt)
        {
            cnt = 0;
            index = (index + 1) % 4;
        }
    }
}
