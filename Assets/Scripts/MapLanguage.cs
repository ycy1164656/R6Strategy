using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapLanguage : MonoBehaviour {

    public Text sTitle;
    public Text sDetail;
    public Text descriText;
    public Text arrowText;
    public Text eIntro;
    public Text e1;
    public Text e2;
    public Text e3;


    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetString("language").Equals("ch"))
        {
            sTitle.text = "标题";
            sDetail.text = "战术介绍";
            descriText.text = "描述";
            arrowText.text = "箭头";
            eIntro.text = "在详细步骤页面点击添加按钮后，点击地图来添加干员图标";
            e1.text = "可破坏墙面";
            e2.text = "可提供视野墙面";
            e3.text = "可提供视野地面";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
