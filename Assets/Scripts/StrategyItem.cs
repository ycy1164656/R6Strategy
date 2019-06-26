using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategyItem : MonoBehaviour {

    public Image iconImage;
    public Button addBtn;
    public Button browseBtn;
    public Text titleText;
    // Use this for initialization
    StrategyList scrollList;
    public GameObject addObj;
    public string originalName;
	void Start () {
        if (DataObj.isFree)
        {
            addObj.SetActive(false);
        }
        addBtn.onClick.AddListener(btnClicked);
        browseBtn.onClick.AddListener(browseClick);
	}
    public void Setup(ItemStoredMap currentItem, StrategyList scroll)
    {
        Translator translator = Camera.main.GetComponent<Translator>();
        titleText.text = translator.translateMapName(currentItem.itemName);
        originalName = currentItem.itemName;
        iconImage.sprite = currentItem.iconImage;
        scrollList = scroll;


    }

    void btnClicked()
    {
        scrollList.AddStrategy(this);
    }

    void browseClick(){
        scrollList.BrowseStrategy(this);
    }
}
