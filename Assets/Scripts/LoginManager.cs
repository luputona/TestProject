using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class LoginManager : MonoBehaviour {

    public static string userName;
    public static string useridcode;
    
    public GameObject m_createName;
    public GameObject m_createNamePanel;
    public GameObject m_mainPanel;
    public GameObject m_inputUserCode;
    public GameObject m_accountMsg;
    public InputField m_inpufield;
    public Button m_mainPanelButton;
    public Text m_already_SignCheck_Text;    
    public Text m_signText;
    public Text m_chekingData_text;
    public JsonData m_charinvenJsonData;
    public CharacterData[] m_charData;

    [SerializeField]
    private string m_checkIdUrl = "http://54.238.128.34/useridcheck.php";
    [SerializeField]
    private string m_addUserUrl = "http://54.238.128.34/useridcheck.php";
    [SerializeField]
    private string m_useridCheckUrl = "http://54.238.128.34/UserIdCodeCheck.php";

    public string m_getid = "";
    public string m_userid;
    public string m_username;
    public string m_charinven;
    public string m_etcineven;
    public string m_createNewId;
        
    public string login = "Login";
    public string logout = "Logout";
    public int m_item;
    public int m_gold;
    public int m_idcheck;
    public int m_accountCheck;
    public bool m_signinCheck = false;
    public bool m_balready_SignCheck = false;
    public bool m_createcodeCheck = true;
    string checkid;


    void Awake()
    {
        m_accountCheck = 4;
        m_createName = m_createNamePanel.transform.FindChild("CreateName_BG").gameObject;
        m_inputUserCode = m_createNamePanel.transform.FindChild("InputCode_BG").gameObject;
        m_accountMsg = m_createNamePanel.transform.FindChild("AccountMsg_BG").gameObject;
        
        m_mainPanelButton = m_mainPanel.GetComponent<Button>();
        m_mainPanelButton.interactable = false;
        CreateUserIdCode();
        
    }
    // Use this for initialization
    void Start ()
    {
        m_createName.SetActive(false);
        m_inputUserCode.SetActive(false);
        m_accountMsg.SetActive(false);
        //StartCoroutine(UserIdCodeCheck());
    }
	
    
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("PlayerPrefs.GetString UserIdCode : " + PlayerPrefs.GetString("UserIdCode"));
        Debug.Log("PlayerPrefs.HasKey UserIdCode : " + PlayerPrefs.HasKey("UserIdCode"));


        if (AssetLoader.GetInstance.m_downCheck && PlayerPrefs.HasKey("UserIdCode") == true && m_createcodeCheck == false)
        {
            Debug.Log("ddddddddddddddd");
            StartCoroutine(Checksignin());
            //Debug.Log("m_signinCheck : "+ m_signinCheck);
            //Debug.Log("m_getid : "+ m_getid);
            //Debug.Log("m_userid : " + m_userid);
            //Debug.Log("checkid : " + checkid);
        }
        else if (AssetLoader.GetInstance.m_downCheck && PlayerPrefs.HasKey("UserIdCode") == false && m_createcodeCheck)
        {
            Debug.Log("eeeeeeeeeeeeee");
            StartCoroutine(UserIdCodeCheck());
        }

        if (m_balready_SignCheck)
        {
            m_accountMsg.SetActive(true);
        }
        else if (!m_balready_SignCheck)
        {
            m_accountMsg.SetActive(false);
        }

        if (m_accountCheck.Equals(1))
        {
            m_createName.SetActive(true);
        }
        else
        {
            m_createName.SetActive(false);
        }

        if (m_accountCheck.Equals(0))
        {
            m_inputUserCode.SetActive(true);
        }
        else
        {
            m_inputUserCode.SetActive(false);
        }


        if (m_signinCheck && AssetLoader.GetInstance.m_downCheck)
        {
            m_chekingData_text.text = string.Format("Complete Checking Data");
            m_createNamePanel.SetActive(false);
            m_mainPanelButton.interactable = true;
        }
        else
        {
            m_chekingData_text.text = string.Format("Checking Data");
            m_mainPanelButton.interactable = false;
        }

        
    }


    IEnumerator Checksignin()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userid);

        //form.AddField("googleidPost", "g09033669007273611046");
        //m_userid = "g09033669007273611046";
        WWW www = new WWW(m_checkIdUrl, form);

        yield return www;

        if (www.error != null)
        {
            Debug.Log("www error :" + www.error);
            yield break;
        }
        if (www.isDone)
        {
            InitializeUserStatus.GetInstance.m_goolgleid = m_userid;
            m_getid = www.text;

            if (m_getid == "false")
            {                
                m_signinCheck = false;
                m_balready_SignCheck = true;

                m_already_SignCheck_Text.text = string.Format("{0}", m_getid);
                if (LoadCharacterData.GetInstance.m_downcomplete)
                {
                    InitializeNewUser();
                }
            }
            else if (m_getid == "true")
            {
                m_already_SignCheck_Text.text = string.Format("{0}", m_getid);
                
                m_signinCheck = true;
                m_balready_SignCheck = false;
                // 계정이 있으면 로그인 체크
                // DB로부터 데이터 불러옴
                // 추후 스케일 변화 애니메이션 추가 작 -> 큰
            }
        }
        
    }

    IEnumerator UserIdCodeCheck()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_createNewId);

        WWW www = new WWW(m_useridCheckUrl, form);

        yield return www;

        if (www.error != null)
        {
            Debug.Log("www error :" + www.error);
            yield break;
        }
        if (www.isDone)
        {
            checkid = www.text;

            if (checkid == "true" && m_createcodeCheck) //DB에 중복 코드가 있을때
            {
                Debug.Log("cccccccccccccc");
                CreateUserIdCode();
            }
            else if (checkid == "false") //DB에 중복 코드가 없을때
            {
                m_userid = m_createNewId;
                m_createcodeCheck = false;
                StartCoroutine(Checksignin()); // 계정등록
                
                Debug.Log("aaaaaaaaaaa");
            }
            
            Debug.Log("m_createcodeCheck : " + m_createcodeCheck);
            yield break;
        }
    }

    //ui에 연결
    public void SigninAlreadyOkButton()
    {
        if(m_balready_SignCheck)
        {
            AddUser(m_userid, m_username, m_charinven, m_etcineven, InitializeUserStatus.GetInstance.m_inititem, InitializeUserStatus.GetInstance.m_initgold, InitializeUserStatus.GetInstance.m_inithp, InitializeUserStatus.GetInstance.m_initmp, InitializeUserStatus.GetInstance.m_initattack, InitializeUserStatus.GetInstance.m_initdefence, InitializeUserStatus.GetInstance.m_initscore, InitializeUserStatus.GetInstance.m_initstatpoint, InitializeUserStatus.GetInstance.m_initmaincharacter);
            InitializeUserStatus.GetInstance.m_goolgleid = m_userid;
        }
        PlayerPrefs.SetString("UserIdCode", m_userid);
        m_balready_SignCheck = false;
        m_accountCheck = 3;
        m_createName.SetActive(false);
    }

    public void SetInputUserCode()
    {
        string temp = m_inpufield.text;
        PlayerPrefs.SetString("UserIdCode", temp);
        m_userid = PlayerPrefs.GetString("UserIdCode");
       
    }

    public void InputUserCodeOKButton()
    {
        m_inputUserCode.SetActive(false);
        m_accountCheck = 4;
    }

    public void SetName(string _name)
    {
        m_username = _name; 
    }

    public void NewAccountButton()
    {
        
        m_accountCheck = 1;
        m_balready_SignCheck = false;
    }
    public void ContinueButton()
    {
        m_accountCheck = 0;
        m_balready_SignCheck = false;
    }

    public void InputUserCodeCancelButton()
    {
        m_accountCheck = 4;
    }

    public void NewAccountCancelButton()
    {
        m_accountCheck = 4;
    }

    void CreateUserIdCode()
    {
        string[] str = new string[10];
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            int ran = Random.Range(100, 999);
            str[i] = ran.ToString();
            //Debug.Log("str : " + i + " : " + str[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            int ran2 = Random.Range(0, 10);
            string str2 = str[ran2];
            sb.Append(str2);
        }
        int ran3 = Random.Range(1, 10);
        if (PlayerPrefs.HasKey("UserIdCode") == true)
        {
            m_createNewId = PlayerPrefs.GetString("UserIdCode");
        }
        else
        {
            m_createNewId =   ran3.ToString();  // sb.ToString();        
        }

        Debug.Log("create new ID : " + ran3);
        //Debug.Log("ID : " + sb.ToString());
    }

    void InitializeNewUser()
    {
        m_charData = new CharacterData[1];
        m_charData[0] = new CharacterData( LoadCharacterData.GetInstance.m_charList[0].Id, LoadCharacterData.GetInstance.m_charList[0].Name, LoadCharacterData.GetInstance.m_charList[0].Cost, LoadCharacterData.GetInstance.m_charList[0].Hp, LoadCharacterData.GetInstance.m_charList[0].Mp, LoadCharacterData.GetInstance.m_charList[0].Sp, LoadCharacterData.GetInstance.m_charList[0].SkillName, LoadCharacterData.GetInstance.m_charList[0].SkillMp, LoadCharacterData.GetInstance.m_charList[0].SkillDamage, LoadCharacterData.GetInstance.m_charList[0].Attack, LoadCharacterData.GetInstance.m_charList[0].AttackName, LoadCharacterData.GetInstance.m_charList[0].Defence, LoadCharacterData.GetInstance.m_charList[0].QName, LoadCharacterData.GetInstance.m_charList[0].QSp, LoadCharacterData.GetInstance.m_charList[0].QDamage, LoadCharacterData.GetInstance.m_charList[0].Profile);
        m_charinvenJsonData = JsonMapper.ToJson(m_charData);
        m_charinven = m_charinvenJsonData.ToString();
        m_etcineven = "";
    }
    public void AddUser(string _userIdCode, string _username, string _charinven, string _etcinven, int _item, int _gold, int _hp, int _mp, int _attack, int _defence, int _score, int _statpoint, string _maincharacter)
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", _userIdCode);
        form.AddField("usernamePost", _username);
        form.AddField("charinvenPost", _charinven);
        form.AddField("etcinvenPost", _etcinven);
        form.AddField("itemPost", _item);
        form.AddField("goldPost", _gold);
        form.AddField("hpPost", _hp);
        form.AddField("mpPost", _mp);
        form.AddField("attackPost", _attack);
        form.AddField("defencePost", _defence);
        form.AddField("scorePost", _score);
        form.AddField("statpointPost", _statpoint);
        form.AddField("maincharacterPost", _maincharacter);


        PlayerPrefs.SetString("UserName", _username);
        PlayerPrefs.SetString("UserIdCode", _userIdCode);
        WWW www = new WWW(m_addUserUrl, form);
    }
    //public void AddUser()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("googleidPost", m_userid);
    //    form.AddField("usernamePost", m_username);
    //    form.AddField("charinvenPost", m_charinven);
    //    form.AddField("etcinvenPost", m_etcineven);
    //    form.AddField("itemPost", 0);
    //    form.AddField("goldPost", 0);
    //    form.AddField("hpPost", 0);
    //    form.AddField("mpPost", 0);
    //    form.AddField("attackPost", 0);
    //    form.AddField("defencePost", 0);
    //    form.AddField("scorePost", 0);
    //    form.AddField("statpointPost", 0);
    //    form.AddField("maincharacterPost", "");
    //    LoadData.GetInstance.m_username = _username;
    //    LoadData.GetInstance.m_userIdCode = _goolgleId;
    //    WWW www = new WWW(m_addUserUrl, form);
    //}

    //public void SignEvent()
    //{
        //if (!GPGSMgr.GetInstance.m_bLogin)
        //{
        //    GPGSMgr.GetInstance.LoginGPGS();            
        //}
        //else
        //{
        //    GPGSMgr.GetInstance.LogoutGPGS();
        //}
        //if(PlayerPrefs.HasKey("UserIdCode"))
        //{            
        //    PlayerPrefs.DeleteKey("UserIdCode");
        //    m_userid = PlayerPrefs.GetString("UserIdCode");
        //    m_signinCheck = false;
        //}
        //else if(PlayerPrefs.GetString("UserIdCode") == null)
        //{
        //    m_accountMsg.SetActive(true);
        //}
    //}


}
