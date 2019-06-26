using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using System.Linq;
using UnityEngine.UI;

public class OperatorList : MonoBehaviour
{

    public GameObject operatorPrefab;
    List<ParseObject> results;
    public Transform contentRect;
    bool getData;
    public Button attackBtn;
    public Button defendBtn;
    public GameObject detailPanel;
    public GameObject detailContent;
    bool isAttack = true;
    // Use this for initialization
    void Start()
    {
        getOperatorFromServer();
        attackBtn.interactable = false;
    }



    public void getOperatorFromServer()
    {
        //MNP.ShowPreloader("", "...");
        Debug.Log("getOperator");
        var query = ParseObject.GetQuery("Operator").Include("Maingun").Include("Secondgun").Include("equipment");
         
        query.FindAsync().ContinueWith(t =>
       {
           results = t.Result.ToList();
           getData = true;
       });

    }

    public void attackClicked(){
        attackBtn.interactable = false;
        defendBtn.interactable = true;
        isAttack = true;
        AddButtonList(results);
    }

    public void defendClicked(){
        attackBtn.interactable = true;
        defendBtn.interactable = false;
        isAttack = false;
        AddButtonList(results);
    }


    // Update is called once per frame
    void Update()
    {
        if (getData)
        {
            AddButtonList(results);
            getData = false;

        }
    }

    public void gotoOperatorDetail(ParseObject obj)
    {
        detailPanel.SetActive(true);
        detailContent.GetComponent<OperatorDetailPanel>().setupWithObj(obj);

    }

    void AddButtonList(IEnumerable<ParseObject> rersult)
    {
        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("OperatorItem"))
            {
                Destroy(child.gameObject);
            }
        }


        for (int i = 0; i < results.Count; i++)
        {
            ParseObject obj = results[i];

            //Debug.Log(obj["stars"].GetType());
            if (isAttack)
            {
                if (obj["team"].Equals("Attackers"))
                {
                    GameObject newobj;
                    newobj = (GameObject)Instantiate(operatorPrefab);
                    newobj.transform.SetParent(contentRect);
                    newobj.transform.localScale = new Vector3(1f, 1f, 1f);
                    newobj.name = "OperatorItem";
                    OperatorItem map = newobj.GetComponent<OperatorItem>();
                    map.Setup(obj, this);
                }
            }else{
                if (obj["team"].Equals("Defenders"))
                {
                    GameObject newobj;
                    newobj = (GameObject)Instantiate(operatorPrefab);
                    newobj.transform.SetParent(contentRect);
                    newobj.transform.localScale = new Vector3(1f, 1f, 1f);
                    newobj.name = "OperatorItem";
                    OperatorItem map = newobj.GetComponent<OperatorItem>();
                    map.Setup(obj, this);
                }
            }
           

        }

    }
}
