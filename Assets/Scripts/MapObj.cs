using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 


[System.Serializable]
public class RoomPiece
{
    public string roomname;
    public Vector3 localPosition;
    public bool isH;
    public bool isS;
    public bool isB;

}


[System.Serializable]
public class MapPiece
{
	public GameObject mapSpilt;
    public string mapSpiltName;
    public List<RoomPiece> rooms;
    public List<Vector3> cameras;
    public float roomLocalScale = 0.005f;
    public float yHeight;
}

[System.Serializable]
public class MapDetail
{
    public string mapName;
    public List<MapPiece> allMapPieces;
    public GameObject wholeMap;


}

public class MapObj : MonoBehaviour {

	public List<MapDetail> mapList;
    public Text CurrentFloorLabel;
    private MapDetail displayMap;
    bool isWhole = true;
    public int currentFloor = 0;
    public string currentFloorName;
    public GameObject displayFloor;
    public GameObject wholeMapObj;
    public Button upBtn;
    public Button downBtn;
    public GameObject textPrefab;
    public GameObject cameraPrefab;
    List<GameObject> roomNameObjs = new List<GameObject> ();
    Quaternion oldRotation;
    public Text mapTypeText;
    //bool upOrDown = true;
    GameObject nowMap;
    MapPiece nowPiece;


	// Use this for initialization
	void Start () {

		Debug.Log (DataObj.MapName);
        downBtn.interactable = false;

        List<string> allRoomnames = new List<string>();

        foreach (MapDetail itemMap in mapList)
        {
            foreach (MapPiece mapPiece in itemMap.allMapPieces)
            {
                foreach (RoomPiece ro in mapPiece.rooms)
                {
                    if (!allRoomnames.Contains(ro.roomname))
                    {
                        Debug.Log(ro.roomname + "+" + mapPiece.mapSpiltName);
                        allRoomnames.Add(ro.roomname);
                    }
                }
            }

            if (itemMap.mapName.Equals(DataObj.MapName))
            {
                displayMap = itemMap;
                CreateMap();
                break;
            }
        }

        foreach (string item in allRoomnames)
        {
            Debug.Log(item);
        }



    }

    


    public void upBtnClickedForMap(){
        if (isWhole)
        {
            isWhole = false;
        }else{
            downBtn.interactable = true;
        }


        if (wholeMapObj != null)
        {
            currentFloor = -1;
            Destroy(wholeMapObj);
        }
        Destroy(displayFloor);
        //if (!upOrDown)
        //{
        //    currentFloor++;
        //    upOrDown = true;
        //}

        if (currentFloor <displayMap.allMapPieces.Count)
        {
            currentFloor++;
            displayFloor = (GameObject)Instantiate(displayMap.allMapPieces[currentFloor].mapSpilt);
            //currentFloorName = displayMap.allMapPieces[currentFloor].mapSpiltName;
            StartCoroutine(changeName(displayMap.allMapPieces[currentFloor].mapSpiltName));
             displayFloor.GetComponent<ControlObject>().controllerObj = this.gameObject;
            gameObject.GetComponent<CommonControl>().displayMap = displayFloor;
            createRoomNameOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
            createCameraOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
            CurrentFloorLabel.text = "Floor:" + currentFloor;
             if (currentFloor + 1 == displayMap.allMapPieces.Count)
            {
                upBtn.interactable = false;

            }
         }

    }

    public void createMapWithName(string mapSpiltName){

        if (mapSpiltName.Equals(displayMap.mapName))
        {
            wholeBtnClickedForMap();
            return;
        }

        if (wholeMapObj != null)
        {
            isWhole = false;
            Destroy(wholeMapObj);
        }
        Destroy(displayFloor);

        for (int i = 0; i < displayMap.allMapPieces.Count; i++)
        {
            MapPiece piece = displayMap.allMapPieces[i];
            if (piece.mapSpiltName.Equals(mapSpiltName))
            {
                displayFloor = (GameObject)Instantiate(displayMap.allMapPieces[i].mapSpilt);
                StartCoroutine(changeName(displayMap.allMapPieces[i].mapSpiltName));
                displayFloor.GetComponent<ControlObject>().controllerObj = this.gameObject;
                gameObject.GetComponent<CommonControl>().displayMap = displayFloor;
                currentFloor = i;
                CurrentFloorLabel.text = "Floor:" + currentFloor;
                createRoomNameOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
                createCameraOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
                if (i == 0)
                {
                    upBtn.interactable = true;
                    downBtn.interactable = false;
                }
                else if (i == displayMap.allMapPieces.Count - 1)
                {
                    upBtn.interactable = false;
                    downBtn.interactable = true;
                }
                else
                {
                    upBtn.interactable = true;
                    downBtn.interactable = true;

                }
                //currentFloorName = displayMap.allMapPieces[i].mapSpiltName;
               
            }
        }


    }

