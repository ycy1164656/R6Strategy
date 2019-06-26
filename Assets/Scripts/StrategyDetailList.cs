using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class StrategyDetailList : MonoBehaviour {

    // Use this for initialization
    public string mapName;
    public GameObject strategyPrefab;
    public List<StrategyDetailItem> itemList;
    public Transform contentRect;
    public Text mapTitle;
    public Button starBtn;
    public Button timeBtn;
    public Button userStarBtn;
    public Button userMineBtn;
    public bool isUserCenter;
    List<ParseObject> results;
    bool getData;
    int amountNow;
    bool loadMore = true;
    List<ParseObject> tempResutl = new List<ParseObject>();
    public ScrollRect scrollview;
    string queryOrder = "stars";
    public InputField searchNumberId;
    public GameObject strategyPanel;
    public GameObject userPanel;
    public GameObject controlObj;
    public string operatorName;
    public void GetStrategiesFormServer(){
        //MNP.ShowPreloader("", "...");
        amountNow = 0;
        var query = ParseObject.GetQuery("Strategy")
                               .WhereEqualTo("mapName", mapName).OrderByDescending(queryOrder);
        amountNow += 10;
        query = query.Limit(10);
        query.FindAsync().ContinueWith(t =>
        {
            results = t.Result.ToList();
            getData = true;

            //MNP.HidePreloader();
            //AddButtonList(results);
             AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });

    }


    public void GetOperatorStrategiesFormServer()
    {
        //MNP.ShowPreloader("", "...");
        mapTitle.text = operatorName;
        amountNow = 0;
        var query = ParseObject.GetQuery("Strategy")
                               .WhereContains("Operators",operatorName).OrderByDescending(queryOrder);
        amountNow += 10;
        query = query.Limit(10);
        query.FindAsync().ContinueWith(t =>
        {
            results = t.Result.ToList();
            getData = true;

            //MNP.HidePreloader();
            //AddButtonList(results);
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });

    }


    public void GetStrategyBySearchId(){
        if (searchNumberId.text.Length == 0)
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();
            return;
        }
        //MNP.ShowPreloader("", "...");
        amountNow = 0;
        var query = ParseObject.GetQuery("Strategy").WhereEqualTo("numberId",searchNumberId.text).WhereEqualTo("mapName", mapName);
        amountNow += 10;
        query = query.Limit(10);
        Debug.Log("searchClicked");
        query.FindAsync().ContinueWith(t =>
        {
            Debug.Log("searchDone");
            results = t.Result.ToList();
            getData = true;
            Debug.Log(results);
            //MNP.HidePreloader();
            //AddButtonList(results);
            //Debug.Log(results);
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });
        searchNumberId.text = "";

    }

    public void GetStrategiesFromServerWithUser()
    {
        //MNP.ShowPreloader("", "...");
        isUserCenter = true;
        amountNow = 0;
        var query = ParseObject.GetQuery("StrategyStar")
                              .WhereEqualTo("user", ParseUser.CurrentUser).Include("strategy");
        query = query.Limit(10);
        amountNow += 10;
        query.FindAsync().ContinueWith(t =>
        {
            results = t.Result.ToList();
            getData = true;
            //MNP.HidePreloader();
            //AddButtonList(results);
            //Debug.Log(results);
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });

    }

    public void GetStrategiesFromServerWithUserOwn()
    {
        //MNP.ShowPreloader("", "...");
        isUserCenter = false;
        amountNow = 0;
        var query = ParseObject.GetQuery("Strategy")
                               .WhereEqualTo("user", ParseUser.CurrentUser);
        query = query.Limit(10);
        amountNow += 10;
        query.FindAsync().ContinueWith(t =>
        {
            results = t.Result.ToList();
            getData = true;
            //AddButtonList(results);
            //Debug.Log(results);
            //MNP.HidePreloader();
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });

    }



    void getMoreStrategiesFromServer()
    {
       
        if (results.Count % 10 != 0)
        {
            return;
        }
        Debug.Log("loadMore!");
        //MNP.ShowPreloader("", "loading...");
        var query = ParseObject.GetQuery("Strategy")
                               .WhereEqualTo("mapName", mapName).OrderByDescending(queryOrder);
        if (isUserCenter)
        {
            query = ParseObject.GetQuery("StrategyStar")
                              .WhereEqualTo("user", ParseUser.CurrentUser).Include("strategy");
        }

        if (userStarBtn!= null && userStarBtn.interactable)
        {
            query = ParseObject.GetQuery("Strategy")
                               .WhereEqualTo("user", ParseUser.CurrentUser);
        }
        if (operatorName != null)
        {
            query = ParseObject.GetQuery("Strategy")
                               .WhereContains("Operators", operatorName).OrderByDescending(queryOrder);
        }

        query = query.Skip(amountNow);
        query = query.Limit(10);
        amountNow += 10;
        query.FindAsync().ContinueWith(t =>
        {
            tempResutl = t.Result.ToList();
            if (tempResutl.Count > 0)
            {
                getData = true;
            }
            //MNP.HidePreloader();
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });
    }


    private void Start()
	{
        if (starBtn != null)
        {
            starBtn.interactable = false;
           
        }

        if (userStarBtn != null)
        {
            userStarBtn.interactable = false;
            queryOrder = "stars";
            GetStrategiesFromServerWithUser();
        }
        
       
	}

	public void starBtnClick(){
        starBtn.interactable = false;
        timeBtn.interactable = true;
        queryOrder = "stars";
        GetStrategiesFormServer();
    }

    public void timeBtnclick(){
        starBtn.interactable = true;
        timeBtn.interactable = false;
        queryOrder = "createdAt";
        GetStrategiesFormServer();
    }

    public void userStarBtnClick()
    {
        userStarBtn.interactable = false;
        userMineBtn.interactable = true;
        GetStrategiesFromServerWithUser();
    }

    public void userMineBtnclick()
    {
        userStarBtn.interactable = true;
        userMineBtn.interactable = false;
        GetStrategiesFromServerWithUserOwn();
    }


    public void GotoMapDetail(ParseObject item)
    {

        DataObj.MapName = (string)item["mapName"];
        DataObj.strategy = item;
        DataObj.isScan = true;
        DataObj.isUserOwn = false;
        if (userMineBtn != null)
        {
            if (userMineBtn.interactable == false)
            {
                DataObj.isUserOwn = true;
            }
            else
            {
                DataObj.isUserOwn = false;
            }

            if (ParseUser.CurrentUser.ObjectId.Equals("dmdnQEFCNi"))
            {
                //supervisor
                DataObj.isUserOwn = true;

            }

            userPanel.SetActive(false);
            controlObj.GetComponent<UserControl>().gotoMap();

            //SceneManager.LoadScene("MapScene");
        }
        else{
            DataObj.shouHud = true;
            strategyPanel.SetActive(false);
            Camera.main.GetComponent<MainMenu>().gotoMapScene();
        }

       
       

    }
	private void Update()
	{
        if (scrollview.normalizedPosition.y < 0.1 && loadMore == false)
        {
            loadMore = true;
            getMoreStrategiesFromServer();
        }
        if (getData)
        {
            AddButtonList(results);
            getData = false;

        }
	}

	void AddButtonList(IEnumerable<ParseObject> rersult)
    {
        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("StrategyDetailItem"))
            {
                Destroy(child.gameObject);
            }
        }
        if (tempResutl.Count > 0)
        {
            results.AddRange(tempResutl);
        }

        for (int i = 0; i < results.Count; i++)
        {
            ParseObject obj = results[i];
            if (isUserCenter)
            {
                ParseObject newObj = results[i];
                obj = (ParseObject)newObj["strategy"];
            }
            //Debug.Log(obj["stars"].GetType());
            GameObject newobj;
            newobj = (GameObject)Instantiate(strategyPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            newobj.name = "StrategyDetailItem";
            if (operatorName.Length > 0)
            {
                StrategyItemOperator map = newobj.GetComponent<StrategyItemOperator>();
                map.Setup(obj, this);
            }else
            {
                StrategyDetailItem map = newobj.GetComponent<StrategyDetailItem>();
                map.Setup(obj, this);
            }
           
            if (i == results.Count - 1)
            {
                loadMore = false;
            }
        }

        //var enumerator = rersult.GetEnumerator();

        //while (enumerator.MoveNext()){
           
        //}


        //for (int i = 0; i < .Count; i++)
        //{
        //    GameObject newobj;
        //    newobj = (GameObject)Instantiate(strategyPrefab);
        //    newobj.transform.SetParent(contentRect);
        //    //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
        //    //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
        //    StrategyDetailItem map = newobj.GetComponent<StrategyItem>();
        //    map.Setup(item, this);

        //}

    } 
	 
}
