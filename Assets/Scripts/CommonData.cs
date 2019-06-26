using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;


[System.Serializable]
public class SavableTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localscale;
    public Vector3 localposition;
    public Quaternion localrotation;
}

[System.Serializable]
public class StepDetailAction
{
    public SavableTransform mainTransform = new SavableTransform();
    public SavableTransform directionTransform = new SavableTransform();
    public string actionInfo;
    public string actionType;
    public string parentFloor;
    public string parentName;
    public string createTime;
    public string actionName;
    public bool hasArrow;
    public string stepImageUrl;
    public bool imageType;
    public byte[] imageData;

 

    public StepDetailAction(Transform _transform,string _type, string _parent,string _name){

        mainTransform.rotation = _transform.rotation;
        mainTransform.localscale = _transform.localScale;
        mainTransform.localposition = _transform.localPosition;
        mainTransform.localrotation = _transform.localRotation;
        //mainTransform = _transform;
        
        actionType = _type;
        parentFloor = _parent;
        parentName = _name;
    }
    public StepDetailAction()
    {
        
    }

}

[System.Serializable]
public class StepDetail
{
    public string stepDetailInfo;
    public List<StepDetailAction> detailActions;
    public int stepIndex;

}

[System.Serializable]
public class StrategySteps
{
    public string stepInfo;
    public string strategyTitle;
    public List<StepDetail> steps;
}


public class CommonData : MonoBehaviour {


    public StrategySteps newStrategy = new StrategySteps();
    public bool isUpdated = false;
    public bool finishClicked = false;
    public int chosenStepIndex = 0;
    public List<GameObject> Operators = new List<GameObject>();
    public Text starText;
    public Text CommentText;
    public Image starImage;
    ParseObject foundObj;
    public List<ParseFile> imageFiles;
    bool staredThis;
	// Use this for initialization
	void Start () {

        if (DataObj.isScan)
        {
            newStrategy = JsonUtility.FromJson<StrategySteps>((string)DataObj.strategy["strategyString"]);
            isUpdated = true;
            finishClicked = true;
          
            gameObject.GetComponent<CommonControl>().ScanData();

            starText.text = ((long)DataObj.strategy["stars"]).ToString();
            CommentText.text = ((long)DataObj.strategy["commentNum"]).ToString();


            var query = ParseObject.GetQuery("StrategyStar").WhereEqualTo("user", ParseUser.CurrentUser).WhereEqualTo("strategy",DataObj.strategy);
             query.FirstAsync().ContinueWith(t =>
            {
                foundObj = t.Result;
                staredThis = true;
            });

        }


    }

    public void starBtnClicked(){

        if (foundObj != null)
        {
            foundObj.DeleteAsync().ContinueWith(t => {
                if (t.IsCompleted)
                {
                    foundObj = null;
                    DataObj.strategy.Increment("stars",-1);
                    DataObj.strategy.SaveAsync();
                    staredThis = true;
                }
            });
            starImage.sprite = Resources.Load<Sprite>("Images/unstar");
        }else{
            ParseObject gameScore = new ParseObject("StrategyStar");
            gameScore["user"] = ParseUser.CurrentUser;
            if (DataObj.isRecommend)
            {
                gameScore["rStrategy"] = DataObj.strategy;
            }
            else{
                gameScore["strategy"] = DataObj.strategy;
            }

            gameScore.SaveAsync().ContinueWith(t => {

                if (t.IsCompleted)
                {
                    DataObj.strategy.Increment("stars");
                    DataObj.strategy.SaveAsync();
                    foundObj = gameScore;
                    staredThis = true;

                }

            });  
        }


    }

	private void Update()
	{
        if (staredThis)
        {
            if (foundObj!= null)
            {
                starImage.sprite = Resources.Load<Sprite>("Images/stared");
            }

            starText.text = ((long)DataObj.strategy["stars"]).ToString();
            CommentText.text = ((long)DataObj.strategy["commentNum"]).ToString();
            staredThis = false;
        }
    }

	// Update is called once per frame

}
