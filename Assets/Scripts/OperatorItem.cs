using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.IO;
public class OperatorItem : MonoBehaviour {

    public Button buttonComponent;
    public Image iconImage;
    public Text titleText;
    OperatorList scroll;
    ParseObject operatorObj;
    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(btnClick);
 
    }

    public void Setup(ParseObject item, OperatorList detailList)
    {
        operatorObj = item;
        titleText.text = item["name"] as string;

        if (File.Exists(DataObj.cachePath + (item["logoUrl"] as string).GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(item["logoUrl"] as string, iconImage));
        }
        else
        {
            StartCoroutine(DownloadImage(item["logoUrl"] as string, iconImage));
        }
        iconImage.preserveAspect = true;

         scroll = detailList;
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




    void btnClick()
    {
        scroll.gotoOperatorDetail(operatorObj);
    }
}
