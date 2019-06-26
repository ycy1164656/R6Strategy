using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using UnityEngine.EventSystems;
//using UnityEditor;
using UnityEngine.SceneManagement;
 
public class CommonControl : MonoBehaviour {

    // Use this for initialization
    public GameObject selectPrefab;
    public GameObject selectionPanel;
    public Text AddStrategyText;
    public GameObject stepDetailPanel;
    public GameObject detailPanel;
    public bool canAddOperation;
    public GameObject OperatorChoosePanel;
    public GameObject EditPanelobj;
    public GameObject worldCanvas;
    //public bool alreadyHavepoint;
    CommonData storedData;
    public GameObject operatorPrefab;
    public GameObject textCanvas;
    public string tempCurrentFloorName;
    public bool IndetailActions;
    public GameObject displayMap;
    public GameObject directionArrowPrefab;
    public GameObject doneEditArrowBtn;
    public bool arrowControl;
    public float scaleChange;
    public GameObject stepScrollView;
    public GameObject addStepButton;
    public GameObject SaveButtonObj;
    public GameObject toggleBtnTransform;
    public GameObject StarBtn;
    public GameObject CommentBtn;
    public bool commentShow;
    public GameObject bgPanel;
    bool saveSucceed;
    public GameObject showOrHideTransForm;
    public Text showOrHideText;
    public Text hideText;
    public Text doneText;
    public GameObject hideButton;
    public GameObject cancelDetailBtn;
    public GameObject markCommon;
    public bool canDrawLine;
    public GameObject hudObj;
    public GameObject allUI;
    public string operatorCreateTime;
    bool fileSaved;
	void Start () {
        cancelDetailBtn.SetActive(false);
         saveSucceed = false;
        canAddOperation = false;
        selectionPanel.SetActive(false);
        storedData = gameObject.GetComponent<CommonData>();
        tempCurrentFloorName = gameObject.GetComponent<MapObj>().currentFloorName;
        SaveButtonObj.SetActive(false);
        StarBtn.SetActive(false);
        CommentBtn.SetActive(false);
        if (DataObj.isBrowseMap)
        {
            selectionPanel.SetActive(false);
            toggleBtnTransform.SetActive(false);
        }

    }



    public void showCommentView(){
        commentShow = true;
    }
    public void hideCommentView()
    {
        commentShow = false;
    }


    public void ScanData(){
        InputField[] inputFields;

        inputFields = selectionPanel.GetComponentsInChildren<InputField>();
        inputFields[1].text = storedData.newStrategy.strategyTitle;
        inputFields[0].text = storedData.newStrategy.stepInfo;
       
        if (DataObj.isUserOwn == false)
        {
            inputFields[0].enabled = false;
            inputFields[1].enabled = false;
            selectionPanel.SetActive(true);
            toggleBtnTransform.SetActive(true);
           
             isShowSelection = true;
            doneText.text = "Hide";

            hideButton.SetActive(false);
            stepScrollView.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//left bottom
            stepScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//left bottom
            addStepButton.SetActive(false);
            bgPanel.SetActive(false);
        }
        StarBtn.SetActive(true);
        CommentBtn.SetActive(true);

     
    }
   

    public void doneEditArrow(GameObject arrow){
        StepDetail detailsteps = storedData.newStrategy.steps[storedData.chosenStepIndex];
        string createtime = arrow.GetComponent<ArrowControl>().objCreateTime;
        foreach (StepDetailAction step in detailsteps.detailActions)
        {
            if (step.createTime.Equals(createtime))
            {
                step.hasArrow = true;
                step.directionTransform.localscale = arrow.transform.localScale;
                step.directionTransform.localposition = arrow.transform.localPosition;
                step.directionTransform.localrotation = arrow.transform.localRotation;

             }
        }
        Destroy(arrow.GetComponent<ArrowControl>());
        arrowControl = false;
        doneEditArrowBtn.SetActive(false);

    }

