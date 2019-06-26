using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Parse;

public class VideoBtn : MonoBehaviour {

    public Text titleText;
    public Text typeText;
    public Image previewImage;
    public Button button;
    string videoUrl;
    VideoScroll scorllList;
	// Use this for initialization
	void Start () {
        button.onClick.AddListener(buttonClicked);
	}

    public void Setup(Dictionary<string,object> video ,VideoScroll scroll){

         foreach (var key in video.Keys)
        {
            Debug.Log(key + "  :   " + video[key]);
        }
        scorllList = scroll;
        titleText.text = video["title"] as string;
        videoUrl = video["url"] as string;
        if (videoUrl.Contains("bilibili"))
        {
            typeText.text = "Bilibili";
        }else{
            typeText.text = "Youtube";
        }

        if (File.Exists(DataObj.cachePath + video["imageUrl"].GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(video["imageUrl"] as string, previewImage));
        }
        else
        {
            StartCoroutine(DownloadImage(video["imageUrl"] as string, previewImage));
        }

        previewImage.preserveAspect = true;


    }


    public void Setup(ParseObject video, VideoScroll scroll)
    {

        foreach (var key in video.Keys)
        {
            Debug.Log(key + "  :   " + video[key]);
        }
        scorllList = scroll;
        titleText.text = video["title"] as string;
        videoUrl = video["url"] as string;
        if (videoUrl.Contains("bilibili"))
        {
            typeText.text = "Bilibili";
        }
        else
        {
            typeText.text = "Youtube";
        }

        if (File.Exists(DataObj.cachePath + video["imageUrl"].GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(video["imageUrl"] as string, previewImage));
        }
        else
        {
            StartCoroutine(DownloadImage(video["imageUrl"] as string, previewImage));
        }

        previewImage.preserveAspect = true;


    }




    void buttonClicked(){
        scorllList.gotoVideoPanel(videoUrl);
    }

    IEnumerator DownloadImage(string url, Image image)
    {
        WWW www = new WWW(url);

        yield return www;

        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();

        //if (url.Contains("jpg"))
        //{
        //    pngData = tex2d.EncodeToJPG();
        //}


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



    // Update is called once per frame
    void Update () {
		
	}
}
