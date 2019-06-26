using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using System.Threading.Tasks;
using System;
 using Photon.Realtime;

public class DataObj : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{

	public static DataObj instance;
	public static string MapName;
	public static int type;
    public static ParseObject strategy;
    public static bool isScan;
    public static bool isBrowseMap;
    public static bool isUserOwn;
     public static bool isDp;
    public static string roomNameString;
    public string AppId; // set in inspector
    public static LoadBalancingClient lbc;
    public static string regionCode;
    public static bool isJoin;
    public static bool isRecommend;
    private ConnectionHandler ch;
    public static bool shouHud;
    public static bool isFree;
    public static string cachePath;
    public static int openTimes;
     private void Awake()
	{
		if (instance != null) {
			Destroy (gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (instance);
	}


    private void Start()
    {
        DataObj.lbc = new LoadBalancingClient();
        DataObj.lbc.AppId = this.AppId;
        DataObj.lbc.AddCallbackTarget(this);
        DataObj.lbc.ConnectToNameServer();

        this.ch = this.gameObject.GetComponent<ConnectionHandler>();
        if (this.ch != null)
        {
            this.ch.Client = DataObj.lbc;
            this.ch.StartFallbackSendAckThread();
        }


    }

    private void Update()
    {
        LoadBalancingClient client = DataObj.lbc;
        if (client != null)
        {
            client.Service();

        }

        

    }






    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster");
        //DataObj.lbc.OpJoinRandomRoom();    // joins any open room (no filter)
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
        Debug.Log("OnRegionListReceived");
        regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
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
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinedRoom()
    {
        //Debug.Log("OnJoinedRoom");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
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
        Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
        Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
        DataObj.regionCode = regionHandler.BestRegion.Code;
        //DataObj.lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
    }





    IEnumerator changeName()
    {
        yield return new WaitForSeconds(3.0f);
        testSave();
    }

    void testSave()
    {
        //ParseObject gameScore = new ParseObject("TestClass");
        //gameScore["testString"] = "Sean Plott";
        //gameScore.SaveAsync().ContinueWith(t =>{
        //    AggregateException ex = t.Exception as AggregateException;
        //    if (ex != null)
        //    {
        //        ParseException inner = ex.InnerExceptions[0] as ParseException;
        //        Debug.Log(inner.Code + "\\\\\\" + inner.Message);
        //    }
        //});
 
        var query = ParseObject.GetQuery("Strategy")
                               .WhereEqualTo("mapName", "House");
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (ParseObject obj  in results)
            {
                Debug.Log(obj["strategyString"]);
                DataObj.strategy =  obj;
                DataObj.MapName = (string)obj["mapName"];
                DataObj.isScan = true;
            }
            //Debug.Log(results);
            AggregateException ex = t.Exception as AggregateException;
            if (ex != null)
            {
                ParseException inner = ex.InnerExceptions[0] as ParseException;
                Debug.Log(inner.Code + "///////" + inner.Message);
            }
        });
    }

   

}
