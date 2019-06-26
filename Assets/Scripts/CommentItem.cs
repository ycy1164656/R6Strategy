using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentItem : MonoBehaviour {
    public Text username;
    public Text createTime;
    public Text contentText;

    CommentScrollList scrollList;

	// Use this for initialization
	 
    public void createRow(string nickname,string createDate,string content){
        username.text = nickname;
        createTime.text = createDate;
        contentText.text = content;


    }
	
	 
}
