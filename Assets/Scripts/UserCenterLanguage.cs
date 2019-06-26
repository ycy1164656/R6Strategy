using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCenterLanguage : MonoBehaviour {

    public Text userNamePlaceholder;
    public Text passwordPlaceholder;
    public Text emailPlaceholder;
    public Text loginLogo;
    public Text registerLogo;
     public Text createAccount;
    public Text forgetAccount;
    public Text login;
    public Text registerUserName;
    public Text registerPassword;
    public Text registerBtn;
    public Text forgetlogo;
    public Text forgetemail;
    public Text sendBtn;
    public Text nickName;
    public Text star;
    public Text mine;
    public Text logout;
    // Use this for initialization
    void Start () {
		
        if (PlayerPrefs.GetString("language").Equals("ch"))
        {
            userNamePlaceholder.text = "用户名";
            passwordPlaceholder.text = "密码";
            emailPlaceholder.text = "电子邮件";
            loginLogo.text = "登录";
            login.text = "登录";
            registerLogo.text = "注册";
            createAccount.text = "注册账号";
            forgetAccount.text = "忘记密码?";
            registerUserName.text = "用户名";
            registerBtn.text = "现在注册";
            registerPassword.text = "密码";
            forgetlogo.text = "忘记密码";
            forgetemail.text = "电子邮箱";
            sendBtn.text = "发送";
            //nickName.text = "昵称";
            star.text = "点赞";
            mine.text = "我的";
            logout.text = "登出";
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