    bool justShowOrHide = false;
    bool isShowSelection = false;
    public void SimpleShowOrHide(){
        Debug.Log("clicked");
        justShowOrHide = !justShowOrHide;
        isShowSelection = !isShowSelection;
        if (justShowOrHide)
        {
            hideButton.SetActive(true);
            selectionPanel.SetActive(true);
            toggleBtnTransform.SetActive(false);
            showOrHideTransForm.SetActive(false);
        }
        else
        {
            selectionPanel.SetActive(false);
            showOrHideText.text = "Show";
            toggleBtnTransform.SetActive(true);
            showOrHideTransForm.SetActive(true);
            //showOrHideTransForm.SetActive(false);
        }
    }




    public void ShowSelectionMenu(){

        isShowSelection = !isShowSelection;
        justShowOrHide = !justShowOrHide;
        if (isShowSelection)
        {
            AddStrategyText.text = "Done";
            if (DataObj.isScan && DataObj.isUserOwn == false)
            {
                doneText.text = "Hide";
                showOrHideTransForm.SetActive(false);
            }
            if (DataObj.isUserOwn == true)
            {
                StarBtn.SetActive(false);
                CommentBtn.SetActive(false);
            }
            //Debug.Log(selectionPanel.GetComponent<RectTransform>().sizeDelta.x);
            //Debug.Log(selectionPanel.GetComponent<RectTransform>().rect.width);

            selectionPanel.SetActive(true);
            toggleBtnTransform.SetActive(false);
            showOrHideTransForm.SetActive(false);
            SaveButtonObj.SetActive(false);

        }else{

            InputField[] inputFields;

            inputFields = selectionPanel.GetComponentsInChildren<InputField>();

            if (inputFields[1].text.Length == 0 || inputFields[0].text.Length == 0)
            {
                MNPopup mNPopup = new MNPopup("Info", "Please Add Title And Introduction");
                mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
                mNPopup.Show();

                //DataObj dataObj = GameObject.Find("DataObj").GetComponent<DataObj>();
                //dataObj.showInfoWithStrings("1", "2");
                return;
            }

            //AddStrategyText.text = "Add";

            if (storedData.newStrategy.strategyTitle != null || storedData.newStrategy.stepInfo != null || storedData.newStrategy.steps.Count != 0 || DataObj.isUserOwn == true)
            {
                AddStrategyText.text = "Edit";
            }
            toggleBtnTransform.SetActive(true);
            showOrHideTransForm.SetActive(true);

            //TODO  Edit
            //selectionPanel.transform.position = new Vector3(oldX, selectionPanel.transform.position.y, selectionPanel.transform.position.z);
            //toggleBtnTransform.transform.position = new Vector3(0.6f, toggleBtnTransform.transform.position.y, toggleBtnTransform.transform.position.z);
            selectionPanel.SetActive(false);
            //showOrHideTransForm.transform.position = new Vector3(0.6f, showOrHideTransForm.transform.position.y, showOrHideTransForm.transform.position.z);
            showOrHideTransForm.SetActive(false);
            if (DataObj.isScan && DataObj.isUserOwn == false)
            {
                AddStrategyText.text = "Show";
                return;
            }
           

            //Debug.Log(inputFields[0].text + "," + inputFields[1].text);
            storedData.newStrategy.strategyTitle = inputFields[1].text;
            storedData.newStrategy.stepInfo = inputFields[0].text;
            SaveButtonObj.SetActive(true);


        }

    }
    public void SaveAndReturn()
    {

        hudObj.SetActive(true);
        allUI.SetActive(false);


        //bool hasImage = false;

        foreach (StepDetail stepdetail in storedData.newStrategy.steps)
        {
            foreach (StepDetailAction detailA in stepdetail.detailActions)
            {
                if (detailA.imageType)
                {
                    detailA.imageData = null;
                }
               
            }
        }
        fileSaved = true;

        //if (hasImage)
        //{
        //    Debug.Log("hasImage");
        //int x = 0;
        //int y = 0;
        //for (int i = 0; i < storedData.newStrategy.steps.Count; i++)
        //{
        //    StepDetail step = storedData.newStrategy.steps[i];
        //    for (int j = 0; j < step.detailActions.Count; j++)
        //    {
        //        StepDetailAction detailAction = step.detailActions[j];
        //        if (detailAction.imageType)
        //        {
        //            if (detailAction.imageData != null)
        //            {
        //                x = i;
        //                y = j;
        //                ParseFile file = new ParseFile("image.png", detailAction.imageData);
        //                file.SaveAsync().ContinueWith(t =>
        //                {
        //                    Debug.Log(t.IsFaulted);
        //                    if (!t.IsFaulted)
        //                    {
        //                        Debug.Log(file.Url);
        //                        Debug.Log(x + "   " + y + "   " + i + "   " + j);
        //                        detailAction.stepImageUrl = file.Url.ToString();
        //                        detailAction.imageData = null;
        //                        if (j >= y && i >= x)
        //                        {
        //                            fileSaved = true;

        //                        }
        //                    }
        //                    else
        //                    {
        //                        Debug.Log(t.Exception.Message);
        //                        Debug.LogException(t.Exception);
        //                    }
        //                });

        //            }
        //        }
        //    }
        //}
        //}else{
        //    Debug.Log("hasNoImage");
        //    fileSaved = true;
        //}
       
    }


