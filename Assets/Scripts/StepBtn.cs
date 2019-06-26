using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepBtn : MonoBehaviour {

    public Text btnName;
    public Button button;
    public int btnIndex;
    public StepContent stepContent;
	// Use this for initialization
	void Start () {
        button.onClick.AddListener(btnClicked);
	}
	
    void btnClicked(){
        stepContent.GotoDetailAction(btnIndex);
    }

	
}
