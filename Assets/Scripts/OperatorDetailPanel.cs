using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.IO;
using System.Linq;

public class OperatorDetailPanel : MonoBehaviour {

    // Use this for initialization
    public GameObject videoPanel;
    public GameObject currentPanel;
    public Transform contentRect;
    public GameObject headerPrefab;
    public GameObject weaponPrefab;
    public GameObject equipPrefab;
    public GameObject operatorStrategyPanel;
    public GameObject operatorStrategyDetail;
    public Text nameText;
    public Image realImage;
    public Text realName;
    public Text affiliation;
    public Text armor;
    public Text speed;
    public Text ability;
    public Text intro;
    ParseObject detailObj;
    public GameObject videoContent;
    public GameObject operatorPanel;

    public void setupWithObj(ParseObject obj){
        detailObj = obj;
        nameText.text = obj["name"] as string;
        realName.text = obj["realName"] as string;
        if (File.Exists(DataObj.cachePath + (obj["bodyUrl"] as string).GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(obj["bodyUrl"] as string, realImage));
        }
        else
        {
            StartCoroutine(DownloadImage(obj["bodyUrl"] as string, realImage));
        }
        realImage.preserveAspect = true;
 
        //StartCoroutine(DownloadImage(obj["bodyUrl"] as string, realImage));
        affiliation.text = obj["Affiliation"] as string;
        armor.text = "Armor:" + obj["armor"] as string;
        speed.text = "Speed:" + obj["speed"] as string;
        ability.text = "Ability:" + obj["Ability"] as string;
        intro.text = "Background:" + obj["Intro"] as string;
        addPrimary();
        addSecond();
        addEquipment();
    }



    public void addPrimary(){
        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("primaryHeader") || child.gameObject.name.Equals("pWeaponItem"))
            {
                Destroy(child.gameObject);
            }
        }
        GameObject header;
        header = (GameObject)Instantiate(headerPrefab);
        header.transform.SetParent(contentRect);
        header.name = "primaryHeader";
        header.GetComponent<Text>().text = "PrimaryWeapon";
        header.transform.localScale = new Vector3(1f, 1f, 1f);

        List<ParseObject> pWeapon =
            ((List<object>)detailObj["Maingun"]).OfType<ParseObject>().ToList(); 

         for (int i = 0; i < pWeapon.Count; i++)
        {
            Debug.Log(pWeapon[i]);
            GameObject newobj;
            newobj = (GameObject)Instantiate(weaponPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.name = "pWeaponItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);

            ParseObject obj = pWeapon[i];
            Weapon map = newobj.GetComponent<Weapon>();
            map.Setup(obj);

        }
    }
    public void addSecond()
    {
        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("secondHeader") || child.gameObject.name.Equals("sWeaponItem"))
            {
                Destroy(child.gameObject);
            }
        }
        GameObject header;
        header = (GameObject)Instantiate(headerPrefab);
        header.transform.SetParent(contentRect);
        header.name = "secondHeader";
        header.GetComponent<Text>().text = "SecondaryWeapon";
        header.transform.localScale = new Vector3(1f, 1f, 1f);




        List<ParseObject> pWeapon =
            ((List<object>)detailObj["Secondgun"]).OfType<ParseObject>().ToList();

        for (int i = 0; i < pWeapon.Count; i++)
        {
            GameObject newobj;
            newobj = (GameObject)Instantiate(weaponPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.name = "sWeaponItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            ParseObject obj = pWeapon[i];
            Weapon map = newobj.GetComponent<Weapon>();
            map.Setup(obj);

        }
    }
    public void addEquipment()
    {
        foreach (Transform child in contentRect.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("equipHeader") || child.gameObject.name.Equals("eWeaponItem"))
            {
                Destroy(child.gameObject);
            }
        }
        GameObject header;
        header = (GameObject)Instantiate(headerPrefab);
        header.transform.SetParent(contentRect);
        header.name = "equipHeader";
        header.GetComponent<Text>().text = "Equipment";
        header.transform.localScale = new Vector3(1f, 1f, 1f);

        List<ParseObject> pWeapon =
            ((List<object>)detailObj["equipment"]).OfType<ParseObject>().ToList();

        for (int i = 0; i < pWeapon.Count; i++)
        {
            GameObject newobj;
            newobj = (GameObject)Instantiate(equipPrefab);
            newobj.transform.SetParent(contentRect);
            newobj.name = "eWeaponItem";
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            ParseObject obj = pWeapon[i];
            Equipment map = newobj.GetComponent<Equipment>();
            map.Setup(obj);

        }
    }

    IEnumerator DownloadImage(string url, Image image)
    {
        WWW www = new WWW(url);

        yield return www;

        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();
        File.WriteAllBytes(DataObj.cachePath + url.GetHashCode(), pngData);

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }

    IEnumerator LoadLocalImage(string url, Image image)
    {
        // 已在本地缓存
        string filePath = "file:///" + DataObj.cachePath + url.GetHashCode();
        WWW www = new WWW(filePath);
        yield return www;
        Texture2D tex2d = www.texture;

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }
    public void gotoVideoPanel(){
        currentPanel.SetActive(false);
        videoPanel.SetActive(true);
        List<Dictionary<string,object>> pWeapon =
            ((List<object>)detailObj["videos"]).OfType<Dictionary<string, object>>().ToList();
        videoContent.GetComponent<VideoScroll>().videos = pWeapon;
        videoContent.GetComponent<VideoScroll>().addButtonsOnpanel();
    }

    public void gotoOperatorStrategyPanel()
    {
        operatorStrategyPanel.SetActive(true);
        operatorStrategyDetail.GetComponent<StrategyDetailList>().operatorName = nameText.text;
        operatorStrategyDetail.GetComponent<StrategyDetailList>().GetOperatorStrategiesFormServer();
        operatorPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
