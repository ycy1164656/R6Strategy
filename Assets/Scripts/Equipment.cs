using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.IO;

public class Equipment : MonoBehaviour {
    public Image gunImage;
    public Text nameSizeText;
    public Text desc;
    // Use this for initialization
    public void Setup(ParseObject obj)
    {
        if (File.Exists(DataObj.cachePath + (obj["image"] as string).GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(obj["image"] as string, gunImage));
        }
        else{
            StartCoroutine(DownloadImage(obj["image"] as string, gunImage));
        }
        gunImage.preserveAspect = true;

        nameSizeText.text =  obj["name"] as string ;
        desc.text = obj["desc"] as string;
         

    }


    IEnumerator DownloadImage(string url, Image image)
    {
        WWW www = new WWW(url);
         
        yield return www;

        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();
        File.WriteAllBytes(DataObj.cachePath + url.GetHashCode(), pngData);

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }

    IEnumerator LoadLocalImage(string url,Image image)
    {
        // 已在本地缓存
        string filePath = "file:///" + DataObj.cachePath + url.GetHashCode();
        WWW www = new WWW(filePath);
        yield return www;
        Texture2D tex2d = www.texture;

        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        image.sprite = m_sprite;
    }


 

}
