using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 
[System.Serializable]
public class OperatorObj
{
    public string name;
    public GameObject operatorPrefab;
    public Button operatorBtn;
}

public class OperatorChoose : MonoBehaviour {

    public CommonControl controlScript;
    public Vector3 hitPoint;
    public GameObject hitobject;
    public RaycastHit rayhit;

    Transform ghostObjInstance;
    //public GameObject btnPrefab;

    public List<OperatorObj> operators;
    //static public List<OperatorObj> staticOperators;

	// Use this for initialization
	void Start () {
        //OperatorChoose.staticOperators = operators;

    }
	
    public void buttonClicked(GameObject btnObj){

        if (DataObj.isDp)
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("operator", btnObj.name);
            DataObj.lbc.LocalPlayer.SetCustomProperties(h);
          
            gameObject.SetActive(false);
            return;
        }

        controlScript.canAddOperation = false;

        OperatorObj operatorobj = new OperatorObj();
        foreach (OperatorObj op in operators)
        {
            if (op.name.Equals(btnObj.name))
            {
                operatorobj = op;
            }
        }


        GameObject cube1 = (GameObject)Instantiate(operatorobj.operatorPrefab);
         
        ghostObjInstance = Instantiate(cube1, cube1.transform.position, Quaternion.identity).GetComponent<Transform>();
        ghostObjInstance.transform.localScale *= controlScript.scaleChange;
        Debug.Log(controlScript.scaleChange);
        Vector3 pivotOffsetExtra;
        bool objPivotIsBase = controlScript.isPivotInBase(ghostObjInstance, out pivotOffsetExtra);

        if (!objPivotIsBase)
        {
            ghostObjInstance = controlScript.CreateBasePivot(ghostObjInstance, pivotOffsetExtra);
         }
         Vector3 pos = rayhit.point;
        ghostObjInstance.position = pos;

        controlScript.AlignGhostToSurface(ghostObjInstance, rayhit.normal);

        ghostObjInstance = ghostObjInstance.GetComponent<PivotHelper>().DeletePivot();

        GameObject cube =  Instantiate(operatorobj.operatorPrefab,ghostObjInstance.position,ghostObjInstance.rotation);
        Destroy(ghostObjInstance.gameObject);
        Destroy(cube1);
        cube.transform.SetParent(hitobject.transform);
        cube.transform.rotation = hitobject.transform.rotation;
        //cube.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
        cube.transform.localScale *= controlScript.scaleChange;

        string currentFloor = controlScript.gameObject.GetComponent<MapObj>().currentFloorName;

        StepDetailAction action = new StepDetailAction(cube.transform, "Operator",currentFloor,hitobject.name);
        action.createTime = System.DateTime.Now.ToString();
        action.actionName = operatorobj.name;
        CommonData storeData = controlScript.gameObject.GetComponent<CommonData>();
        StepDetail detail = storeData.newStrategy.steps[storeData.chosenStepIndex];
        if (detail.detailActions != null)
        {
            detail.detailActions.Add(action);
        }else{
            detail.detailActions = new List<StepDetailAction>();
            detail.detailActions.Add(action);
        }
       
        OperatorData cubeData = cube.GetComponent<OperatorData>();
        cubeData.createTime = action.createTime;
        cubeData.actionName = operatorobj.name;
        cubeData.parentFloor = currentFloor;
        cubeData.actionType = "Operator";

        stepDetailPAnel panelScript = controlScript.detailPanel.GetComponent<stepDetailPAnel>();
        panelScript.scrollView.GetComponent<DetailStepScrollview>().storeData = storeData;

        storeData.Operators.Add(cube);


        Debug.Log(JsonUtility.ToJson(storeData));
        storeData.isUpdated = true;

        //controlScript.alreadyHavepoint = false;
        this.gameObject.SetActive(false);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
