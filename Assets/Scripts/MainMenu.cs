using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Parse;
using UnityEngine.Advertisements;


 public class MainMenu : MonoBehaviour {

	// Use this for initialization


    public Text languageText;
    public GameObject englishBtn;
    public GameObject chnBtn;
    public Text aboutText;
    public Text aboutContent;
    public GameObject strategyMenu;
    public GameObject mainMenu;
    public GameObject dpMenu;
    string languageNow = "en";
    public GameObject hud;
    public GameObject dpEntry;
    public Text mapmenu;
    public Text strategymenu;
    public Text operatormenu;
    public Text videomenu;
    public Text dpmenu;
    bool getConfigNow;
    public Text statementText;
    private void Start()
	{
        DataObj.isFree = false;
        Application.targetFrameRate = 60;

        ParseConfig.GetAsync().ContinueWith(t =>
        {
                ParseConfig config = t.Result;
               getConfigNow = true;
        });

         

        DataObj.cachePath = Application.persistentDataPath + "/";

        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    DataObj.cachePath = Application.dataPath + "/Resources/";
        //}else{
        //    DataObj.cachePath = Application.persistentDataPath + "/Resources/Cache/";
        //}

        //Advertisement.Initialize("2801060", true);
        if (DataObj.isFree)
        {
            dpEntry.SetActive(false);
        }

        languageNow = PlayerPrefs.GetString("language");
         changeAllText();


    }

    public void gotoPrivacy(){
        Application.OpenURL("https://www.jianshu.com/p/f01c2372d789");
    }

    private void Update()
    {
        if (DataObj.shouHud)
        {
            hud.SetActive(true);
        }
        else
        {
            hud.SetActive(false);
        }

        if (getConfigNow)
        {
            getConfigNow = false;
            string iosgameId;
            ParseConfig.CurrentConfig.TryGetValue("iOSGameId", out iosgameId);
            Debug.Log(iosgameId);
            string androidgameId;
            ParseConfig.CurrentConfig.TryGetValue("androidGameId", out androidgameId);

            string statementString;
            ParseConfig.CurrentConfig.TryGetValue("statement", out statementString);

            if (statementString.Length > 0)
            {
                statementText.text = statementString;
            }else{
                statementText.text = "";
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize(androidgameId, false);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize(iosgameId, false);
            }
            //Advertisement.Initialize(iosgameId, true);
        }

    }

    public void gotoMapScene(){
        if (DataObj.isFree)
        {
            if (DataObj.openTimes % 2 == 0)
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (Advertisement.IsReady())
                    {
                        Advertisement.Show("video");
                    }

                }
            }
           

        }
        DataObj.openTimes++;
        StartCoroutine(gotoMapSceneLater());
    }


    IEnumerator gotoMapSceneLater()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MapScene");
    }


    public void gotoUserScene()
    {
        SceneManager.LoadScene("UserScene");
    }
     
    public void goToStrategyMenu(){
        if (ParseUser.CurrentUser != null)
        {
            strategyMenu.SetActive(true);
            mainMenu.SetActive(false);
        }else{
            gotoUserScene();
        }
    }
    public void goToDpMenu()
    {
        if (ParseUser.CurrentUser != null)
        {
            dpMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            gotoUserScene();
        }
    }

    public void changeLanguage(Text btnTitle){

        if (btnTitle.text.Equals("English"))
        {
             PlayerPrefs.SetString("language", "en");

        }else if (btnTitle.text.Equals("中文"))
        {
             PlayerPrefs.SetString("language", "ch");
        }
        PlayerPrefs.Save();

        changeAllText();
    }

    void changeAllText(){

         
            
            languageNow = PlayerPrefs.GetString("language");
            if (languageNow.Equals("en"))
            {
            mapmenu.text = "Maps";
            strategymenu.text = "Strategy";
            operatormenu.text = "Operator";
            videomenu.text = "Video";
            dpmenu.text = "DeadPartner";
            languageText.text = "Language";
                aboutText.text = "About";
            aboutContent.text = "I think Rainbow Six Siege is more than a shoot game,the strategy of this game is also an important part,so I made this app during my part-time for almost half year all by myself. There may be still some bugs or inadequacies. If you have any suggestions please contact me.Thanks!       Email: ycy1164656 @gmail.com   Wechat:ycy1164656";
            }
            else if (languageNow.Equals("ch"))
            {
            mapmenu.text = "地图";
            strategymenu.text = "策略";
            operatormenu.text = "干员";
            videomenu.text = "视频";
            dpmenu.text = "死亡伙伴";
            languageText.text = "语言";
                aboutText.text = "关于";
            aboutContent.text = "我认为彩虹六号围攻不只是一个射击游戏，战术策略也是它很重要的一部分。所以我用我的业余时间花了大半年做了这个应用。应用可能还存在不少漏洞或不足之处。如果您有什么建议，请联系我,谢谢！  Email:ycy1164656@gmail.com   Wechat:ycy1164656";
            }

    }

	



}
