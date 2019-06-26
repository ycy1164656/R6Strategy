using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour {

 
    float horizontalSpeed = 20.0f;
    float verticalSpeed = 20.0f;
    public string objCreateTime;
    //  public GameObject houseMap;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //电脑操作
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetMouseButton(0))
            {

                float h = horizontalSpeed * Input.GetAxis("Mouse X");
                float v = verticalSpeed * Input.GetAxis("Mouse Y");
                transform.Rotate(0, -h, 0, Space.World);
                transform.Rotate(v, 0, 0, Space.World);

            }
        }


            if (Input.touchCount <= 0)
            {
                return;
            }

            //单点触摸， 水平上下旋转  
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;

                transform.Rotate(Vector3.right * (deltaPos.y * 0.2f), Space.World);

            transform.Rotate(Vector3.down * (deltaPos.x * 0.2f), Space.World);

                //          transform.Rotate (deltaPos.y * 0.2f, 0, 0, Space.World);
                //          transform.Rotate (0, -deltaPos.x * 0.2f, 0, Space.Self);

                //          Debug.Log (transform.forward);
                //          transform.Rotate(new Vector3(0,-deltaPos.x,0));
                //          transform.Rotate (new Vector3 (deltaPos.y, 0, 0));

            }

            

    }
}
