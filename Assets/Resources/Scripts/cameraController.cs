using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("屏幕移动速度")]
    private float screenMoveSpeed = 1f;

    [SerializeField]
    private float judgeValue = 0.1f;

    [SerializeField]
    [Tooltip("背景左下边界点")]
    private Transform leftDown;

    [SerializeField]
    [Tooltip("背景右上边界点")]
    private Transform rightTop;

    Camera cam;

    float defaultCamSize;

    float width, height;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultCamSize = cam.orthographicSize;
        width = rightTop.position.x - leftDown.position.x;
        height = rightTop.position.y - leftDown.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseInput!=0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + defaultCamSize * mouseInput, 0.3f*defaultCamSize, defaultCamSize);
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(2))
        {
            Vector3 moveDir = new Vector3(-mouseX, -mouseY);
            cam.transform.position = cam.transform.position + moveDir * screenMoveSpeed * Time.deltaTime;
        }
        Vector3 pos = cam.transform.position;
        float tempX = width / 2 * (1 - cam.orthographicSize / defaultCamSize);
        float tempY = height / 2 * (1 - cam.orthographicSize / defaultCamSize);
        pos.x = Mathf.Clamp(pos.x, -tempX, tempX);
        pos.y = Mathf.Clamp(pos.y, -tempY, tempY);
        cam.transform.position = pos;
    }

    //Vector3 isAtBorder(Vector3 mouse)
    //{
    //    Vector3 result = Vector3.zero;
    //    if (mouse.x < judgeValue)
    //    {
    //        result.x = -1f;
    //    }
    //    else if (mouse.x > (Screen.width - judgeValue))
    //    {
    //        result.x = 1f;
    //    }
    //    if (mouse.y < judgeValue)
    //    {
    //        result.y = -1f;
    //    }
    //    else if (mouse.x > (Screen.height - judgeValue))
    //    {
    //        result.y = 1f;
    //    }
    //    return result;
    //}

    private void OnMouseDrag()
    {
        
    }
}