    void followeeMap(string mapSpilt){
        if (gameObject.GetComponent<DpScanMap>().followed)
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("floor", mapSpilt);
            DataObj.lbc.LocalPlayer.SetCustomProperties(h);
             
        }

    }

    public void downBtnClickedForMap(){
        isWhole = false;
        upBtn.interactable = true;
        if (wholeMapObj != null)
        {
            Destroy(wholeMapObj);
        }

        Destroy(displayFloor);

        //if (upOrDown)
        //{
        //    currentFloor--;
        //    upOrDown = false;
        //}

        if (currentFloor > 0)
        {
            currentFloor--;
            displayFloor = (GameObject)Instantiate(displayMap.allMapPieces[currentFloor].mapSpilt);
            //currentFloorName = displayMap.allMapPieces[currentFloor].mapSpiltName;
            StartCoroutine(changeName(displayMap.allMapPieces[currentFloor].mapSpiltName));
            displayFloor.GetComponent<ControlObject>().controllerObj = this.gameObject;
            gameObject.GetComponent<CommonControl>().displayMap = displayFloor;
            createRoomNameOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
            createCameraOnMap(displayFloor, displayMap.allMapPieces[currentFloor]);
            CurrentFloorLabel.text = "Floor:" + currentFloor;

            if (currentFloor == 0)
            {
                downBtn.interactable = false;

            }

         }


    }

    IEnumerator changeName(string floorname){
        yield return new WaitForSeconds(0.2f);
        followeeMap(floorname);
        currentFloorName = floorname;
        gameObject.GetComponent<DpScanMap>().oldFloorName = currentFloorName;
    }

    public void wholeBtnClickedForMap()
    {
        if (wholeMapObj != null)
        {
            return;
        }

        Destroy(displayFloor);
        downBtn.interactable = false;
        upBtn.interactable = true;
        isWhole = true;
        //upOrDown = true;
        wholeMapObj = (GameObject)Instantiate(displayMap.wholeMap);
        wholeMapObj.GetComponent<ControlObject>().controllerObj = this.gameObject;
        gameObject.GetComponent<CommonControl>().displayMap = wholeMapObj;
        CurrentFloorLabel.text = "WholeMap";
        currentFloorName = displayMap.mapName;
        followeeMap(currentFloorName);
        gameObject.GetComponent<DpScanMap>().oldFloorName = currentFloorName;
        currentFloor = 0;
    }

    public void CreateMap(){

        wholeMapObj = (GameObject)Instantiate(displayMap.wholeMap);
        wholeMapObj.GetComponent<ControlObject>().controllerObj = this.gameObject;
        gameObject.GetComponent<CommonControl>().displayMap = wholeMapObj;
        CurrentFloorLabel.text = "WholeMap";
        currentFloorName = displayMap.mapName;
        currentFloor = 0;
     }

    public void BackToPresentScene()
	{
        if (DataObj.isDp)
        {
            DataObj.isJoin = false;
            DataObj.lbc.OpLeaveRoom(false);
            DataObj.lbc.Disconnect();
        }
        DataObj.isScan = false;
        DataObj.isBrowseMap = false;
        DataObj.isDp = false;
        DataObj.shouHud = false;
        DataObj.isRecommend = false;


        SceneManager.LoadScene (0);

	}

    public void changeMapTypeText()
    {
        if (mapTypeText.text.Equals("Hostage"))
        {
            mapTypeText.text = "Secure";
        }
        else if (mapTypeText.text.Equals("Secure"))
        {
            mapTypeText.text = "Bomb";
        }
        else
        {
            mapTypeText.text = "Hostage";
        }
        createRoomNameOnMap(nowMap, nowPiece);
    }

    void createRoomNameOnMap(GameObject thismap,MapPiece mapPiece){

        if (thismap == null)
        {
            return;
        }

        nowMap = thismap;
        nowPiece = mapPiece;
        if (roomNameObjs.Count > 0)
        {
            foreach (GameObject item in roomNameObjs)
            {
                Destroy(item);
            }

        }

        RoomData roomData = gameObject.GetComponent<RoomData>();
        for (int i = 0; i < mapPiece.rooms.Count; i++)
        {
            RoomPiece roomPiece = mapPiece.rooms[i];
            GameObject threedtext = Instantiate(textPrefab);
            threedtext.transform.SetParent(thismap.transform);
            threedtext.transform.localPosition = new Vector3(roomPiece.localPosition.x,0, roomPiece.localPosition.z);
            threedtext.transform.localScale = new Vector3(mapPiece.roomLocalScale, mapPiece.roomLocalScale, mapPiece.roomLocalScale);
            threedtext.transform.rotation = new Quaternion(0.7071f, 0f, 0f,0.7071f);
            threedtext.GetComponent<TextMesh>().text = roomData.TranslateWithString(roomPiece.roomname);
            threedtext.GetComponent<TextMesh>().color = Color.black;
            roomNameObjs.Add(threedtext);

            if (mapTypeText.text.Equals("Hostage"))
            {
                if (roomPiece.isH)
                {
                    GameObject symbols = Instantiate(textPrefab);
                    symbols.transform.SetParent(thismap.transform);
                    symbols.transform.localPosition = new Vector3(roomPiece.localPosition.x, mapPiece.yHeight * 1.5f, roomPiece.localPosition.z);
                    symbols.transform.localScale = new Vector3(mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3);
                    symbols.GetComponent<TextMesh>().text = "H";
                    symbols.GetComponent<TextMesh>().color = Color.red;

                    roomNameObjs.Add(symbols);
                }
            }
            else if (mapTypeText.text.Equals("Secure"))
            {
                if (roomPiece.isS)
                {
                    GameObject symbols = Instantiate(textPrefab);
                    symbols.transform.SetParent(thismap.transform);
                    symbols.transform.localPosition = new Vector3(roomPiece.localPosition.x, mapPiece.yHeight * 1.5f, roomPiece.localPosition.z);
                    symbols.transform.localScale = new Vector3(mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3);
                    symbols.GetComponent<TextMesh>().text = "S";
                    symbols.GetComponent<TextMesh>().color = Color.green;

                    roomNameObjs.Add(symbols);
                }
            }
            else
            {
                if (roomPiece.isB)
                {
                    GameObject symbols = Instantiate(textPrefab);
                    symbols.transform.SetParent(thismap.transform);
                    symbols.transform.localPosition = new Vector3(roomPiece.localPosition.x, mapPiece.yHeight * 1.5f, roomPiece.localPosition.z);
                    symbols.transform.localScale = new Vector3(mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3, mapPiece.roomLocalScale * 3);
                    symbols.GetComponent<TextMesh>().text = "B";
                    symbols.GetComponent<TextMesh>().color = Color.blue;

                    roomNameObjs.Add(symbols);
                }
            }


        }



    }
    void createCameraOnMap(GameObject thismap, MapPiece mapPiece)     {



        for (int i = 0; i < mapPiece.cameras.Count; i++)         {             Vector3 roomPiece = mapPiece.cameras[i];             GameObject threedtext = Instantiate(cameraPrefab);             threedtext.transform.SetParent(thismap.transform);             threedtext.transform.localPosition = new Vector3(roomPiece.x, roomPiece.y, roomPiece.z);
        }        } 

    private static bool Isgameobject(GameObject s)
    {
         return true;
    }


    private void Update()
    {
        if (nowMap != null && nowPiece != null)
        {
            createRoomNameOnMap(nowMap, nowPiece);

        }
        //if (displayFloor != null)
        //{
        //    if (oldRotation != displayFloor.transform.rotation)
        //    {

        //        oldRotation = displayFloor.transform.rotation;
        //        if (roomNameObjs.Count > 0)
        //        {

        //            foreach (GameObject item in roomNameObjs)
        //            {
        //                item.transform.SetPositionAndRotation(new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z), new Quaternion(0, oldRotation.y,0,0));
        //                //item.transform.Rotate(new Vector3(0, - oldRotation.y, 0));


        //            }
        //        }
        //    }
        //}


    }

    // Update is called once per frame

}
