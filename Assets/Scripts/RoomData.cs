using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class RoomWord
{
    public string roomname;
    public string ch;

}


public class RoomData : MonoBehaviour
{
    public List<RoomWord> roomWords;
    public string TranslateWithString(string oname){

        string newString = oname;
        string now = PlayerPrefs.GetString("language");

        foreach (RoomWord word  in roomWords)
        {

            if (word.roomname.Equals(oname))
            {
                if (now.Equals("ch"))
                {
                    newString = word.ch;
                }
            }

        }


        return newString;
    }

    

	 
}


