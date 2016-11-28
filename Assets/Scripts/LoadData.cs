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
    public string m_googleId;
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

    public int m_inithp; //{ get; set; }
    public int m_initmp; //{ get; set; }
    public int m_initattack; //{ get; set; }
    public int m_initdefence; //{ get; set; }
    public int m_initgold; //{ get; set; }
    public int m_inititem; //{ get; set; }

    public Text m_text;
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

        m_inithp = 100;
        m_initmp = 100;
        m_initattack = 5;
        m_initdefence = 5;

        m_initgold = 10000;
        m_inititem = 1;

        m_hp = m_inithp;
        m_mp = m_initmp;
        m_attack = m_initattack;
        m_defence = m_initdefence;
        m_item = m_inititem;
        m_gold = m_initgold;
    }

    public LoadData()
    {
    }
    
    void Start()
    {

        //if (PlayerPrefs.GetString("UserName") == "" || PlayerPrefs.GetString("UserName") == GPGSMgr.GetInstance.GetNameGPGS())
        //{
        //    m_playername = GPGSMgr.GetInstance.GetNameGPGS();
        //}
        //else
        //{
        //    m_playername = PlayerPrefs.GetString("UserName");
        //}

        StartCoroutine(DownloadUserData());
        
    }

    void Update()
    {
        
        m_text.text = string.Format("{0} /// {1}", m_userId, m_username);
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
    IEnumerator DownloadUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("googleidPost", m_googleId);
        WWW www = new WWW(m_userdataUrl, form);

        yield return www;
        string dbText = www.text;
        m_dbText = dbText.Split(';');
        if (www.error != null)
        {
            Debug.Log("user Data Dowload Error");
        }
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
            m_maincharacter = SplitData(m_dbText[0], "maincharacter");


            m_charJsonData = JsonMapper.ToObject(m_charinven.ToString());

            print("jsonstring :" + m_charinven.ToString());
            ConstructLocalCharDatabase();
            print("list : "+m_localcharList[0].Name);
            print("jsondata : " + m_charJsonData.ToString());
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
        form.AddField("usernamePost", _name);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadCharacterInventory(string _charinven)
    {
        m_charinven = _charinven;
        WWWForm form = new WWWForm();
        form.AddField("characterinvenPost", _charinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadEtcInventory(string _etcinven)
    {
        m_etcinven = _etcinven;
        WWWForm form = new WWWForm();
        form.AddField("ectinvenPost", _etcinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadItem(int _item)
    {
        m_item = _item;
        WWWForm form = new WWWForm();
        form.AddField("itemPost", _item);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadGold(int _gold)
    {
        m_gold = _gold;
        WWWForm form = new WWWForm();
        form.AddField("goldPost", _gold);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadHp(int _hp)
    {
        m_hp = _hp;
        WWWForm form = new WWWForm();
        form.AddField("hpPost", _hp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadMp(int _mp)
    {
        m_mp = _mp;
        WWWForm form = new WWWForm();
        form.AddField("mpPost", _mp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadAttack(int _attack)
    {
        m_attack = _attack;
        WWWForm form = new WWWForm();
        form.AddField("attackPost", _attack);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadDefence(int _defence)
    {
        m_defence = _defence;
        WWWForm form = new WWWForm();
        form.AddField("defencePost", _defence);
        WWW www = new WWW(m_uploadUrl, form);
    }

    public void UploadScore(int _score)
    {
        m_score = _score;
        WWWForm form = new WWWForm();
        form.AddField("scorePost", _score);
        WWW www = new WWW(m_uploadUrl, form);
    }
    
    public void UploadStatPoint(int _statpoint)
    {
        m_statpoint = _statpoint;
        WWWForm form = new WWWForm();
        form.AddField("statpointPost", _statpoint);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UploadMainCharacter(string _mainchar)
    {
        m_maincharacter = _mainchar;
        WWWForm form = new WWWForm();
        form.AddField("maincharacterPost", _mainchar);
        WWW www = new WWW(m_uploadUrl, form);
    }

    public void UploadAllData()
    {
        UploadName(m_username);
        UploadCharacterInventory(m_charinven);
        UploadEtcInventory(m_etcinven);
        UploadItem(m_item);
        UploadGold(m_gold);
        UploadHp(m_hp);
        UploadMp(m_mp);
        UploadAttack(m_attack);
        UploadDefence(m_defence);
        UploadScore(m_score);
        UploadStatPoint(m_statpoint);
        UploadMainCharacter(m_maincharacter);
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


    public void LoadInventory(List<CharacterData> _loadinven)
    {
        _loadinven = new List<CharacterData>();
        for(int i = 0; i < _loadinven.Count; i++ )
        {
            MainInvenUIManager.GetInstance.m_inventory.Add(_loadinven[i]);
        }
    }



}
