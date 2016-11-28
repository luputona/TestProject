using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;


public class DataInsert : Singleton<DataInsert>
{
    public string inputID;
    public string inputEmail;
    public string inputName;
    public string m_tempinven = "";

    public GameObject m_createAccountPopup;
    public GameObject m_alreadyaccountPopup;
    public JsonData m_initCharJosn;
    
    public Button m_mainPanel;
    
    public Text m_userName;
    public Text m_userMail;
    public Text m_userID;
    public Text m_loginText;
    public Text m_text;
    public Text m_alreadyaccountPopupText;
    public Text m_isdone;
    public InputField m_inputfield;
    public CharacterData m_initCharData;
   

    public bool m_accountOkCheck = false;
    public bool m_alreadyCheck = true;
    
    public string login = "Login";
    public string logout = "Logout";
    public string loginCheck = "";
    public string m_createIdUrl = "http://54.238.128.34/InsertUser.php";
    public string m_checkIdUrl = "http://54.238.128.34/useridcheck.php";
    public string m_adduserUrl = "http://54.238.128.34/adduser.php";
    public string m_getid;
    private string m_nullstr = "";

    void Awake()
    {
        //m_mainPanel.interactable = false;
        m_alreadyaccountPopup.SetActive(false);
        m_createAccountPopup.SetActive(false);
        
    }
    // Use this for initialization
    void Start ()
    {
       
    }
    void Update()
    {        
        if (GPGSMgr.GetInstance.m_bLogin && m_createAccountPopup.activeSelf)
        {
            inputID = GPGSMgr.GetInstance.GetIDGPGS();
            inputEmail = GPGSMgr.GetInstance.GetMailGPGS();
            
            m_text.text = string.Format("ID : {0} \n Mail : {1} \n Name : {2}", inputID, inputEmail, inputName);

            ShowInputField();
        }

        if (GPGSMgr.GetInstance.m_bLogin == false && m_createAccountPopup.activeSelf)
        {
            m_loginText.text = string.Format("GoogleLogin");
            loginCheck = logout;

            ShowInputField();
        }
        else if(GPGSMgr.GetInstance.m_bLogin )
        {
            m_loginText.text = string.Format("{0}", logout);
            loginCheck = login;

            ShowInputField();
        }
        if (m_accountOkCheck)
        {
            StartCoroutine(CreateAccountCheck());
        }


        if (AssetLoader.GetInstance.m_downCheck && loginCheck == login && m_accountOkCheck && !m_alreadyaccountPopup.activeSelf )
        {
            m_mainPanel.interactable = true;
        }
        else
        {
            m_mainPanel.interactable = false;
        }
    }
    void ShowInputField()
    {
        m_userMail.text = string.Format("{0}", inputEmail);
        m_userName.text = string.Format("{0}", inputName);
        m_userID.text = string.Format("{0}", inputID);
    }
    public void SetName(string _name)
    {
        //inputName = _name;
        if(_name == "")
        {
            inputName = GPGSMgr.GetInstance.GetNameGPGS();
        }
        else if(_name != "")
        {
            inputName = _name;
        }
       
    }


    //DB에 유저 등록
    void CreateUser(string _googleid, string _email, string _username)
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", _googleid);
        form.AddField("emailPost", _email);
        form.AddField("usernamePost", _username);
       

        WWW www = new WWW(m_createIdUrl, form);

        
    }
    void AddUser(string _googleid, string _username, string _charinven, string _etcinven, int _item, int _gold)
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", _googleid);
        form.AddField("usernamePost", _username);
        form.AddField("charinvenPost", _charinven);
        form.AddField("etcinvenPost", _etcinven);
        form.AddField("itemPost", _item);
        form.AddField("goldPost", _gold);

        WWW www = new WWW(m_adduserUrl, form);
    }

    //계정 생성 창 띄움
    public void CreateAccountPopup()
    {
        m_createAccountPopup.SetActive(true);
        m_accountOkCheck = true;
        inputName = m_nullstr;
        inputID = m_nullstr;
        inputEmail = m_nullstr;

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

            if (m_getid == "true")
            {
                m_isdone.text = string.Format("{0}", m_getid);
                //m_alreadyCheck = false;

                LoadData.GetInstance.m_username = inputName;
                
                //m_accountOkCheck = false;
            }
            else if (m_getid == "false")
            {
                // 계정 중복되면 중복메시지 창 띄움 
                // 추후 스케일 변화 애니메이션 추가 작 -> 큰
                m_isdone.text = string.Format("{0}", m_getid);
                if (m_alreadyCheck)
                {
                    m_alreadyaccountPopup.SetActive(true);
                }
                else if (!m_alreadyCheck)
                {
                    m_alreadyaccountPopup.SetActive(false);
                }
                m_alreadyaccountPopupText.text = string.Format("already have an account");
            }
        }
    }

    public void CreateAccountCancelButton()
    {
        //GPGSMgr.GetInstance.m_bLogin = false;
        m_createAccountPopup.SetActive(false);
    }
    
    public void CreateAccountOkButton()
    {
        m_accountOkCheck = true;
        if(m_getid == "false")
        {
            m_alreadyCheck = true;
           
            //m_alreadyaccountPopup.SetActive(true);
        }
        else
        {
            CreateUser(inputID, inputEmail, inputName);
            AddUser(inputID, inputName, m_initCharJosn.ToString(), m_tempinven, LoadData.GetInstance.m_item, LoadData.GetInstance.m_gold);
            m_alreadyCheck = false;
            m_alreadyaccountPopup.SetActive(false);
            m_createAccountPopup.SetActive(false);
            LoadData.GetInstance.m_username = inputName;
        }
        
    }

    public void AlreadyAcountPopup()
    {
        // 계정 중복되면 중복메시지 창 닫음
        // 추후 스케일 변화 애니메이션 추가 큰 -> 작
        //m_accountOkCheck = false;
        m_alreadyCheck = false;
        m_alreadyaccountPopup.SetActive(false);
        
    }
       // already have an account

    public void LoginEvent()
    {
        if (!GPGSMgr.GetInstance.m_bLogin)
        {
            GPGSMgr.GetInstance.LoginGPGS();
            
        }
        else
        {
            GPGSMgr.GetInstance.LogoutGPGS();
            inputName = m_nullstr;
            inputID = m_nullstr;
            inputEmail = m_nullstr;
        }
    }

    
}