    void saveAfterFileupload(){


        List<string> operators = new List<string>();

        foreach (StepDetail step in storedData.newStrategy.steps)
        {
            foreach (StepDetailAction detailAction  in step.detailActions)
            {
                if (!operators.Contains(detailAction.actionName))
                {
                    operators.Add(detailAction.actionName);
                }
            }
        }



        string strategyString = JsonUtility.ToJson(storedData.newStrategy);
        //Debug.Log(test);
        //MNP.ShowPreloader("", "Saving...");
        InputField[] inputFields = selectionPanel.GetComponentsInChildren<InputField>();
        if (DataObj.isUserOwn)
        {
            Debug.Log("SaveOwn");
            ParseObject strategy = DataObj.strategy;
            strategy["strategyString"] = strategyString;
            strategy["Operators"] = operators;
            strategy.SaveAsync().ContinueWith(t =>
            {
                //MNP.HidePreloader();
                // Now let's update it with some new data.  In this case, only cheatMode
                // and score will get sent to the cloud.  playerName hasn't changed.
                saveSucceed = true;

            });
        }
        else
        {
            Debug.Log("Savenew");
            ParseObject strategy = new ParseObject("Strategy");
            strategy["strategyString"] = strategyString;
            strategy["mapName"] = DataObj.MapName;
            strategy["user"] = ParseUser.CurrentUser;
            strategy["stars"] = 0;
            strategy["commentNum"] = 0;
            strategy["title"] = inputFields[1].text;
            strategy["numberId"] = getNumberId();
            strategy["Operators"] = operators;
            strategy.SaveAsync().ContinueWith(t =>
            {
                //MNP.HidePreloader();
                // Now let's update it with some new data.  In this case, only cheatMode
                // and score will get sent to the cloud.  playerName hasn't changed.
                saveSucceed = true;

            });
        }
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


    public void ShowStrategyPanel(){
        if (AddStrategyText.text.Equals("Add"))
        {
            AddStrategyText.text = "Done";
            isShowSelection = true;
            selectionPanel.SetActive(true);
            selectionPanel.transform.position = new Vector3(0, selectionPanel.transform.position.y, selectionPanel.transform.position.z);
        }else{
            AddStrategyText.text = "Add";
            selectionPanel.SetActive(false);
        }

    }
    public void AddSteps(){

        if (detailPanel != null)
        {
            Destroy(detailPanel);
        }

        detailPanel  = (GameObject)Instantiate(stepDetailPanel);
        IndetailActions = true;
        detailPanel.transform.SetParent(selectionPanel.transform);
        //detailPanel.transform.position = new Vector3(0,0,0);
        detailPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//left bottom
        detailPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//right top
        detailPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        detailPanel.name = "StepDetailPanel";
        stepDetailPAnel panelscript = detailPanel.GetComponent<stepDetailPAnel>();
        panelscript.Setup(this);
        StepDetail newDetail = new StepDetail();
        storedData.newStrategy.steps.Add(newDetail);
        storedData.chosenStepIndex = storedData.newStrategy.steps.Count - 1;
        newDetail.stepIndex = storedData.newStrategy.steps.Count - 1;
        cancelDetailBtn.SetActive(true);
        //Debug.Log(panel.transform.position);
 
    }
    public void cancelAddDetail(){
        Destroy(detailPanel);
        cancelDetailBtn.SetActive(false);
        storedData.newStrategy.steps.RemoveAt(storedData.chosenStepIndex);

    }

    public void EditOrReviewStepDetail(StepDetail detailAction){
        if (detailPanel != null)
        {
            Destroy(detailPanel);
        }

        detailPanel = (GameObject)Instantiate(stepDetailPanel);

        detailPanel.transform.SetParent(selectionPanel.transform);
        //detailPanel.transform.position = new Vector3(0, 0, 0);
        detailPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//left bottom
        detailPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//right top
        detailPanel.transform.localScale = new Vector3(1f, 1f, 1f);
      
        detailPanel.name = "StepDetailPanel";
        stepDetailPAnel panelscript = detailPanel.GetComponent<stepDetailPAnel>();
        panelscript.Setup(this);
        panelscript.detailIntro.text = detailAction.stepDetailInfo;

        panelscript.scrollView.GetComponent<DetailStepScrollview>().storeData = storedData;

        createOperatorOnMap();

        storedData.isUpdated = true;

    }
    public void bigSizeWhenchosen(string createTime){
        foreach (GameObject cube in storedData.Operators)
        {
            OperatorData cubeData = cube.GetComponent<OperatorData>();
            cube.transform.localScale = cubeData.localscale;
            if (cubeData.createTime.Equals(createTime))
            {
                cube.transform.localScale = new Vector3(3f * cubeData.localscale.x, 3f * cubeData.localscale.y, 3f * cubeData.localscale.z);
            }

        }
    }
     
    public void createOperatorOnMap(){
        
        if (storedData == null)
        {
            return;
        }

        if (storedData.newStrategy.steps.Count == 0)
        {
            return;
        }
        List<StepDetailAction> detailActions = storedData.newStrategy.steps[storedData.chosenStepIndex].detailActions;

        foreach (GameObject item in storedData.Operators)
        {
              Destroy(item);
        }
        storedData.Operators = new List<GameObject>();

        if (detailActions != null && IndetailActions)
        {

            for (int i = 0; i < detailActions.Count; i++)
            {
                StepDetailAction action = detailActions[i];
                //string currentfloorName = gameObject.GetComponent<MapObj>().currentFloorName;
                if (action.parentFloor.Equals(tempCurrentFloorName) && action.actionType.Equals("Operator"))
                {

                    OperatorObj operatorobj = new OperatorObj();
                    foreach (OperatorObj op in OperatorChoosePanel.GetComponent<OperatorChoose>().operators)
                    {
                        if (op.name.Equals(action.actionName))
                        {
                            operatorobj = op;
                        }
                    }


                    GameObject parentObj = GameObject.Find(action.parentName);
                    Debug.Log("parentObj = " + parentObj);
                    GameObject cube = (GameObject)Instantiate(operatorobj.operatorPrefab);
                    cube.transform.SetParent(parentObj.transform);
                    cube.transform.localScale = action.mainTransform.localscale;
                    cube.transform.localPosition = action.mainTransform.localposition;
                    cube.transform.localRotation = action.mainTransform.localrotation;

                    if (action.hasArrow)
                    {
                        GameObject arrow = (GameObject)Instantiate(directionArrowPrefab);
                        arrow.name = "arrow";
                        arrow.transform.SetParent(cube.transform);
                        arrow.transform.localScale = action.directionTransform.localscale;
                        arrow.transform.localPosition = action.directionTransform.localposition;
                        arrow.transform.localRotation = action.directionTransform.localrotation;
                    }
                   

                    OperatorData cubeData = cube.GetComponent<OperatorData>();
                    cubeData.createTime = action.createTime;
                    cubeData.actionName = operatorobj.name;
                    cubeData.parentFloor = action.parentFloor;
                    cubeData.actionType = action.actionType;
                    cubeData.localscale = action.mainTransform.localscale;

                    storedData.Operators.Add(cube);
                    storedData.isUpdated = true;
                 }
               
            }
        }
        if (operatorCreateTime != null)
        {
            bigSizeWhenchosen(operatorCreateTime);
        }
       

    }
    

    public void AlignGhostToSurface(Transform itemToAlign, Vector3 hitNormal)
    {
        if (itemToAlign == null) return;

        itemToAlign.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal) * Quaternion.Euler(new Vector3(0, 0, 0));

    }

