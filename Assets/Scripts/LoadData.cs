using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using LitJson;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{
    public List<CharacterData> m_localcharList = new List<CharacterData>();
    public string m_userIdCode;
    public string m_userId;
    public string m_username;
   
    public string[] m_dbText;
    public int m_gold;
    public int m_item;
    public int m_hp;
    public int m_mp;
    public int m_attack;
    public int m_defence;
    public int m_score;
    public int m_statpoint;
    public string m_maincharacter;
    public string m_charinven;
    public string m_etcinven;
       

    public string m_selectCharacter;
    public string m_selectVehicle;

    //public int m_inithp; //{ get; set; }
    //public int m_initmp; //{ get; set; }
    //public int m_initattack; //{ get; set; }
    //public int m_initdefence; //{ get; set; }
    //public int m_initgold; //{ get; set; }
    //public int m_inititem; //{ get; set; }
    //public int m_initscore;
    //public int m_initstatpoint;
    //public string m_initmaincharacter;


    public Button m_panelButton;
    public Text m_progressText;
    public bool m_downcheck = false;
    public float m_currentValue = 0;
    private JsonData m_charJsonData;
    private string jsonstring;
    

    [SerializeField]
    private string m_userdataUrl = "http://54.238.128.34/userdb.php";
    [SerializeField]
    private string m_uploadUrl = "http://54.238.128.34/Updateuserdata.php";

    void Awake()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }

        //StartCoroutine(GetCharacterData());


        //TextAsset text = Resources.Load<TextAsset>("StreamingAssets/PlayerChar");
        //jsonstring = text.ToString();
        //m_charJsonData = JsonMapper.ToObject(jsonstring);
        //print(text.ToString());


        //m_inithp = 100;
        //m_initmp = 100;
        //m_initattack = 5;
        //m_initdefence = 5;

        //m_initgold = 10000;
        //m_inititem = 1;
        //m_initmaincharacter = "UnityChan";
        
        //if (m_panelButton == null)
        //{
        //    m_panelButton = GameObject.Find("PanelButton").GetComponent<Button>();
        //}
        
    }
        
    void Start()
    {
        //m_panelButton.interactable = false;
        m_userIdCode = PlayerPrefs.GetString("UserIdCode");
        m_hp = InitializeUserStatus.GetInstance.m_inithp;
        m_mp = InitializeUserStatus.GetInstance.m_initmp;
        m_attack = InitializeUserStatus.GetInstance.m_initattack;
        m_defence = InitializeUserStatus.GetInstance.m_initdefence;
        m_item = InitializeUserStatus.GetInstance.m_inititem;
        m_gold = InitializeUserStatus.GetInstance.m_initgold;


        StartCoroutine(DownloadUserData());
        

    }

    void Update()
    {
        //if(m_panelButton == null)
        //{
        //    m_panelButton = null;
        //    //m_panelButton = GameObject.FindGameObjectWithTag("DataLoadScene_Loading_BG").gameObject.GetComponent<Button>();            
        //}
        //else
        //{
        //    if (m_currentValue >= 100)
        //    {
        //        m_panelButton.interactable = true;
        //    }
        //}

        //if (m_progressText == null)
        //{
        //    m_progressText = null;
        //    //m_progressText = GameObject.FindGameObjectWithTag("DataLoadScene_progress_Text").gameObject.GetComponent<Text>();
        //}
        //else
        //{
        //    m_progressText.text = string.Format("{0:0}%", m_currentValue);
        //}
       
        if(PlayerPrefs.HasKey("UserIdCode"))
        {
            m_userIdCode = PlayerPrefs.GetString("UserIdCode");
        }
        

        
        //m_text.text = string.Format("{0} /// {1}", m_localcharList[2].Name , m_localcharList.Count);
    }
    void ConstructLocalCharDatabase()
    {
        for (int i = 0; i < m_charJsonData.Count; i++)
        {
            m_localcharList.Add(new CharacterData(
              (int)m_charJsonData[i]["Id"],
              m_charJsonData[i]["Name"].ToString(),
              (int)m_charJsonData[i]["Cost"],
              (int)m_charJsonData[i]["Hp"],
              (int)m_charJsonData[i]["Mp"],
              (int)m_charJsonData[i]["Sp"],
              m_charJsonData[i]["SkillName"].ToString(),
              (int)m_charJsonData[i]["SkillMp"],
              (int)m_charJsonData[i]["SkillDamage"],
              (int)m_charJsonData[i]["Attack"],
              m_charJsonData[i]["AttackName"].ToString(),
              (int)m_charJsonData[i]["Defence"],
              m_charJsonData[i]["QName"].ToString(),
              (int)m_charJsonData[i]["QSp"],
              (int)m_charJsonData[i]["QDamage"],
              m_charJsonData[i]["Profile"].ToString()));
           
           
        }
    }
   

    //---------------DB에서 일괄 다운로드 -------------------
    public IEnumerator DownloadUserData()
    {
        WWWForm form = new WWWForm();
        //form.AddField("googleidPost", "g09033669007273611046");
        form.AddField("googleidPost", m_userIdCode);
        WWW www = new WWW(m_userdataUrl, form);

        yield return www;
        string dbText = www.text;
        m_dbText = dbText.Split(';');
        if (www.error != null)
        {
            Debug.Log("user Data Dowload Error");
        }
        while(!www.isDone)
        {
            m_currentValue = www.progress * 100f;
            yield return null;
        }
        m_currentValue = 100f;
        if(www.isDone)
        {            
            m_userId = SplitData(m_dbText[0],"userid:");
            m_username = SplitData(m_dbText[0], "username:");
            m_charinven = SplitData(m_dbText[0], "characterinventory:");
            m_etcinven = SplitData(m_dbText[0], "etcinventory:");
            m_item =  int.Parse(SplitData(m_dbText[0], "item:"));
            m_gold = int.Parse(SplitData(m_dbText[0], "gold:"));
            m_hp = int.Parse(SplitData(m_dbText[0], "hp:"));
            m_mp = int.Parse(SplitData(m_dbText[0], "mp:"));
            m_attack = int.Parse(SplitData(m_dbText[0], "attack:"));
            m_defence = int.Parse(SplitData(m_dbText[0], "defence:"));
            m_score = int.Parse(SplitData(m_dbText[0], "score:"));
            m_statpoint = int.Parse(SplitData(m_dbText[0], "statpoint:"));
            m_maincharacter = SplitData(m_dbText[0], "maincharacter:");

            PlayerPrefs.SetString("SelectCharacter", m_maincharacter);

            m_charJsonData = JsonMapper.ToObject(m_charinven);
            ConstructLocalCharDatabase();
            m_downcheck = true;
            //print("jsonstring :" + m_charinven.ToString());

            //print("list : "+m_localcharList[0].Name);
            UploadAllData();
        }        
        

    }


    string SplitData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);

        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }


    //-------------------- DB로 업로드 ----------------------------
    public void UploadName(string _name)
    {
        m_username = _name;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("usernamePost", m_username);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadCharacterInventory(string _charinven)
    {
        m_charinven = _charinven;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("characterinvenPost", m_charinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadEtcInventory(string _etcinven)
    {
        m_etcinven = _etcinven;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("ectinvenPost", m_etcinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadItem(int _item)
    {
        m_item = _item;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("itemPost", m_item);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadGold(int _gold)
    {
        m_gold = _gold;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("goldPost", m_gold);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadHp(int _hp)
    {
        m_hp = _hp;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("hpPost", m_hp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadMp(int _mp)
    {
        m_mp = _mp;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("mpPost", m_mp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadAttack(int _attack)
    {
        m_attack = _attack;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("attackPost", m_attack);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadDefence(int _defence)
    {
        m_defence = _defence;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("defencePost", m_defence);
        WWW www = new WWW(m_uploadUrl, form);
    }

    public void UploadScore(int _score)
    {
        m_score = _score;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("scorePost", m_score);
        WWW www = new WWW(m_uploadUrl, form);
    }
    
    public void UploadStatPoint(int _statpoint)
    {
        m_statpoint = _statpoint;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("statpointPost", m_statpoint);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadMainCharacter(string _mainchar)
    {
        m_maincharacter = _mainchar;
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("maincharacterPost", m_maincharacter);
        WWW www = new WWW(m_uploadUrl, form);
    }

    public void UploadAllData()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_userId);
        form.AddField("usernamePost", m_username);
        form.AddField("characterinvenPost", m_charinven);
        form.AddField("ectinvenPost", m_etcinven);
        form.AddField("itemPost", m_item);
        form.AddField("goldPost", m_gold);
        form.AddField("hpPost", m_hp);
        form.AddField("mpPost", m_mp);
        form.AddField("attackPost", m_attack);
        form.AddField("defencePost", m_defence);
        form.AddField("scorePost", m_score);
        form.AddField("statpointPost", m_statpoint);
        form.AddField("maincharacterPost", m_maincharacter);
        WWW www = new WWW(m_uploadUrl, form);
        //UploadName(m_username);
        //UploadCharacterInventory(m_charinven);
        //UploadEtcInventory(m_etcinven);
        //UploadItem(m_item);
        //UploadGold(m_gold);
        //UploadHp(m_hp);
        //UploadMp(m_mp);
        //UploadAttack(m_attack);
        //UploadDefence(m_defence);
        //UploadScore(m_score);
        //UploadStatPoint(m_statpoint);
        //UploadMainCharacter(m_maincharacter);
    }


    void OnApplicationPause(bool status)
    {
        if(status)
        {
            //나갔을경우 , 어플정지
            UploadAllData();
        }
        else
        {
            //어플 재개
         
        }
    }


    //public void LoadInventory(List<CharacterData> _loadinven)
    //{
    //    _loadinven = new List<CharacterData>();
    //    for(int i = 0; i < _loadinven.Count; i++ )
    //    {
    //        MainInvenUIManager.GetInstance.m_inventory.Add(_loadinven[i]);
    //    }
    //}



}
