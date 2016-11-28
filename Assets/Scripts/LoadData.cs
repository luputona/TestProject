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
    public string m_playername;// { get; set; }
   
    public string[] m_dbText;
    public int m_hp;
    public int m_mp;
    public int m_attack;
    public int m_defence;
    public int m_gold;
    public int m_item;    

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
            jsonstring = SplitData(m_dbText[0], "characterinventory:");
            m_item =  int.Parse(SplitData(m_dbText[0], "item:"));
            m_gold = int.Parse(SplitData(m_dbText[0], "gold:"));
            m_charJsonData = JsonMapper.ToObject(jsonstring.ToString());

            print("jsonstring :" + jsonstring.ToString());
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

    public void UpdateName(string _name)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", _name);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateCharacterInventory(string _charinven)
    {
        WWWForm form = new WWWForm();
        form.AddField("characterinvenPost", _charinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateEtcInventory(string _etcinven)
    {
        WWWForm form = new WWWForm();
        form.AddField("ectinvenPost", _etcinven);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateItem(int _item)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemPost", _item);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateGold(int _gold)
    {
        WWWForm form = new WWWForm();
        form.AddField("goldPost", _gold);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateHp(int _hp)
    {
        WWWForm form = new WWWForm();
        form.AddField("hpPost", _hp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void Update(int _mp)
    {
        WWWForm form = new WWWForm();
        form.AddField("mpPost", _mp);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateAttack(int _attack)
    {
        WWWForm form = new WWWForm();
        form.AddField("attackPost", _attack);
        WWW www = new WWW(m_uploadUrl, form);
    }
    public void UpdateDefence(int _defence)
    {
        WWWForm form = new WWWForm();
        form.AddField("defencePost", _defence);
        WWW www = new WWW(m_uploadUrl, form);
    }

    public void UpdateScore(int _score)
    {
        WWWForm form = new WWWForm();
        form.AddField("scorePost", _score);
        WWW www = new WWW(m_uploadUrl, form);
    }
    
    public void UpdateStatPoint(int _statpoint)
    {
        WWWForm form = new WWWForm();
        form.AddField("statpointPost", _statpoint);
        WWW www = new WWW(m_uploadUrl, form);
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