    public Transform CreateBasePivot(Transform item, Vector3 pivotOffset)
    {
        var normMesh = item.GetComponentInChildren<MeshRenderer>();
        var skinMesh = item.GetComponentInChildren<SkinnedMeshRenderer>();

        if (normMesh == null && skinMesh == null)
        {
            Debug.LogError("No renderers found!");
            return null;
        }

        GameObject pivotG = new GameObject("Temp_Ghost_Pivot_Parent"); // create parent
        Transform pivotT = pivotG.transform;

        pivotG.AddComponent<PivotHelper>();
        //get mesh center
        Vector3 meshCenter = (normMesh != null) ? normMesh.bounds.extents : skinMesh.bounds.extents;
        // apply pivot delta
        meshCenter.x = pivotOffset.x;
        meshCenter.z = pivotOffset.z;
        meshCenter.y += pivotOffset.y;


        item.SetParent(pivotT); // set the current object as parent
        item.localPosition = meshCenter; // move the object and leave the parent object in the pivot position

        return pivotT;
    }

    /// <summary>
    /// Check if the object pivot is in center or not.
    /// This function returns the pivot Offset (can be Vector3.zero)
    /// </summary>
    /// <param name="item">Item to use</param>
    /// <param name="renderer">Renderer attached to the item</param>
    /// <param name="pivotOffset">Offset of the pivot to be in base</param>
    /// <returns></returns>
    public bool isPivotInBase(Transform item, out Vector3 pivotOffset)
    {
        var normMesh = item.GetComponentInChildren<MeshRenderer>();
        var skinMesh = item.GetComponentInChildren<SkinnedMeshRenderer>();

        if (normMesh == null && skinMesh == null)
        {
            Debug.LogError("No mesh renderer found!");
            pivotOffset = Vector3.zero;
            return false;
        }

        var pivotMargin = (normMesh != null) ? normMesh.bounds.extents.y * 2 / 3 : skinMesh.bounds.extents.y * 2 / 3; //set the base pivot margin. 
                                                                                                                      //Its' height must be lower than obj center * 2/3

        Vector3 delta = item.position - ((normMesh != null) ? normMesh.bounds.center : skinMesh.bounds.center);
        if (delta.magnitude >= pivotMargin && delta.y < 0) //delta.y < 0 fix issues that not centerd pivots above the object center were taken as base pivots
        {
            pivotOffset = Vector3.zero;
            return true;
        }
        else
        {
            pivotOffset = delta; // save pivot delta to use to create a fake pivot
            return false;
        }
    }


