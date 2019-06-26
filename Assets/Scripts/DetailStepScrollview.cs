using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DetailStepScrollview : MonoBehaviour {

    public GameObject buttonPrefab;
    public CommonData storeData;
    Quaternion tempRotation;
    bool btnclicked;
 	// Use this for initialization
	void Start () {
		
	}

    public void RotateToCenterOfScreen(string btnTitle){

        StepDetail stepDetail = storeData.newStrategy.steps[storeData.chosenStepIndex];

        foreach (StepDetailAction tempoperator in stepDetail.detailActions)
        {
            if (tempoperator.imageType)
            {
                if (tempoperator.createTime.Equals(btnTitle))
                {
                    storeData.gameObject.GetComponent<PaintOnMap>().showImageView(tempoperator);
                }
            }else if (tempoperator.createTime.Equals(btnTitle))
            {
                if (!storeData.gameObject.GetComponent<MapObj>().currentFloorName.Equals(tempoperator.parentFloor))
                {
                    storeData.gameObject.GetComponent<MapObj>().createMapWithName(tempoperator.parentFloor);
                }
                tempRotation = tempoperator.mainTransform.rotation;
                btnclicked = true;
                storeData.gameObject.GetComponent<CommonControl>().operatorCreateTime = btnTitle;

                //GameObject EditPanelobj = storeData.gameObject.GetComponent<CommonControl>().EditPanelobj;

                //EditPanelobj.SetActive(true);
                //foreach (GameObject temp in storeData.Operators)
                //{
                //    if (temp.GetComponent<OperatorData>().createTime.Equals(btnTitle)){
                //        EditPanelobj.GetComponent<HandelAction>().actionObj = temp;
                //    }
                //}
               
                 
                //EditPanelobj.GetComponent<HandelAction>().controlScript = storeData.gameObject.GetComponent<CommonControl>();
                //EditPanelobj.GetComponent<HandelAction>().editSetup();
            }
        }



    }


    IEnumerator DownloadImage(string url, Image image)
    {
        Debug.Log(url);
        WWW www = new WWW(url);

        yield return www;

        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();
        Debug.Log(DataObj.cachePath + url.GetHashCode());
        File.WriteAllBytes(DataObj.cachePath + url.GetHashCode(), pngData);

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }

    IEnumerator LoadLocalImage(string url, Image image)
    {
        // 已在本地缓存
        string filePath = "file:///" + DataObj.cachePath + url.GetHashCode();
        WWW www = new WWW(filePath);
        yield return www;
        Texture2D tex2d = www.texture;

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }


    // Update is called once per frame
    void Update () {

        if (btnclicked)
        {
            if (storeData.gameObject.GetComponent<MapObj>().currentFloorName.Equals(storeData.gameObject.GetComponent<CommonControl>().tempCurrentFloorName))
            {
                GameObject displayMap = storeData.gameObject.GetComponent<CommonControl>().displayMap;
                //displayMap.transform.Rotate(0, -tempRotation.y * Time.deltaTime, 0, Space.Self);
                //displayMap.transform.Rotate(-tempRotation.x * Time.deltaTime, 0, 0, Space.World);

                displayMap.transform.rotation = Quaternion.Lerp(displayMap.transform.rotation, tempRotation, 7 * Time.deltaTime);
                if (displayMap.transform.rotation == tempRotation)
                {
                    btnclicked = false;
                    if (DataObj.isUserOwn)
                    {
                        storeData.gameObject.GetComponent<CommonControl>().SimpleShowOrHide();
                    }
                    else
                    {
                        storeData.gameObject.GetComponent<CommonControl>().ShowSelectionMenu();
                    }
                }
                storeData.gameObject.GetComponent<CommonControl>().bigSizeWhenchosen(storeData.gameObject.GetComponent<CommonControl>().operatorCreateTime);
                //storeData.gameObject.GetComponent<CommonControl>().ShowSelectionMenu();
            }

        }

        if (storeData != null)
        {
            if (storeData.isUpdated)
            {
                storeData.isUpdated = false;

                foreach (Transform button in transform.GetComponentsInChildren<Transform>())
                {
                    if (button.gameObject.tag.Equals("DetailStep"))
                    {
                        Destroy(button.gameObject);
                    }
                }

                StepDetail stepDetail = storeData.newStrategy.steps[storeData.chosenStepIndex];
                if (stepDetail.detailActions != null)
                {
                    for (int i = 0; i < stepDetail.detailActions.Count; i++)
                    {

                        GameObject newBtn = (GameObject)Instantiate(buttonPrefab);
                        newBtn.transform.SetParent(transform);
                        newBtn.transform.localScale = new Vector3(1f, 1f, 1f);
                        StepDetailAction action = stepDetail.detailActions[i];
                        DetailStep item = newBtn.GetComponent<DetailStep>();
                        item.btnTitle.text = "Click button and tap Operator to add information";
                        item.btnCreateTime = action.createTime;
                        Debug.Log(action.actionName);
                        if (action.imageType)
                        {
                            if (action.stepImageUrl != null)
                            {
                                if (File.Exists(DataObj.cachePath + action.stepImageUrl.GetHashCode()))
                                {
                                    StartCoroutine(LoadLocalImage(action.stepImageUrl, item.btnIcon));
                                }
                                else
                                {
                                    StartCoroutine(DownloadImage(action.stepImageUrl, item.btnIcon));
                                }
                                StartCoroutine(DownloadImage(action.stepImageUrl, item.btnIcon));
                            }
                            else { 
                            Texture2D texture = new Texture2D(Screen.width, Screen.height);
                            texture.LoadImage(action.imageData);

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                            item.btnIcon.sprite = sprite;
                             }
                        }
                        else
                        {
                            item.btnIcon.sprite = Resources.Load<Sprite>("Images/OperatorImage/" + action.actionName);

                        }
                        item.scrollviewScript = this;
                        if (action.actionInfo != null)
                        {
                            item.btnTitle.text = action.actionInfo;
                        }
                    }
                }



            }
        }

	}
}
