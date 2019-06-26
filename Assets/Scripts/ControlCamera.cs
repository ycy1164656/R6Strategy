using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
 
public class ControlCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float moveSpeed = 5.0f;
    Vector3 originPosition;
    bool pressed = false;
    public GameObject controlobj;
    // Use this for initialization
    void Start () {
        originPosition = Camera.main.transform.position;
          
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    public void BackToOriginPosition(){
        Camera.main.transform.position = originPosition;
        if (controlobj.GetComponent<DpScanMap>().followed)
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("camera", JsonUtility.ToJson(Camera.main.transform.position));
             DataObj.lbc.LocalPlayer.SetCustomProperties(h);

           
        }
    }

    // Update is called once per frame
    void Update () {
         
        if (pressed)
        {
            if (name.Equals("CameraRightBtn"))
            {
                 Camera.main.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }
            if (name.Equals("CameraDownBtn"))
            {
                Camera.main.transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
            }
            if (name.Equals("CameraLeftBtn"))
            {
                Camera.main.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            }
            if (name.Equals("CameraUpBtn"))
            {
                Camera.main.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            }

            if (controlobj.GetComponent<DpScanMap>().followed)
            {

                ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
                h.Add("camera", JsonUtility.ToJson(Camera.main.transform.position));
                DataObj.lbc.LocalPlayer.SetCustomProperties(h);

            }

        }


    }
}
