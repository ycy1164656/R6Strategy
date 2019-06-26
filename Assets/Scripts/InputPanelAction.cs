using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class InputPanelAction : MonoBehaviour {

    public GameObject ControlObj;
     public InputField textField;
    public Text descriptionText;
     GameObject operatorObj;
	// Use this for initialization
	void Start () {
 
    }
	

    public void Setup(GameObject actionObj){

        operatorObj = actionObj;

        //worldCanvas.name = "InfoCanvas";

        //float newy = hitpoint.y + 25.0f;
        //worldCanvas.transform.position = new Vector3(hitpoint.x, newy, hitpoint.z);
        //textCanvas = worldCanvas;
       

    }

    public void confirmAction(){

        if (ControlObj.GetComponent<CommonControl>().canDrawLine || ControlObj.GetComponent<PaintOnMap>().isModify)
        {
            descriptionText.text = textField.text;
            gameObject.SetActive(false);
            ControlObj.GetComponent<PaintOnMap>().isModify = false;
        }
        else
        {

            if (textField.text.Length == 0)
            {
                MNPopup mNPopup = new MNPopup("Info", "Add some information about this step");
                mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
                mNPopup.Show();
                //EditorUtility.DisplayDialog("Error", "Add some information about this step", "OK", "Cancel");
                return;
            }

            OperatorData objdata = operatorObj.GetComponent<OperatorData>();
            CommonData storeData = ControlObj.GetComponent<CommonData>();
            StepDetail detail = storeData.newStrategy.steps[storeData.chosenStepIndex];
            Debug.Log(JsonUtility.ToJson(objdata));

            //Text infotext = textCanvas.GetComponentInChildren<Text>();
            //infotext.text = "Detail";
            //int stepIndex = objdata.actionNumber;
            foreach (StepDetailAction step in detail.detailActions)
            {
                if (step.createTime.Equals(objdata.createTime))
                {
                    step.actionInfo = textField.text;

                }
            }




            objdata.actionInfo = textField.text;
            Debug.Log(JsonUtility.ToJson(storeData));
            textField.text = "";
            storeData.isUpdated = true;

            gameObject.SetActive(false);
        }
    }


    public void cancelAction(){

        gameObject.SetActive(false);

    }
	 
}
