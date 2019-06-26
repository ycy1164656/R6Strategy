using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailStep : MonoBehaviour {

    public Text btnTitle;
    public Image btnIcon;
    public Button button;
    public string btnCreateTime;
    public DetailStepScrollview scrollviewScript;
	// Use this for initialization
    void Start()
    {
        button.onClick.AddListener(btnClicked);
    }

    void btnClicked()
    {
        scrollviewScript.RotateToCenterOfScreen(btnCreateTime);
    }
}
