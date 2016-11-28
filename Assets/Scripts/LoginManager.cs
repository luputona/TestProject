using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class LoginManager : MonoBehaviour {

    
    public GameObject m_createName;
    public GameObject m_createNamePanel;
    public GameObject m_mainPanel;
    public Button m_mainPanelButton;
    public Text m_already_SignCheck_Text;    
    public Text m_signText;
    public JsonData m_charinvenJsonData;
    public CharacterData[] m_charData;

    [SerializeField]
    private string m_checkIdUrl = "http://54.238.128.34/useridcheck.php";
    [SerializeField]
    private string m_addUserUrl = "http://54.238.128.34/useridcheck.php";
    
    public string m_getid = "";
    public string m_userid;
    public string m_username;
    public string m_charinven;
    public string m_etcineven;
    public string login = "Login";
    public string logout = "Logout";
    public int m_item;
    public int m_gold;
    public int m_idcheck;
    public bool m_signinCheck = false;
    public bool m_balready_SignCheck = false;

    void Awake()
    {
        m_createName = m_createNamePanel.transform.FindChild("CreateName_BG").gameObject;
        m_mainPanelButton = m_mainPanel.GetComponent<Button>();
        m_mainPanelButton.interactable = false;
    }
    // Use this for initialization
    void Start ()
    {
        m_createName.SetActive(false);

        GPGSMgr.GetInstance.LoginGPGS();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if(!GPGSMgr.GetInstance.m_bLogin)
        {
            m_signText.text = string.Format("{0}", login);            
        }
        else
        {
            m_userid = GPGSMgr.GetInstance.GetIDGPGS();
            m_signText.text = string.Format("{0}", logout);
            
            StartCoroutine(Checksignin());
        }

        
        if (m_balready_SignCheck)
        {
            m_createName.SetActive(true);            
        }
        else
        {
            m_createName.SetActive(false);
        }

        if(m_signinCheck && AssetLoader.GetInstance.m_downCheck)
        {
            m_createNamePanel.SetActive(false);
            m_mainPanelButton.interactable = true;
        }
       



    }

    public void SignEvent()
    {
        if (!GPGSMgr.GetInstance.m_bLogin)
        {
            GPGSMgr.GetInstance.LoginGPGS();            
        }
        else
        {
            GPGSMgr.GetInstance.LogoutGPGS();
        }
    }

    public void AddUser(string _goolgleId, string _username, string _charinven, string _etcinven, int _item, int _gold)
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", _goolgleId);
        form.AddField("usernamePost", _username);
        form.AddField("charinvenPost", _charinven);
        form.AddField("etcinvenPost", _etcinven);
        form.AddField("itemPost", _item);
        form.AddField("goldPost", _gold);
        LoadData.GetInstance.m_username = _username;
        LoadData.GetInstance.m_googleId = _goolgleId;
        WWW www = new WWW(m_addUserUrl, form);
    }

    IEnumerator Checksignin()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userid);

        WWW www = new WWW(m_checkIdUrl, form);

        yield return www;

        if (www.error != null)
        {
            Debug.Log("www error :" + www.error);
            yield break;
        }
        if (www.isDone)
        {

            m_getid = www.text;

            if (m_getid == "false")
            {                
                m_idcheck = 1;                
                m_signinCheck = false;
                m_balready_SignCheck = true;

                m_already_SignCheck_Text.text = string.Format("{0}", m_getid);
                if (LoadCharacterData.GetInstance.m_downcomplete)
                {
                    InitializeNewUser();
                }
                // 계정이 없으면 닉네임 생성창 띄움   
                //WWWForm form1 = new WWWForm();

                //form1.AddField("idcheckPost", m_idcheck);
                //WWW www1 = new WWW(m_checkIdUrl, form1);

                //yield return www1;
            }
            else if (m_getid == "true")
            {
                m_already_SignCheck_Text.text = string.Format("{0}", m_getid);
                // 계정이 있으면 로그인 체크
                // DB로부터 데이터 불러옴
                // 추후 스케일 변화 애니메이션 추가 작 -> 큰
                m_idcheck = 0;
                m_signinCheck = true;
                m_balready_SignCheck = false;
            }
           

        }
    }

    public void SigninAlreadyOkButton()
    {
        if(m_balready_SignCheck)
        {
            AddUser(m_userid, m_username, m_charinven, m_etcineven, LoadData.GetInstance.m_inititem, LoadData.GetInstance.m_initgold);
        }        
        m_balready_SignCheck = false;
        m_createName.SetActive(false);
    }
    public void SetName(string _name)
    {
        m_username = _name; 
    }

    void InitializeNewUser()
    {
        m_charData = new CharacterData[1];
        m_charData[0] = new CharacterData( LoadCharacterData.GetInstance.m_charList[0].Id, LoadCharacterData.GetInstance.m_charList[0].Name, LoadCharacterData.GetInstance.m_charList[0].Cost, LoadCharacterData.GetInstance.m_charList[0].Hp, LoadCharacterData.GetInstance.m_charList[0].Mp, LoadCharacterData.GetInstance.m_charList[0].Sp, LoadCharacterData.GetInstance.m_charList[0].SkillName, LoadCharacterData.GetInstance.m_charList[0].SkillMp, LoadCharacterData.GetInstance.m_charList[0].SkillDamage, LoadCharacterData.GetInstance.m_charList[0].Attack, LoadCharacterData.GetInstance.m_charList[0].AttackName, LoadCharacterData.GetInstance.m_charList[0].Defence, LoadCharacterData.GetInstance.m_charList[0].QName, LoadCharacterData.GetInstance.m_charList[0].QSp, LoadCharacterData.GetInstance.m_charList[0].QDamage, LoadCharacterData.GetInstance.m_charList[0].Profile);
        m_charinvenJsonData = JsonMapper.ToJson(m_charData);
        m_charinven = m_charinvenJsonData.ToString();
    }

   
}
