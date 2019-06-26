using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;

public class StrategyDetailItem : MonoBehaviour {

    public Text titleText;
    public Text detailText;
    public Button buttonComponent;
    public Text starText;
    public Text numberIdText;
    ParseObject strategyObj;
    StrategyDetailList scroll;
	// Use this for initialization
	void Start () {
        buttonComponent.onClick.AddListener(btnClick);
	}
	
    public void Setup(ParseObject item,StrategyDetailList detailList){
        strategyObj = item;
        StrategySteps newS = JsonUtility.FromJson<StrategySteps>((string)item["strategyString"]);
        titleText.text = newS.strategyTitle;
        detailText.text = newS.stepInfo;
        starText.text = ((long)item["stars"]).ToString();
        numberIdText.text = (string )item["numberId"];
        scroll = detailList;
    }

    void btnClick()
    {
        scroll.GotoMapDetail(strategyObj);
    }
	 
}
