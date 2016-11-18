using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPGSBtn : MonoBehaviour {

    public Text m_loginText;
    //public RawImage m_userImage = null;
    public Text m_userName;
    public Text m_userMail;
    public Text m_userID;

    public string login = "Login";
    public string logout = "Logout";

	// Use this for initialization
	void Start ()
    {
        //GPGSMgr.GetInstance.InitializeGPGS();
	}
	
	// Update is called once per frame
	void Update ()
    {
        SettingUser();
	    if(GPGSMgr.GetInstance.m_bLogin == false)
        {
            m_loginText.text = string.Format("{0}", login);            
        }
        else
        {
            m_loginText.text = string.Format("{0}", logout);
           
        }
        //Achievement();
	}

    public void ClickEvent()
    {
        if(!GPGSMgr.GetInstance.m_bLogin)
        {
            GPGSMgr.GetInstance.LoginGPGS();
        }
        else
        {            
            GPGSMgr.GetInstance.LogoutGPGS();
        }        
    }

    void SettingUser()
    {
        //if (m_userImage.mainTexture == null)
        //{
        //    return;
        //}
        //m_userImage.texture = GPGSMgr.GetInstance.GetImageGPGS();
        m_userMail.text = string.Format("Mail : {0}", GPGSMgr.GetInstance.GetMailGPGS());
        m_userName.text = string.Format("Name: {0}", GPGSMgr.GetInstance.GetNameGPGS());
        m_userID.text = string.Format("ID: {0}", GPGSMgr.GetInstance.GetIDGPGS());
    }

    void Achievement()
    {
        if(Test.m_count == 5)
        {
            Social.ReportProgress(TestAchievement.achievement_test1, 100.0f, (bool success) =>
            {
            // handle success or failure
            });
        }
        else if(Test.m_count == 10 )
        {
            Social.ReportProgress(TestAchievement.achievement_test2, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (Test.m_count == 15)
        {
            Social.ReportProgress(TestAchievement.achievement_test3, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (Test.m_count == 20)
        {
            Social.ReportProgress(TestAchievement.achievement_test4, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (Test.m_count == 25)
        {
            Social.ReportProgress(TestAchievement.achievement_test5, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }   
    }

    public void DisplayAchievement()
    {
        Social.ShowAchievementsUI();
    }

    public void SaveGame()
    {
        GPGSMgr.GetInstance.SaveGame();
    }
    public void LoadGame()
    {
        GPGSMgr.GetInstance.LoadGame();
    }

}
