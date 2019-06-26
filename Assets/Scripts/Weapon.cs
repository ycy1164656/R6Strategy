using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Parse;
using System.IO;
public class Weapon : MonoBehaviour {

    public Image gunImage;
    public Text nameText;
    public Text damage;
    public Text rate;
    public Text mobility;
    public Text size;
    public Text total;
    public Text desc;
    public Text sight;
    public Text barrel;
    public Text grip;
    public Text under;
    public Text ClassType;
    // Use this for initialization
    void Start () {
		
	}
	
    public void Setup(ParseObject obj){
        if (File.Exists(DataObj.cachePath + (obj["image"] as string).GetHashCode()))
        {
            StartCoroutine(LoadLocalImage(obj["image"] as string, gunImage));
        }
        else
        {
            StartCoroutine(DownloadImage(obj["image"] as string, gunImage));
        }
        gunImage.preserveAspect = true;
        nameText.text = nameText.text + obj["name"] as string;
        damage.text = damage.text + obj["damage"] as string;
        rate.text = rate.text + obj["rate"] as string;
        mobility.text = mobility.text +  obj["firemode"] as string;
        size.text = size.text + obj["size"] as string;
        total.text = total.text + obj["total"] as string;
        desc.text =  obj["desc"] as string;
        ClassType.text = ClassType.text + obj["typeName"] as string;

         List<string> sights =
            ((List<object>)obj["sight"]).OfType<string>().ToList();
        for (int i = 0; i <sights.Count  ; i++)
        {
            sight.text = sight.text + "\n" + sights[i];
        }
        List<string> grips =
            ((List<object>)obj["grip"]).OfType<string>().ToList();
        for (int i = 0; i < grips.Count; i++)
        {
            grip.text = grip.text + "\n" + grips[i];
        }
         List<string> unders =
            ((List<object>)obj["underBarrel"]).OfType<string>().ToList();
        for (int i = 0; i < unders.Count; i++)
        {
            under.text = under.text + "\n" + unders[i];
        }
         
        List<string> barrels =
           ((List<object>)obj["barrel"]).OfType<string>().ToList();
        for (int i = 0; i < barrels.Count; i++)
        {
            barrel.text = barrel.text + "\n" + barrels[i];
        }


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

    // Update is called once per frame
    void Update () {
		
	}
}
