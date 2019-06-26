using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
//using UnityEditor;

public class stepDetailPAnel : MonoBehaviour {


    public Button finishBtn;
    public Button addOpBtn;
    public InputField detailIntro;
    public GameObject scrollView;
    public GameObject BackBtn;
    public GameObject AddBtn;
    public GameObject FinishBtn;
    public Button back;
    public Button imgBtn;
    public GameObject imageBtnObj;
    CommonControl _common;
 	// Use this for initialization
	void Start () {

        finishBtn.onClick.AddListener(buttonClicked);
        addOpBtn.onClick.AddListener(addClicked);
        imgBtn.onClick.AddListener(addImage);
        if (DataObj.isScan)
        {
            if (DataObj.isUserOwn)
            {
                detailIntro.enabled = true;
                BackBtn.SetActive(false);
                AddBtn.SetActive(true);
                FinishBtn.SetActive(true);
                imageBtnObj.SetActive(true);
            }
            else
            {
                detailIntro.enabled = false;
                AddBtn.SetActive(false);
                FinishBtn.SetActive(false);
                imageBtnObj.SetActive(false);
                BackBtn.SetActive(true);
                back.onClick.AddListener(buttonClicked);
            }
        }else{
            detailIntro.enabled = true;
            BackBtn.SetActive(false);
            AddBtn.SetActive(true);
            FinishBtn.SetActive(true);
            imageBtnObj.SetActive(true);
        }
    }
    void addImage(){
         _common.gameObject.GetComponent<PaintOnMap>().showPanelAndImage();
        _common.canDrawLine = true;
    }
	
    public void Setup( CommonControl common){

        _common = common;
 
     }

    void addClicked(){

        if (_common.canAddOperation == true)
        {
            _common.canAddOperation = false;
            addOpBtn.interactable = true;
        }
        else
        {
            _common.SimpleShowOrHide();
            _common.canAddOperation = true;
            addOpBtn.interactable = false;

        }
    }







    void buttonClicked(){

        CommonData storeData = _common.gameObject.GetComponent<CommonData>();
        if (DataObj.isScan == false || DataObj.isUserOwn == true)
        {
            if (detailIntro.text.Length == 0)
            {
                MNPopup mNPopup = new MNPopup("Info", "Add some information about this step");
                mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
                mNPopup.Show();
                //EditorUtility.DisplayDialog("Error", "Add some information about this step", "OK");
                return;
            }


            StepDetail detail = storeData.newStrategy.steps[storeData.chosenStepIndex];
            detail.stepDetailInfo = detailIntro.text;

            foreach (GameObject item in storeData.Operators)
            {
                Destroy(item);
            }
        }
        _common.cancelDetailBtn.SetActive(false);
            storeData.finishClicked = true;
           _common.IndetailActions = false;
           Destroy(gameObject);
       


     }



	private void Update()
	{
        if (_common.canAddOperation == false)
        {
            addOpBtn.interactable = true;
        }
    }

}
