using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using UnityEngine.SceneManagement;

[System.Serializable]

public class ItemStoredMap
{
    public string itemName;
    public Sprite iconImage;

}

public class StrategyList : MonoBehaviour {
    
    public GameObject strategyPrefab;
    public List<ItemStoredMap> mapList;
    public Transform contentPanel;
    public GameObject StrategyMenu;
    public GameObject StrategyDetail;
    public GameObject StategyDetailContent;
    public GameObject strategyMapPanel;
    string now;
	// Use this for initialization
	void Start () {
        AddButtonList();
        now = PlayerPrefs.GetString("language");
	}
	
	// Update is called once per frame

    public void AddStrategy(StrategyItem item){
        DataObj.MapName = item.originalName;
        DataObj.isScan = false;
        strategyMapPanel.SetActive(false);
        DataObj.shouHud = true;
        Camera.main.GetComponent<MainMenu>().gotoMapScene();
    }

    public void BrowseStrategy(StrategyItem item)
    {
        StrategyDetail.SetActive(true);
        StategyDetailContent.GetComponent<StrategyDetailList>().mapName = item.originalName;
        StategyDetailContent.GetComponent<StrategyDetailList>().mapTitle.text = item.originalName;
        StategyDetailContent.GetComponent<StrategyDetailList>().GetStrategiesFormServer();
        StrategyMenu.SetActive(false);
    }

    private void Update()
    {
        string then = PlayerPrefs.GetString("language");
        if (!now.Equals(then))
        {
            now = then;
            foreach (Transform child in contentPanel.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.name.Equals("mapItem"))
                {
                    Destroy(child.gameObject);
                }
            }
            AddButtonList();
        }
    }


    private void AddButtonList()
    {

        for (int i = 0; i < mapList.Count; i++)
        {
            GameObject newobj;
            newobj = (GameObject)Instantiate(strategyPrefab);
            newobj.transform.SetParent(contentPanel);
            newobj.name = "mapItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            ItemStoredMap item = mapList[i];
            StrategyItem map = newobj.GetComponent<StrategyItem>();
            map.Setup(item, this);

        }

    }

	
}
