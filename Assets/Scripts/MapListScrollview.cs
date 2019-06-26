using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Parse;
using Photon.Realtime;

[System.Serializable]

public class ItemMap
{
	public string itemName;
	public Sprite iconImage;

}

public class MapListScrollview : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{

	public GameObject buttonPrefab;
	public List<ItemMap> itemList;
	public Transform contentPanel;
    //private LoadBalancingClient lbc;
    string now;
    public bool isDpPanel;
    public GameObject mapPanel;
    public GameObject dpPanel;
    public bool isVideoPanel;
    public GameObject videoPanel;
    public GameObject videoContentRect;
     // Use this for initialization
    void Start () {
		AddButtonList ();
        now = PlayerPrefs.GetString("language");


    }

	public void GotoMapDetail(MapItem item){

        if (isVideoPanel)
        {
            videoPanel.SetActive(true);
            videoContentRect.GetComponent<VideoScroll>().setupAndGetVideosFromServer(item.originalName);

            return;
        }
        if (isDpPanel)
        {

            var timeNow = System.DateTime.Now;
            string roomName = string.Format("{0}{1}{2}{3}", timeNow.Hour, timeNow.Minute, timeNow.Second, timeNow.Millisecond);
            DataObj.roomNameString = roomName;
            DataObj.MapName = item.originalName;
            DataObj.isBrowseMap = true;
            DataObj.isDp = true;
            DataObj.isJoin = false;
            DataObj.lbc.AddCallbackTarget(this);
            DataObj.lbc.ConnectToRegionMaster(DataObj.regionCode);
            Debug.Log(DataObj.regionCode);

            // 注册连接成功事件

               

        }
        else{
            DataObj.shouHud = true;
            mapPanel.SetActive(false);
            DataObj.MapName = item.originalName;
            DataObj.isBrowseMap = true;
            Camera.main.GetComponent<MainMenu>().gotoMapScene();
         }

       

	}


   



    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        if (DataObj.isJoin)
        {
            EnterRoomParams roomParams = new EnterRoomParams();
            roomParams.RoomName = DataObj.roomNameString;
            DataObj.lbc.LocalPlayer.NickName = ParseUser.CurrentUser["nickName"] as string;
            DataObj.lbc.OpJoinRoom(roomParams);
        }
        else
        {
            DataObj.lbc.LocalPlayer.NickName = ParseUser.CurrentUser["nickName"] as string;
            EnterRoomParams p = new EnterRoomParams();
            p.RoomName = DataObj.roomNameString;
            ExitGames.Client.Photon.Hashtable t = new ExitGames.Client.Photon.Hashtable();
            t.Add("m", DataObj.MapName);
            p.RoomOptions = new RoomOptions() { MaxPlayers = 5, CustomRoomProperties = t };
            DataObj.lbc.OpCreateRoom(p);
            //DataObj.lbc.OpJoinRandomRoom();    // joins any open room (no filter)
        }
       

    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        //Debug.Log("OnRegionListReceived");
        //regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
    }



    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    public void OnJoinedLobby()
    {
    }

    public void OnLeftLobby()
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
        Debug.Log("createRoom");
        dpPanel.SetActive(false);
        DataObj.shouHud = true;
        Camera.main.GetComponent<MainMenu>().gotoMapScene();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (DataObj.isJoin)
        {
            DataObj.MapName = DataObj.lbc.CurrentRoom.CustomProperties["m"] as string;
            DataObj.isBrowseMap = true;
            DataObj.isDp = true;
            DataObj.shouHud = true;
            dpPanel.SetActive(false);
            Camera.main.GetComponent<MainMenu>().gotoMapScene();
        }

    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debug.Log("OnJoinRandomFailed");
        //DataObj.lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnLeftRoom()
    {
    }


    /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
    /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
    private void OnRegionPingCompleted(RegionHandler regionHandler)
    {
        //Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
        //Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
         //this.lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
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
        if (DataObj.lbc != null)
        {
            DataObj.lbc.Service();
        }
       
    }



	private void AddButtonList(){

		for (int i = 0; i < itemList.Count;i++)
		{
			GameObject newobj;
			newobj = (GameObject)Instantiate (buttonPrefab);
			newobj.transform.SetParent (contentPanel);
            newobj.transform.localScale = new Vector3(1f, 1f, 1f);
            newobj.name = "mapItem";
             //newobj.GetComponent<RectTransform>().sizeDelta = new Vector2(400,50);
            //Debug.Log(newobj.GetComponent<RectTransform>().sizeDelta);
            ItemMap item = itemList [i];
			MapItem map = newobj.GetComponent<MapItem>();
			map.Setup (item,this);

		}

	}
}