	private void Update()
	{
        if (saveSucceed)
        {
            saveSucceed = false;
            if (DataObj.isDp)
            {
                DataObj.isJoin = false;
                DataObj.lbc.OpLeaveRoom(false);
                DataObj.lbc.Disconnect();
            }
            DataObj.isScan = false;
            DataObj.isBrowseMap = false;
            DataObj.isDp = false;
            DataObj.shouHud = false;
            DataObj.isRecommend = false;
            SceneManager.LoadScene(0);
        }


        if (fileSaved)
        {
            fileSaved = false;
            saveAfterFileupload();
        }


        if (!tempCurrentFloorName.Equals(gameObject.GetComponent<MapObj>().currentFloorName))
        {
            tempCurrentFloorName = gameObject.GetComponent<MapObj>().currentFloorName;
            createOperatorOnMap();
        }

        //电脑操作
        if (EventSystem.current.IsPointerOverGameObject()){
            //Debug.Log("当前触摸在UI上");
            return;
        }else{
            //Debug.Log("当前没有触摸在UI上");
        }

     
        if (Input.touchCount == 0)
        {
            return;
        }
        //if (Input.GetMouseButtonUp(0))
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(ray);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                    //Debug.Log(hit.point);

                if (hit.collider.gameObject.tag.Equals("Operator"))
                {
                    //Debug.Log("11121212121");
                    if (canAddOperation)
                    {
                        canAddOperation = false;
                        return;
                    }else if (detailPanel == null)
                    {
                        return;
                    }else{
                        if (DataObj.isScan && DataObj.isUserOwn == false)
                        {
                            StepDetail detailsteps = storedData.newStrategy.steps[storedData.chosenStepIndex];
                            string createtime = hit.collider.gameObject.GetComponent<OperatorData>().createTime;
                            StepDetailAction tempAction = new StepDetailAction();

                            foreach (StepDetailAction step in detailsteps.detailActions)
                            {
                                if (step.createTime.Equals(createtime))
                                {
                                    tempAction = step;
                                }
                            }

                            MNPopup mNPopup = new MNPopup("Info", tempAction.actionInfo);
                            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
                            mNPopup.Show();


                        }
                        else{
                            if (arrowControl == false) 
                            {
                                EditPanelobj.SetActive(true);
                                EditPanelobj.GetComponent<HandelAction>().actionObj = hit.collider.gameObject;
                                EditPanelobj.GetComponent<HandelAction>().controlScript = this;
                                EditPanelobj.GetComponent<HandelAction>().editSetup();
                            }
                       
                        }
                     }
                }

                if (gameObject.GetComponent<DpScanMap>().canAddMark)
                {
                    addMarkOnMap(hit);
                    gameObject.GetComponent<DpScanMap>().canAddMark = false;
                }

                if (canAddOperation && OperatorChoosePanel.activeSelf == false)
                   {
                    OperatorChoosePanel.SetActive(true);
                    OperatorChoosePanel.GetComponent<OperatorChoose>().controlScript = this;
                    OperatorChoosePanel.GetComponent<OperatorChoose>().hitPoint = hit.point;
                    OperatorChoosePanel.GetComponent<OperatorChoose>().hitobject = hit.collider.gameObject;
                    OperatorChoosePanel.GetComponent<OperatorChoose>().rayhit = hit;
                    //canAddOperation = false;

                  }
            }


        }

    }

    void addMarkOnMap(RaycastHit hit)
    {
        Transform ghostObjInstance;
        GameObject cube1 = (GameObject)Instantiate(markCommon);

        ghostObjInstance = Instantiate(cube1, cube1.transform.position, Quaternion.identity).GetComponent<Transform>();
        ghostObjInstance.transform.localScale *= scaleChange;
        Debug.Log(scaleChange);
        Vector3 pivotOffsetExtra;
        bool objPivotIsBase = isPivotInBase(ghostObjInstance, out pivotOffsetExtra);

        if (!objPivotIsBase)
        {
            ghostObjInstance = CreateBasePivot(ghostObjInstance, pivotOffsetExtra);
        }
        Vector3 pos = hit.point;
        ghostObjInstance.position = pos;

        AlignGhostToSurface(ghostObjInstance, hit.normal);

        ghostObjInstance = ghostObjInstance.GetComponent<PivotHelper>().DeletePivot();

        GameObject cube = Instantiate(markCommon, ghostObjInstance.position, ghostObjInstance.rotation);
        Destroy(ghostObjInstance.gameObject);
        Destroy(cube1);
        cube.transform.SetParent(hit.collider.gameObject.transform);
        cube.transform.rotation = hit.collider.gameObject.transform.rotation;
        //cube.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
        cube.transform.localScale *= scaleChange;
        gameObject.GetComponent<DpScanMap>().canAddMark = false;
        gameObject.GetComponent<DpScanMap>().markBtn.interactable = true;

        StepDetailAction action = new StepDetailAction(cube.transform, "Common", gameObject.GetComponent<MapObj>().currentFloorName, hit.collider.gameObject.name);
        action.createTime = System.DateTime.Now.ToString();
        Debug.Log(JsonUtility.ToJson(action));
        gameObject.GetComponent<DpScanMap>().markOnMaps.Add(cube);

        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("marks", JsonUtility.ToJson(action));
        DataObj.lbc.LocalPlayer.SetCustomProperties(h);

         

    }


    
}
