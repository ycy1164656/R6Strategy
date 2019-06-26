using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;


public class StrategyItemOperator : MonoBehaviour {

     public Text titleText;
    public Text descText;
    public Text mapName;
    public Button buttonComponent;
    // Use this for initialization
    ParseObject strategyObj;
    StrategyDetailList scroll;
    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(btnClick);
    }

    public void Setup(ParseObject item, StrategyDetailList detailList)
    {
        strategyObj = item;
        StrategySteps newS = JsonUtility.FromJson<StrategySteps>((string)item["strategyString"]);
        titleText.text = newS.strategyTitle;
        descText.text = newS.stepInfo;
        Translator translator = Camera.main.GetComponent<Translator>();
        mapName.text = translator.translateMapName(item["mapName"] as string);
         
        scroll = detailList;
    }

    void btnClick()
    {
        scroll.GotoMapDetail(strategyObj);
    }
}
