using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using Parse;

public class PaintOnMap : MonoBehaviour {
    LineRenderer line;
    GameObject clone;
    public GameObject lineObj;
    public List<GameObject> hideObj;
    public List<GameObject> showObj;
    List<GameObject> lineArray = new List<GameObject>();
    public GameObject imagePanel;
    public Image screenShotImg;
    public GameObject inputPanel;
    public GameObject textObj;
    public Text descText;
    public GameObject modifyBtn;
    public GameObject deleteBtn;
    public GameObject showInfoBtn;
    public GameObject browsetextObj;
    public GameObject confirmModifyBtn;
    public Text browsedescText;
    StepDetailAction detailAction;
    public bool isModify;
    bool fileSaved;
     ParseFile tempFile;
       int i = 0;
	// Use this for initialization
	void Start () {
		
	}

    IEnumerator DownloadImage(string url, Image image)
    {
        WWW www = new WWW(url);

        yield return www;

        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();
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

    public void showImageView(StepDetailAction action){
        detailAction = action;
        imagePanel.SetActive(true);
        confirmModifyBtn.SetActive(false);

        if (action.stepImageUrl != null)
        {
            if (File.Exists(DataObj.cachePath + action.stepImageUrl.GetHashCode()))
            {
                StartCoroutine(LoadLocalImage(action.stepImageUrl, screenShotImg));
            }
            else
            {
                StartCoroutine(DownloadImage(action.stepImageUrl, screenShotImg));
            }
        }
        else
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height);
            texture.LoadImage(detailAction.imageData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            screenShotImg.sprite = sprite;

        }

        browsedescText.text = detailAction.actionInfo;
        browsetextObj.SetActive(true);
        if (DataObj.isScan)
        {
            if (DataObj.isUserOwn)
            {
                modifyBtn.SetActive(true);
                deleteBtn.SetActive(true);
                showInfoBtn.SetActive(false);
            }else{
                modifyBtn.SetActive(false);
                deleteBtn.SetActive(false);
                showInfoBtn.SetActive(true);
                browsetextObj.SetActive(false);
            }
        }else{
            modifyBtn.SetActive(true);
            deleteBtn.SetActive(true);
            showInfoBtn.SetActive(false);
        }

    }

    bool infoShowed = false;
    public void showDetailInfo(){

        infoShowed = !infoShowed;
        browsetextObj.SetActive(infoShowed);

    }

    public void modifyClicked(){
        isModify = true;
        deleteBtn.SetActive(false);
        confirmModifyBtn.SetActive(true);
        inputPanel.SetActive(true);
        inputPanel.GetComponent<InputPanelAction>().descriptionText = browsedescText;
    }


    public void confirmModifyClicked(){
        CommonData storeData = GetComponent<CommonData>();
        StepDetail detail = storeData.newStrategy.steps[storeData.chosenStepIndex];
 
       
        foreach (StepDetailAction step in detail.detailActions)
        {
            if (step.createTime.Equals(detailAction.createTime))
            {
                step.actionInfo = browsedescText.text;

            }
        }
        storeData.isUpdated = true;
        imagePanel.SetActive(false);

    }

    public void deleteImage(){
        CommonData storedata = GetComponent<CommonData>();
        StepDetail detailsteps = storedata.newStrategy.steps[storedata.chosenStepIndex];
         


        StepDetailAction tempAction = new StepDetailAction();

        foreach (StepDetailAction step in detailsteps.detailActions)
        {
            if (step.createTime.Equals(detailAction.createTime))
            {
                tempAction = step;
            }
        }
        //detailsteps.detailActions = newList;
        detailsteps.detailActions.Remove(tempAction);

        storedata.isUpdated = true;
        imagePanel.SetActive(false);
    }



    public void showPanelAndImage(){

        //LoadByIO();
        //Application.persistentDataPath + "/CaptureScreenshot.png"

        foreach (GameObject gameobj in hideObj)
        {
            gameobj.SetActive(false);
        }
        foreach (GameObject gameobj in showObj)
        {
            gameobj.SetActive(true);
        }
        descText.text = null;
        //imagePanel.SetActive(true);
        //mainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //StartCoroutine(ScreenShoot());

    }

    public void inputDescription()
    {
        inputPanel.SetActive(true);
        inputPanel.GetComponent<InputPanelAction>().descriptionText = descText;
         
    }



    public void confirmNow(){
        Debug.Log(11111111111);
        StartCoroutine(ScreenShoot());
        GetComponent<CommonControl>().canDrawLine = false;
        foreach (GameObject gameobj in hideObj)
        {
            gameobj.SetActive(true);
        }
        foreach (GameObject gameobj in showObj)
        {
            gameobj.SetActive(false);
        }

        StartCoroutine(deleteObjcs());
       
    }


