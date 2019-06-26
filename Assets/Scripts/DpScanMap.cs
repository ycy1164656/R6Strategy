//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using LeanCloud.Play;
using Parse;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DpScanMap : MonoBehaviour, IInRoomCallbacks, IOnEventCallback{
    public Text roomId;
    public GameObject roomText;
    public List<GameObject> opButtons;
    public GameObject teamMatePanel;
     public GameObject operatorPanel;
    public GameObject chosenTransform;
    string chosenPlayer;
     public bool followed;
    public string oldFloorName;
    public GameObject mark;
    public Button markBtn;
    public GameObject wipeOut;
     public bool canAddMark;
    public GameObject commonPrefab;
    public List<GameObject> markOnMaps = new List<GameObject>();
    public GameObject infoBtn;
    string lastDate;
    public GameObject chooseBtn;

	// Use this for initialization

    public void OnPlayerEnteredRoom(Player newPlayer){
        updateTeammatePanel();
    }
    public void OnPlayerLeftRoom(Player otherPlayer){
        updateTeammatePanel();
    }


     
    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged){
     
    }

     
    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps){
        updateTeammatePanel();
        Debug.Log(targetPlayer.NickName);
        foreach (var key in changedProps.Keys)
        {
            Debug.Log(key + "  " + changedProps[key]);
        }
        if (changedProps.ContainsKey("follow"))
        {
            if (changedProps["follow"].Equals(ParseUser.CurrentUser["nickName"]) && !targetPlayer.NickName.Equals(ParseUser.CurrentUser["nickName"]))
            {
                followed = true;
            }
            else
            {
                followed = false;
            }
        }



        if (chosenPlayer != null)
        {
             
            if (targetPlayer.NickName.Equals(chosenPlayer) && !targetPlayer.NickName.Equals(ParseUser.CurrentUser["nickName"]))
            {
                Hashtable newprops = changedProps;


                if (newprops.ContainsKey("camera"))
                {
                    Camera.main.transform.position = JsonUtility.FromJson<Vector3>(newprops["camera"] as string);
                }


                if (newprops.ContainsKey("floor"))
                {
                    if (!newprops["floor"].Equals(oldFloorName))
                    {
                        oldFloorName = newprops["floor"] as string;
                        gameObject.GetComponent<MapObj>().createMapWithName(oldFloorName);
                    }
                }
                if (newprops.ContainsKey("rotation"))
                {
                    if (gameObject.GetComponent<MapObj>().wholeMapObj != null)
                    {
                        gameObject.GetComponent<MapObj>().wholeMapObj.transform.rotation = JsonUtility.FromJson<Quaternion>(newprops["rotation"] as string);
 
                    }
                    else
                    {
                        gameObject.GetComponent<MapObj>().displayFloor.transform.rotation = JsonUtility.FromJson<Quaternion>(newprops["rotation"] as string);
 
                    }
                }
                if (newprops.ContainsKey("scale"))
                {
                    if (gameObject.GetComponent<MapObj>().wholeMapObj != null)
                    {
                        gameObject.GetComponent<MapObj>().wholeMapObj.transform.localScale = JsonUtility.FromJson<Vector3>(newprops["scale"] as string);

                    }
                    else
                    {
                        gameObject.GetComponent<MapObj>().displayFloor.transform.localScale = JsonUtility.FromJson<Vector3>(newprops["scale"] as string);

                    }
                }


                if (newprops.ContainsKey("marks"))
                {

                    StepDetailAction action = JsonUtility.FromJson<StepDetailAction>(newprops["marks"] as string);
                    if (action.createTime.CompareTo(lastDate) == 1)
                    {
                        lastDate = action.createTime;
                        createMarkOnMap(action);
                    }



                }


            }


        }



    }

     
    public void OnMasterClientSwitched(Player newMasterClient){

    }

    public void OnEvent(EventData photonEvent){
        // we have defined two event codes, let's determine what to do
        Hashtable data = photonEvent.CustomData as Hashtable;
        switch (photonEvent.Code)
        {
            case 1:
                // do something
                if (data["nickName"].Equals(chosenPlayer))
                {
                    wipeOutMarks();
                }

                break;
            case 2:
                // do something else
                break;
        }
    }


    public void addMarksOnMap(){
        canAddMark = true;
        markBtn.interactable = false;
         
    }

	void Start () {
        if (DataObj.isDp)
        {
            if (DataObj.lbc != null)
            {
                DataObj.lbc.AddCallbackTarget(this);
            }
            lastDate = System.DateTime.Now.ToString();
            infoBtn.SetActive(false);
            mark.SetActive(true);
            wipeOut.SetActive(true);
            chosenTransform.SetActive(false);
            teamMatePanel.SetActive(true);
            roomText.SetActive(true);
            Debug.Log(DataObj.roomNameString);
            roomId.text = "RoomId:\n" + DataObj.roomNameString;
            chooseBtn.SetActive(true);
            updateTeammatePanel();
             

             
        }

    }

    public void wipeOutMarks(){
        if (markOnMaps.Count>0)
        {
            foreach (GameObject cube in markOnMaps)
            {
                Destroy(cube);
            }
            if (followed)
            {
                byte eventCode = 1; // make up event codes at will
                Hashtable evData = new Hashtable();    // put your data into a key-value hashtable
                evData.Add("nickName", ParseUser.CurrentUser["nickName"]);
                DataObj.lbc.OpRaiseEvent(eventCode, evData, RaiseEventOptions.Default,SendOptions.SendReliable);

            }


        }


    }

    private static bool Isgameobject(string s)
    {
        return true;
    }


    void createMarkOnMap(StepDetailAction action){


                GameObject parentObj = GameObject.Find(action.parentName);
                Debug.Log("parentObj = " + parentObj);
                GameObject cube = (GameObject)Instantiate(commonPrefab);
                cube.transform.SetParent(parentObj.transform);
                cube.transform.localScale = action.mainTransform.localscale;
                cube.transform.localPosition = action.mainTransform.localposition;
                cube.transform.localRotation = action.mainTransform.localrotation;


                markOnMaps.Add(cube);
    }

    public void followPlayer(Text playerName){
        chosenPlayer = playerName.text;
        Hashtable h = new Hashtable();
        h.Add("follow", chosenPlayer);
        DataObj.lbc.LocalPlayer.SetCustomProperties(h);

        //var props = new Dictionary<string, object>();
        //props.Add("follow", chosenPlayer);
        //// 请求设置玩家属性

        //play.Player.SetCustomProperties(props);
    }

    public void chooseOperator(){
        operatorPanel.SetActive(true);
    }

    void updateTeammatePanel(){
        //Debug.Log("updateTeamPanel");
        Debug.Log(DataObj.lbc.CurrentRoom.PlayerCount);
        foreach (var key in DataObj.lbc.CurrentRoom.Players.Keys)
        {
            Debug.Log(key + "   " + DataObj.lbc.CurrentRoom.Players[key]);
        }

        for (int i = 0; i < 5; i++)
        {
            if (i< DataObj.lbc.CurrentRoom.PlayerCount)
            {

                opButtons[i].SetActive(true);

                Player player = DataObj.lbc.CurrentRoom.Players[i + 1];
                Button btn = opButtons[i].GetComponent<Button>();
                btn.GetComponentInChildren<Text>().text = player.NickName;
                if (player.CustomProperties.ContainsKey("operator"))
                {
                    Debug.Log(player.CustomProperties["operator"]);
                    btn.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Images/OperatorImage/" + player.CustomProperties["operator"]);
                }
                if (player.NickName.Equals(chosenPlayer))
                {
                    btn.interactable = false;
                    btn.GetComponentInChildren<Text>().text = player.NickName + " follow";
                }else if (player.CustomProperties.ContainsKey("follow"))
                {
                    if (player.CustomProperties["follow"].Equals(ParseUser.CurrentUser["nickName"]))
                    {
                        btn.interactable = false;
                    }
                    else{
                        btn.interactable = true;
                    }

                }
                else{
                    btn.interactable = true;
                }
            }
            else{
                opButtons[i].SetActive(false);
            }
        }
    }

     


     


    // Update is called once per frame
    void Update () {
        if (DataObj.lbc != null)
        {
            DataObj.lbc.Service();
        }
    }
}
