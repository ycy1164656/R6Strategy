using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class UserControl : MonoBehaviour {

    public InputField loginUsername;
    public InputField loginPassword;
    public InputField createUsername;
    public InputField createPassword;
    public InputField createEmail;
    public InputField forgetEmail;
    public InputField changeNickName;
    public GameObject loginPanel;
    public GameObject userPanel;
    public GameObject createPanel;
    public Text nickNameText;
    public GameObject userStrategyList;
    public GameObject hud;
    public GameObject mineBtn;
	// Use this for initialization
	void Start () {
         if (ParseUser.CurrentUser != null)
        {
            userPanel.SetActive(true);
            loginPanel.SetActive(false);
            nickNameText.text = "Nickname:" + ParseUser.CurrentUser["nickName"];
            changeNickName.text = (string )ParseUser.CurrentUser["nickName"];
            userStrategyList.GetComponent<StrategyDetailList>().GetStrategiesFromServerWithUser();
        }
        else
        {
            userPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
        if (DataObj.isFree)
        {
            mineBtn.SetActive(false);
        }
    }

     

    public void loginClicked(){
        if (loginUsername.text.Length == 0 || loginPassword.text.Length == 0)
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();

        }
        else
        {
            //MNP.ShowPreloader("", "Login...");
            ParseUser.LogInAsync(loginUsername.text, loginPassword.text).ContinueWith(t =>
            {
                MNP.HidePreloader();
                if (t.IsFaulted || t.IsCanceled)
                {
                    // The login failed. Check the error to see why.
                    using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ParseException error = (ParseException)enumerator.Current;
                            // error.Message will contain an error message
                            // error.Code will return "OtherCause"
                            UnityMainThreadDispatcher.Instance().Enqueue(showErrorWithMessage(error.Message));
                            
                        }
                    }
                }
                else
                {

                //shouldLoginIn = true;
                UnityMainThreadDispatcher.Instance().Enqueue(loginSucceed());
                    // Login was successful.
                }
            });
        }
    }

    IEnumerator loginSucceed()
    {
        loginPanel.SetActive(false);
        userPanel.SetActive(true);
        nickNameText.text = "Nickname:" + ParseUser.CurrentUser["nickName"];
        yield return null;
    }

    IEnumerator showErrorWithMessage(string message)
    {
        MNPopup mNPopup = new MNPopup("Error", message);
        mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
        mNPopup.Show();
        yield return null;
    }

    IEnumerator showSuccessWithMessage(string message)
    {
        MNPopup mNPopup = new MNPopup("Succeed!", message);
        mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
        mNPopup.Show();
        yield return null;
    }

    public void createUserClicked(){

        if (createEmail.text.Length == 0 || createPassword.text.Length == 0||createUsername.text.Length == 0)
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();

        }
        else
        {
            var user = new ParseUser();
            user.Username = createUsername.text;
            user.Password = createPassword.text;
            user.Email = createEmail.text;
             

            // other fields can be set just like with ParseObject
            user["nickName"] = createUsername.text;
            //MNP.ShowPreloader("", "Creating...");
            user.SignUpAsync().ContinueWith(t => {
                MNP.HidePreloader();
                if (t.IsFaulted)
                {
                    // Errors from Parse Cloud and network interactions
                    using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ParseException error = (ParseException)enumerator.Current;
                            // error.Message will contain an error message
                            // error.Code will return "OtherCause"
                            UnityMainThreadDispatcher.Instance().Enqueue(showErrorWithMessage(error.Message));
                        }
                    }
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(createSuccess());
                }
            });

        }

       

    }

    IEnumerator createSuccess()
    {
        MNPopup mNPopup = new MNPopup("Succeed!", "Create Successful!");
        mNPopup.AddAction("Ok", () => {
            createSuccessConfirm();

        });
        mNPopup.Show();
        yield return null;
    }

    void createSuccessConfirm()
    {
        userPanel.SetActive(true);
        nickNameText.text = "Nickname:" + ParseUser.CurrentUser["nickName"];
        changeNickName.text = (string)ParseUser.CurrentUser["nickName"];
        createPanel.SetActive(false);
    }

    public void changeNickNameConfirm(){
        if (changeNickName.text.Length == 0)
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();

        }
        else
        {
            ParseUser.CurrentUser["nickName"] = changeNickName.text;
            //MNP.ShowPreloader("", "");
            ParseUser.CurrentUser.SaveAsync().ContinueWith(t => {
                MNP.HidePreloader();
                if (t.IsFaulted)
                {
                    // Errors from Parse Cloud and network interactions
                    using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ParseException error = (ParseException)enumerator.Current;
                            // error.Message will contain an error message
                            // error.Code will return "OtherCause"
                            UnityMainThreadDispatcher.Instance().Enqueue(showErrorWithMessage(error.Message));
                        }
                    }
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(changedNickname());
                }
            });
        }
    }

    

    public void forgetSendEmail()
    {
        if (forgetEmail.text.Length == 0)
        {
            MNPopup mNPopup = new MNPopup("Error", "Please Input Correct!");
            mNPopup.AddAction("Ok", () => { Debug.Log("Ok action callback"); });
            mNPopup.Show();

        }
        else
        {
             //MNP.ShowPreloader("", "");
            ParseUser.RequestPasswordResetAsync(forgetEmail.text).ContinueWith(t => {
                MNP.HidePreloader();
                if (t.IsFaulted)
                {
                    // Errors from Parse Cloud and network interactions
                    using (IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ParseException error = (ParseException)enumerator.Current;
                            // error.Message will contain an error message
                            // error.Code will return "OtherCause"
                            UnityMainThreadDispatcher.Instance().Enqueue(showErrorWithMessage(error.Message));
                        }
                    }
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(showSuccessWithMessage("Send Email Successful!"));
                   
                }
            });
        }
    }
    public void LoginViewBack()
    {
        SceneManager.LoadScene("UIScene");
    }
    public void QuitLogin()
    {
        SceneManager.LoadScene("UIScene");
        ParseUser.LogOutAsync();
        
    }

    public void gotoMap(){

        if (DataObj.isFree)
        {
            if (DataObj.openTimes % 2 == 0)
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (Advertisement.IsReady())
                    {
                        Advertisement.Show("video");
                    }

                }
            }
           

        }
        DataObj.openTimes++;
        hud.SetActive(true);
        StartCoroutine(gotoMapSceneLater());
    }

    IEnumerator gotoMapSceneLater()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator changedNickname()
    {
        nickNameText.text = "Nickname:" + ParseUser.CurrentUser["nickName"];
        yield return null;
    }

    

}
