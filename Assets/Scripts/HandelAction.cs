using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandelAction : MonoBehaviour {

    public Button AddInfoBtn;
    public Button DeleteBtn;
    public GameObject actionObj;
    public CommonControl controlScript;
    //public GameObject worldCanvas;
    //public Vector3 hitPoint;
    // Use this for initialization

    public GameObject InputPanel;
    public Text addInfoText;
    string existInfo;
    public GameObject directionArrowPrefab;
    public GameObject doneEditArrowBtn;
    public Button doneBtn;
    public Text AddArrowText;
    bool hasArrow;
    GameObject tempArrow;

	void Start () {
        //AddInfoBtn.onClick.AddListener(addActionInfo);
        DeleteBtn.onClick.AddListener(deleteAction);
        doneBtn.onClick.AddListener(doneEdit);

    }

    void doneEdit(){
        controlScript.doneEditArrow(tempArrow);
    }

    public void editSetup(){
        CommonData storedata = controlScript.gameObject.GetComponent<CommonData>();
        StepDetail detailsteps = storedata.newStrategy.steps[storedata.chosenStepIndex];
        string createtime = actionObj.GetComponent<OperatorData>().createTime;
        StepDetailAction tempAction = new StepDetailAction();

        foreach (StepDetailAction step in detailsteps.detailActions)
        {
            if (step.createTime.Equals(createtime))
            {
                tempAction = step;
            }
        }
        if (tempAction.actionInfo != null)
        {
            addInfoText.text = @"Description";
            existInfo = tempAction.actionInfo;
        }
        else
        {
            addInfoText.text = @"Description";
            existInfo = "";
        }
        hasArrow = tempAction.hasArrow;
        if (tempAction.hasArrow)
        {
            AddArrowText.text = @"Arrow";
         }
        else
        {
            AddArrowText.text = @"Arrow";
        }

    }

    public void SetUpInputPanel(){
        //GameObject textCanvas = (GameObject)Instantiate(worldCanvas);
        InputPanel.SetActive(true);
        InputPanelAction actionScript = InputPanel.GetComponent<InputPanelAction>();
        if (existInfo.Length > 0)
        {
            actionScript.textField.text = existInfo;
        }
        actionScript.Setup(actionObj);
      

        gameObject.SetActive(false);
    }

     
    public void AddOrEditDirectionArrow(){

        if (hasArrow)
        {
            foreach (Transform arrowTransform in actionObj.transform.GetComponentsInChildren<Transform>())
            {
                if (arrowTransform.gameObject.name.Equals("arrow"))
                {
                    GameObject arrow = arrowTransform.gameObject;
                    arrow.AddComponent<ArrowControl>();
                    arrow.GetComponent<ArrowControl>().objCreateTime = actionObj.GetComponent<OperatorData>().createTime;
                    tempArrow = arrow;
                }
            }


        }
        else
        {
            GameObject arrow = (GameObject)Instantiate(directionArrowPrefab);
            arrow.name = "arrow";
            arrow.transform.SetParent(actionObj.transform);
            arrow.transform.localPosition = new Vector3(0, 1.1f, 0);
            arrow.transform.localScale = new Vector3(30.0f, 30.0f, 30.0f);
            arrow.AddComponent<ArrowControl>();
            arrow.GetComponent<ArrowControl>().objCreateTime = actionObj.GetComponent<OperatorData>().createTime;
            tempArrow = arrow;
        }
        controlScript.arrowControl = true;
        doneEditArrowBtn.SetActive(true);

        gameObject.SetActive(false);

    }

    void deleteAction(){
        CommonData storedata = controlScript.gameObject.GetComponent<CommonData>();
        StepDetail detailsteps = storedata.newStrategy.steps[storedata.chosenStepIndex];
        string createtime = actionObj.GetComponent<OperatorData>().createTime;


        StepDetailAction tempAction = new StepDetailAction();

        foreach (StepDetailAction step in detailsteps.detailActions)
        {
            if (step.createTime.Equals(createtime))
            {
                tempAction = step;
            }
        }
        //detailsteps.detailActions = newList;
        detailsteps.detailActions.Remove(tempAction);


        storedata.isUpdated = true;
        Destroy(actionObj);
        this.gameObject.SetActive(false);
    }
	
    public void closeThisPanel(){
        gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
