using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
 using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinRoomNow : MonoBehaviour {

    public InputField roomName;
    public GameObject contentPanel;
	// Use this for initialization
	void Start () {
		
	}

    public void JoinNow(){
        if (roomName.text.Length > 0)
        {
            DataObj.isJoin = true;
            DataObj.roomNameString = roomName.text;
            DataObj.lbc.AddCallbackTarget(contentPanel.GetComponent<MapListScrollview>());
            DataObj.lbc.ConnectToRegionMaster(DataObj.regionCode);
             

             
        }
        else{
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();
        }
    }
	
    public void leaveRoom(){
        //Play play = Play.Instance;
        ////play.LeaveRoom();
        //play.Disconnect();
        DataObj.isJoin = false;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
