using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DataInsert : Singleton<DataInsert>
{
    public string inputID;
    public string inputEmail;
    public string inputName;
    public GameObject m_createAccountPopup;
    public GameObject m_alreadyaccountPopup;
    public Button m_mainPanel;
    public Text m_alreadyaccountPopupText;
    public Text m_userName;
    public Text m_userMail;
    public Text m_userID;
    public Text m_loginText;
    public Text m_text;

    public Text m_isdone;

    public bool m_accountOkCheck = false;
    public string login = "Login";
    public string logout = "Logout";
    public string loginCheck = "";
    public string m_createIdUrl = "http://54.238.128.34/InsertUser.php";
    public string m_checkIdUrl = "http://54.238.128.34/useridcheck.php";
    public string m_getid;

    void Awake()
    {
        m_mainPanel.interactable = false;
        m_alreadyaccountPopup.SetActive(false);
        m_createAccountPopup.SetActive(false);
    }
    // Use this for initialization
    void Start ()
    {
        //if (GPGSMgr.GetInstance.m_bLogin)
        //{
        //    CreateUser(GPGSMgr.GetInstance.GetIDGPGS(), GPGSMgr.GetInstance.GetMailGPGS(), PlayerPrefs.GetString("UserName"));
        //}
    }
    void Update()
    {
        if (GPGSMgr.GetInstance.m_bLogin )
        {
            inputID = GPGSMgr.GetInstance.GetIDGPGS();
            inputEmail = GPGSMgr.GetInstance.GetMailGPGS();
            
            m_text.text = string.Format("ID : {0} \n Mail : {1} \n Name : {2}", inputID, inputEmail, inputName);
        }

        if (GPGSMgr.GetInstance.m_bLogin == false)
        {
            m_loginText.text = string.Format("{0}", login);
            loginCheck = logout;
        }
        else
        {
            m_loginText.text = string.Format("{0}", logout);
            loginCheck = login;

            m_userMail.text = string.Format("{0}", inputEmail);
            m_userName.text = string.Format("{0}", inputName);
            m_userID.text = string.Format("{0}", inputID);
        }

        if(AssetLoader.GetInstance.m_downCheck && loginCheck == login && m_accountOkCheck)
        {
            m_mainPanel.interactable = true;
        }
        else
        {
            m_mainPanel.interactable = false;
        }

        if (m_accountOkCheck)
        {
            StartCoroutine(CreateAccountCheck());
        }
    }
    public void InputName(string _name)
    {
        inputName = _name;
        if (inputName == "")
        {
            inputName = GPGSMgr.GetInstance.GetNameGPGS();
            PlayerPrefs.SetString("UserName", _name);
        }
        else if(inputName != "")
        {
            inputName = _name;
            PlayerPrefs.SetString("UserName", _name);
        }
        
    }

    public void CreateUser(string _googleid, string _email, string _username)
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", _googleid);
        form.AddField("emailPost", _email);
        form.AddField("usernamePost", _username);

        WWW www = new WWW(m_createIdUrl, form);
    }

    //계정 생성 창 띄움
    public void CreateAccountPopup()
    {
        m_createAccountPopup.SetActive(true);
        GPGSMgr.GetInstance.m_bLogin = true;

        if(GPGSMgr.GetInstance.m_bLogin)
        {
            inputID = GPGSMgr.GetInstance.GetIDGPGS();
            inputEmail = GPGSMgr.GetInstance.GetMailGPGS();
        }
    }

    //계정 중복 체크, 중복이면 중복알림창 띄우고 아니면 생성.
    public IEnumerator CreateAccountCheck()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", inputID);

        WWW www = new WWW(m_checkIdUrl, form);
        
        yield return www;
        
        if(www.error != null)
        {
            Debug.Log("www error :" + www.error);
            yield break;
        }
        if(www.isDone)
        {
            m_getid = www.text;

            if(m_getid == "true")
            {
                CreateUser(inputID, inputEmail, inputName);
                LoadData.GetInstance.m_playername = inputName;
                m_isdone.text = string.Format("{0}", m_getid);
            }
            else if(m_getid == "false")
            {
                // 계정 중복되면 중복메시지 창 띄움 
                // 추후 스케일 변화 애니메이션 추가 작 -> 큰
                m_alreadyaccountPopup.SetActive(true);
                m_alreadyaccountPopupText.text = string.Format("already have an account");
                m_isdone.text = string.Format("{0}", m_getid);
            }
            
        }
    }

    public void CreateAccountCancelButton()
    {
        GPGSMgr.GetInstance.m_bLogin = false;
        m_createAccountPopup.SetActive(false);
    }
    
    public void CreateAccountOkButton()
    {
        m_accountOkCheck = true;
       
        if (m_getid == "true")
        {
            m_createAccountPopup.SetActive(false);
            
        }        
    }

    public void AlreadyAcountPopup()
    {
        // 계정 중복되면 중복메시지 창 닫음
        // 추후 스케일 변화 애니메이션 추가 큰 -> 작
        m_accountOkCheck = false;
        m_alreadyaccountPopup.SetActive(false);
        
    }
       // already have an account
}