    IEnumerator deleteObjcs()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject gameobj in lineArray)
        {
            Destroy(gameobj);
        }
        lineArray.RemoveAll(Isgameobject);

    }



    private static bool Isgameobject(GameObject s)
    {
        return true;
    }
    public void cancelDraw(){
        foreach (GameObject gameobj in hideObj)
        {
            gameobj.SetActive(true);
        }
        foreach (GameObject gameobj in showObj)
        {
            gameobj.SetActive(false);
        }

        foreach (GameObject gameobj in lineArray)
        {
            Destroy(gameobj);
        }
        lineArray.RemoveAll(Isgameobject);
        GetComponent<CommonControl>().canDrawLine = false;
    }



    public void lastStepAction(){
        Debug.Log(lineArray.Count);
        if (lineArray.Count > 0)
        {
            Destroy(lineArray[lineArray.Count - 1]);
            lineArray.RemoveAt(lineArray.Count - 1);
        }

        Debug.Log(lineArray.Count);
    }


    public void hidePanel(){
        imagePanel.SetActive(false);
        GetComponent<CommonControl>().canDrawLine = false;
    }
     

    private IEnumerator ScreenShoot()
    {
        //Wait for graphics to render
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        //Render from all!
        //foreach (Camera cam in Camera.allCameras)
        //{
        Camera.main.targetTexture = rt;
        Camera.main.Render();
        Camera.main.targetTexture = null;
        //}

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        Camera.main.targetTexture = null;

        //Added to avoid errors
        RenderTexture.active = null;
        Destroy(rt);
       
        //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
        byte[] bytes = screenShot.EncodeToPNG();
       
        ParseFile file = new ParseFile(getNumberId() + ".png", bytes);
        file.SaveAsync().ContinueWith(t =>
        {
            Debug.Log(t.IsFaulted);
            if (!t.IsFaulted)
            {

                fileSaved = true;

            }
            else
            {
                Debug.Log(t.Exception.Message);
                Debug.LogException(t.Exception);
            }
        });
        tempFile = file;
        //storeData.imageFiles.Add(file);
       


        //Texture2D texture = new Texture2D(Screen.width, Screen.height);
        //texture.LoadImage(bytes);

        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //screenShotImg.sprite = sprite;
    }
    string getNumberId()
    {

        System.TimeSpan time = System.DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1);

        string timestampString = ((long)time.TotalMilliseconds).ToString();

        System.Random rd = new System.Random();

        int rdn = rd.Next(100, 999);

        string idString = timestampString.Substring(timestampString.Length - 6) + rdn.ToString();

        return idString;
    }

    //private void LoadByIO()
    //{

    //    //double startTime = (double)Time.time;
    //    //创建文件读取流
    //    FileStream fileStream = new FileStream(Application.persistentDataPath + "/CaptureScreenshot.png", FileMode.Open, FileAccess.Read);
    //    fileStream.Seek(0, SeekOrigin.Begin);
    //    //创建文件长度缓冲区
    //    byte[] bytes = new byte[fileStream.Length];
    //    //读取文件
    //    fileStream.Read(bytes, 0, (int)fileStream.Length);
    //    //释放文件读取流
    //    fileStream.Close();
    //    fileStream.Dispose();
    //    fileStream = null;

    //    //创建Texture
    //  Texture2D texture = new Texture2D(Screen.width, Screen.height);
    //texture.LoadImage(bytes);

    //    //创建Sprite


    //    //startTime = (double)Time.time - startTime;
    //    //Debug.Log("IO加载用时:" + startTime);
    //}





    // Update is called once per frame
    void Update () {

        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    Debug.Log("当前触摸在UI上");
        //    return;

        //}

        if (fileSaved)
        {
            fileSaved = false;
            CommonData storeData = GetComponent<CommonData>();

            StepDetailAction action = new StepDetailAction();
            action.createTime = System.DateTime.Now.ToString();
            action.imageType = true;
             action.actionInfo = descText.text;
            action.stepImageUrl = tempFile.Url.ToString();
            action.actionType = "Image";
            action.parentFloor = "10";
            StepDetail detail = storeData.newStrategy.steps[storeData.chosenStepIndex];
            if (detail.detailActions != null)
            {
                detail.detailActions.Add(action);
            }
            else
            {
                detail.detailActions = new List<StepDetailAction>();
                detail.detailActions.Add(action);
            }



            stepDetailPAnel panelScript = GetComponent<CommonControl>().detailPanel.GetComponent<stepDetailPAnel>();
            panelScript.scrollView.GetComponent<DetailStepScrollview>().storeData = storeData;
            storeData.isUpdated = true;
        }



        if (GetComponent<CommonControl>().canDrawLine)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.touches[0];
                    if (touch.phase == TouchPhase.Began)
                    {
                        clone = (GameObject)Instantiate(lineObj, lineObj.transform.position, Quaternion.identity);
                        lineArray.Add(clone);
                        //获得该物体上的LineRender组件  
                        line = clone.GetComponent<LineRenderer>();
                        //设置起始和结束的颜色  
                        line.startColor = Color.red;

                        line.endColor = Color.red;
                        //设置起始和结束的宽度  
                        line.startWidth = .2f;
                        line.endWidth = .2f;
                        //计数  
                        i = 0;
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        i++;
                        //设置顶点数  
                        line.positionCount = i;
                        //设置顶点位置(顶点的索引，将鼠标点击的屏幕坐标转换为世界坐标)  
                        line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 15)));
                    }

                }

            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //实例化对象  
                    clone = (GameObject)Instantiate(lineObj, lineObj.transform.position, Quaternion.identity);
                    lineArray.Add(clone);
                    //获得该物体上的LineRender组件  
                    line = clone.GetComponent<LineRenderer>();
                    //设置起始和结束的颜色  
                    line.startColor = Color.red;

                    line.endColor = Color.red;
                    //设置起始和结束的宽度  
                    line.startWidth = .2f;
                    line.endWidth = .2f;
                    //计数  
                    i = 0;
                }
                if (Input.GetMouseButton(0))
                {
                    //每一帧检测，按下鼠标的时间越长，计数越多  
                    i++;
                    //设置顶点数  
                    line.positionCount = i;
                    //设置顶点位置(顶点的索引，将鼠标点击的屏幕坐标转换为世界坐标)  
                    line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));


                }
            }
        }
       
    }
}
