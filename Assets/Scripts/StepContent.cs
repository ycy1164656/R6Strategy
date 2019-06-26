using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepContent : MonoBehaviour {

    public GameObject buttonPrefab;

    public Transform contentPanel;
    public GameObject controllerObj;

    CommonData storedData;
	// Use this for initialization
	void Start () {
        storedData = controllerObj.GetComponent<CommonData>();
	}
	
    public void GotoDetailAction(int btnIndex){

        StepDetail item = storedData.newStrategy.steps[btnIndex];
        storedData.chosenStepIndex = btnIndex;
        controllerObj.GetComponent<CommonControl>().IndetailActions = true;
        controllerObj.GetComponent<CommonControl>().EditOrReviewStepDetail(item);

    }

	// Update is called once per frame
	void Update () {
		
        if (storedData.finishClicked)
        {
            storedData.finishClicked = false;

            foreach (Transform button  in contentPanel.GetComponentsInChildren<Transform>())
            {
                if (button.gameObject.tag.Equals("Step"))
                {
                    Destroy(button.gameObject);
                }
            }

            for (int i = 0; i < storedData.newStrategy.steps.Count; i++)
            {
                
                GameObject newobj = (GameObject)Instantiate(buttonPrefab);
                newobj.transform.SetParent(contentPanel);
                newobj.transform.localScale = new Vector3(1f, 1f, 1f);
                StepDetail item = storedData.newStrategy.steps[i];
                StepBtn nameBtn = newobj.GetComponent<StepBtn>();
                nameBtn.btnIndex = i;
                nameBtn.btnName.text = item.stepDetailInfo;
                nameBtn.stepContent = this;

            }


        }

    }
}
