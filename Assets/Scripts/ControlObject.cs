using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlObject : MonoBehaviour {
	private Touch oldTouch1;  //上次触摸点1(手指1)  
	private Touch oldTouch2;  //上次触摸点2(手指2) 
	float horizontalSpeed = 8.0f;
	float verticalSpeed = 8.0f;
    public GameObject controllerObj;
    Quaternion oldRotation;
    Vector3 oldScale;
  //	public GameObject houseMap;

	// Use this for initialization
	void Start () {
         
        controllerObj.GetComponent<CommonControl>().scaleChange = 1f;
	}

    // Update is called once per frame
    void Update()
    {
        //没有触摸  
        if (controllerObj.GetComponent<CommonControl>().canDrawLine == false && controllerObj.GetComponent<CommonControl>().canAddOperation == false && controllerObj.GetComponent<CommonControl>().arrowControl == false && controllerObj.GetComponent<CommonControl>().commentShow == false && controllerObj.GetComponent<CommonControl>().selectionPanel.activeSelf == false)
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("当前触摸在UI上");
                return;

            }
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                //电脑操作
                if (Input.GetAxis("Mouse ScrollWheel") != 0f)//这个是鼠标滚轮响应函数
                {
                    //滚轮响应一次就让scale自增或自减，注意，滚轮函数是有返回值的，
                    //返回是float型的！这个由滚轮向前（正数）还是向后（负数）滚决定
                    transform.localScale *= (1 + Input.GetAxis("Mouse ScrollWheel"));//改变物体大小
                    controllerObj.GetComponent<CommonControl>().scaleChange *= (1 + Input.GetAxis("Mouse ScrollWheel"));
                }

                if (Input.GetMouseButton(0))
                {

                    float h = horizontalSpeed * Input.GetAxis("Mouse X");
                    float v = verticalSpeed * Input.GetAxis("Mouse Y");
                    transform.Rotate(0, -h, 0, Space.Self);
                    transform.Rotate(v, 0, 0, Space.World);

                }
            }
               


                if (oldRotation != transform.rotation || oldScale != transform.localScale)
            {
                oldRotation = transform.rotation;
                oldScale = transform.localScale;
                if (controllerObj.GetComponent<DpScanMap>().followed)
                {
                    ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
                    h.Add("rotation", JsonUtility.ToJson(oldRotation));
                    h.Add("scale", JsonUtility.ToJson(oldScale));
                    DataObj.lbc.LocalPlayer.SetCustomProperties(h);
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

                transform.Rotate(Vector3.down * (deltaPos.x * 0.2f), Space.Self);

                //			transform.Rotate (deltaPos.y * 0.2f, 0, 0, Space.World);
                //			transform.Rotate (0, -deltaPos.x * 0.2f, 0, Space.Self);

                //			Debug.Log (transform.forward);
                // 			transform.Rotate(new Vector3(0,-deltaPos.x,0));
                // 			transform.Rotate (new Vector3 (deltaPos.y, 0, 0));

            }

            //多点触摸, 放大缩小  
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            //第2点刚开始接触屏幕, 只记录，不做处理  
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            //两个距离之差，为正表示放大手势， 为负表示缩小手势  
            float offset = newDistance - oldDistance;

            //放大因子， 一个像素按 0.01倍来算(100可调整)  
            float scaleFactor = offset / 200f;
            //		Vector3 localScale = transform.localScale;  
            //		Vector3 scale = new Vector3(localScale.x + scaleFactor,  
            //			localScale.y + scaleFactor,   
            //			localScale.z + scaleFactor);  

            //最小缩放到 0.3 倍  
            //		if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f) {  
            transform.localScale *= (1f + scaleFactor);
            //		}  

           

            //记住最新的触摸点，下次使用  
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
    }
}
