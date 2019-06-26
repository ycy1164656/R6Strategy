using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System;
using System.Linq;

public class VideoScroll : MonoBehaviour {

    public Transform contentRect;
    public GameObject btnPrefab;
    public List<Dictionary<string,object>> videos;
    // Use this for initialization
    public Text mapName;
    bool getData;
    int amountNow;
    bool loadMore = true;
    List<ParseObject> results;
    List<ParseObject> tempResutl = new List<ParseObject>();
    public ScrollRect scrollview;

    public InputField videoUrl;
    public InputField imageUrl;
    public InputField videoTitle;
    public GameObject inputPanel;

    bool saveSucceed;


    public void confirmUpload(){

        if (videoUrl.text.Length == 0 || imageUrl.text.Length == 0 || videoTitle.text.Length == 0  )
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();
         }else{

            ParseObject newvideo = new ParseObject("Video");
            newvideo["title"] = videoTitle.text;
            newvideo["url"] = videoUrl.text;
            newvideo["imageUrl"] = imageUrl.text;
            newvideo["mapName"] = mapName.text;
            newvideo.SaveAsync().ContinueWith(t =>
            {
                //MNP.HidePreloader();
                // Now let's update it with some new data.  In this case, only cheatMode
                // and score will get sent to the cloud.  playerName hasn't changed.
                saveSucceed = true;

            });

        }


    }

    public void setupAndGetVideosFromServer(string mapname){
        mapName.text = mapname;
        amountNow = 0;
        var query = ParseObject.GetQuery("Video")
                               .WhereEqualTo("mapName", mapname).WhereEqualTo("canShow",true).OrderByDescending("createdAt");
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


    void getMoreStrategiesFromServer()
    {

        if (results.Count % 10 != 0)
        {
            return;
        }
        Debug.Log("loadMore!");
        //MNP.ShowPreloader("", "loading...");
        var query = ParseObject.GetQuery("Video")
                               .WhereEqualTo("mapName", mapName.text).WhereEqualTo("canShow", true).OrderByDescending("createdAt");


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



    public void addButtonsOnpanel(){

        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("videoItem"))
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < videos.Count; i++)
        {
            GameObject newobj;
            newobj = (GameObject)Instantiate(btnPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.name = "videoItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            Dictionary<string,object> dic = videos[i];
            VideoBtn map = newobj.GetComponent<VideoBtn>();
            map.Setup(dic, this);

        }

    }

    public void AddButtonList()
    {

        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("videoItem"))
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
            GameObject newobj;
            newobj = (GameObject)Instantiate(btnPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.name = "videoItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            ParseObject dic = results[i];
            VideoBtn map = newobj.GetComponent<VideoBtn>();
            map.Setup(dic, this);

        }

    }


    public void gotoVideoPanel(string urlString){
        Application.OpenURL(urlString);
    }
	// Update is called once per frame
	void Update () {

        if (saveSucceed)
        {
            saveSucceed = false;
            inputPanel.SetActive(false);
        }


        if (scrollview.normalizedPosition.y < 0.1 && loadMore == false)
        {
            loadMore = true;
            getMoreStrategiesFromServer();
        }
        if (getData)
        {
            AddButtonList();
            getData = false;

        }
    }
}
