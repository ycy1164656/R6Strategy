using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour {



    public Text star;
    public Text time;


    string now;
	// Use this for initialization
	void Start () {
        now = PlayerPrefs.GetString("language");
        changeStrings();
	}
	
	// Update is called once per frame
	void Update () {
        string then = PlayerPrefs.GetString("language");
        if (!now.Equals(then))
        {
            now = then;
            changeStrings();
        }
    }


    void changeStrings(){

        if (now.Equals("en"))
        {
           
            star.text = "Star";
            time.text = "Time";
           
        }else if (now.Equals("ch"))
        {
           
            star.text = "赞";
            time.text = "时间";

        }

    }

    public string translateMapName(string originalName){
        string newName = originalName;

        if (now.Equals("en"))
        {
            
        }else if (now.Equals("ch"))
        {
            if (originalName.Equals("House"))
            {
                newName = "芝加哥豪宅";
            }
            if (originalName.Equals("Bank"))
            {
                newName = "银行";
            }
            if (originalName.Equals("Hereford"))
            {
                newName = "赫里福德基地";
            }
            if (originalName.Equals("Tower"))
            {
                newName = "塔楼";
            }
            if (originalName.Equals("Villa"))
            {
                newName = "别墅";
            }
            if (originalName.Equals("Skyscraper"))
            {
                newName = "摩天大楼";
            }
            if (originalName.Equals("ThemePark"))
            {
                newName = "主题公园";
            }
            if (originalName.Equals("KafeDostoyevsky"))
            {
                newName = "杜斯妥也夫斯基咖啡馆";
            }
            if (originalName.Equals("Kanal"))
            {
                newName = "运河";
            }
            if (originalName.Equals("Yacht"))
            {
                newName = "游艇";
            }
            if (originalName.Equals("Consulate"))
            {
                newName = "大使馆";
            }
            if (originalName.Equals("Border"))
            {
                newName = "边境";
            }
            if (originalName.Equals("University"))
            {
                newName = "大学";
            }
            if (originalName.Equals("Oregon"))
            {
                newName = "俄勒冈";
            }
            if (originalName.Equals("Chalet"))
            {
                newName = "木屋";
            }
            if (originalName.Equals("ClubHouse"))
            {
                newName = "俱乐部会所";
            }
            if (originalName.Equals("Plane"))
            {
                newName = "总统专机";
            }
            if (originalName.Equals("Coastline"))
            {
                newName = "海岸线";
            }
            if (originalName.Equals("Hereford Rework"))
            {
                newName = "新基地";
            }
            if (originalName.Equals("Favela"))
            {
                newName = "贫民窟";
            }
            if (originalName.Equals("Common"))
            {
                newName = "通用";
            }


        }


        return newName;
    }


}
