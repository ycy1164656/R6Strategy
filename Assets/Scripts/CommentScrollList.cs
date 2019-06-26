using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.Linq;
using System;

public class CommentScrollList : MonoBehaviour {

    public GameObject commentItemPrefab;
    public Transform contentPanel;
    List<ParseObject> results = new List<ParseObject>();
    bool getData;
    public InputField commentField;
    public GameObject confirmBtn;
    public ScrollRect scrollview;
    int amountNow;
    bool loadMore = true;
    List<ParseObject> tempResutl = new List<ParseObject>();
    // Use this for initialization
    string queryString = "strategy";
	void Start () {
        if (DataObj.isRecommend)
        {
            queryString = "rStrategy";
        }
        confirmBtn.SetActive(false);
        getCommentFromServer();
        commentField.onValueChanged.AddListener(delegate
        {
            commentTyped();
        });
	}

    void getCommentFromServer(){
        amountNow = 0;
        var query = ParseObject.GetQuery("Comment")
                               .WhereEqualTo(queryString, DataObj.strategy).Include(queryString).Include("user");
       query = query.Limit(10);
        amountNow += 10;
        query.FindAsync().ContinueWith(t =>
        {
            results = t.Result.ToList();
            getData = true;
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });
    }

    void getMoreCommentFromServer()
    {
        if (results.Count % 10 != 0)
        {
            return;
        }
        Debug.Log("loadMore!");
        var query = ParseObject.GetQuery("Comment")
                               .WhereEqualTo(queryString, DataObj.strategy).Include(queryString).Include("user");
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
            
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }

        });
    }

     
    void commentTyped(){
        if (commentField.text.Length > 0)
        {
            confirmBtn.SetActive(true);
        }else{
            confirmBtn.SetActive(false);
        }

     }

    public void confirmComment(){

        ParseObject strategy = new ParseObject("Comment");
        if (DataObj.isRecommend)
        {
            strategy["rStrategy"] = DataObj.strategy;
        }else{
            strategy["strategy"] = DataObj.strategy;
        }

        strategy["user"] = ParseUser.CurrentUser;
        strategy["content"] = commentField.text;
        strategy.SaveAsync().ContinueWith(t => {
              if (t.IsFaulted)
             {
                 // Errors from Parse Cloud and network interactions
                 using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                 {
                     if (enumerator.MoveNext())
                     {
                         ParseException error = (ParseException)enumerator.Current;
                         // error.Message will contain an error message
                         // error.Code will return "OtherCause"
                         UnityMainThreadDispatcher.Instance().Enqueue(showErrorWithMessage(error.Message));
                     }
                 }
             }
             else
             {
                commentField.text = "";
                getData = true;
                DataObj.strategy.Increment("commentNum");
                DataObj.strategy.SaveAsync();
            }
         });
        results.Add(strategy);
       
    }


     
    IEnumerator showErrorWithMessage(string message)
    {
        MNPopup mNPopup = new MNPopup("Error", message);
        mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
        mNPopup.Show();
        yield return null;
    }

    IEnumerator showSuccessWithMessage(string message)
    {
        MNPopup mNPopup = new MNPopup("Succeed!", message);
        mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
        mNPopup.Show();
        yield return null;
    }

	private void Update()
	{
         
        if (scrollview.normalizedPosition.y <0.1 && loadMore == false)
        {
            loadMore = true;
            getMoreCommentFromServer();
        }

         if (getData)
        {
            AddButtonList();
            getData = false;
        }
    }


	void AddButtonList()
    {
        foreach (Transform child in contentPanel.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("CommentItem"))
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
            Debug.Log(results);
            ParseObject obj = results[i];
            ParseUser user = (ParseUser)obj["user"];

            //Debug.Log(obj["stars"].GetType());
            GameObject newobj;
            newobj = (GameObject)Instantiate(commentItemPrefab);
            newobj.transform.SetParent(contentPanel);
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            newobj.name = "CommentItem";
            CommentItem map = newobj.GetComponent<CommentItem>();
            map.createRow((string)user["nickName"], obj.CreatedAt.ToString(), (string)obj["content"]);
            if (i == results.Count - 1)
            {
                loadMore = false;
            }
        }

         
    }
}
