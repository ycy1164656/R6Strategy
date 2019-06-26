using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapItem : MonoBehaviour {

	public Button buttonComponent;
	public Text nameLabel;
	public Image iconImage;
    public string originalName;

	private ItemMap item;
	private MapListScrollview scrollList;


	// Use this for initialization
	void Start () {
		buttonComponent.onClick.AddListener (btnClicked);
	}
	
	public void Setup(ItemMap currentItem,MapListScrollview scroll){

        Translator translator = Camera.main.GetComponent<Translator>();
		item = currentItem;
        nameLabel.text = translator.translateMapName(item.itemName);
        originalName = item.itemName;
		iconImage.sprite = item.iconImage;
		scrollList = scroll;
        iconImage.preserveAspect = true;

	}

	public void btnClicked()
	{
         
            scrollList.GotoMapDetail(this);



	}
}
