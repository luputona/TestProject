using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{
    public List<CharacterData> m_localcharList = new List<CharacterData>();
    public string m_playername;// { get; set; }

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

    
    private JsonData m_charJsonData;
    string jsonstring;
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


        TextAsset text = Resources.Load<TextAsset>("StreamingAssets/PlayerChar");
        jsonstring = text.ToString();
        m_charJsonData = JsonMapper.ToObject(jsonstring);
        print(text.ToString());

        
    }

    public LoadData()
    {
    }
    
    void Start()
    {
        m_inithp = 100; 
        m_initmp = 100; 
        m_initattack = 5;
        m_initdefence = 5;

        m_initgold = 10000; 
        m_inititem = 1;
        //if (PlayerPrefs.GetString("UserName") == "" || PlayerPrefs.GetString("UserName") == GPGSMgr.GetInstance.GetNameGPGS())
        //{
        //    m_playername = GPGSMgr.GetInstance.GetNameGPGS();
        //}
        //else
        //{
        //    m_playername = PlayerPrefs.GetString("UserName");
        //}
            

        m_hp = m_inithp;
        m_mp = m_initmp;
        m_attack = m_initattack;
        m_defence = m_initdefence;
        m_item = m_inititem;
        m_gold = m_initgold;

        ConstructLocalCharDatabase();
    }
    void ConstructLocalCharDatabase()
    {
        
        for (int i = 0; i < m_charJsonData.Count; i++)
        {
            m_localcharList.Add(new CharacterData(
                m_charJsonData[i]["Name"].ToString(),
                (int)m_charJsonData[i]["Id"],
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

    IEnumerator GetCharacterData()
    {
        WWW www = new WWW(Application.dataPath + "/Resources/StreamingAssets/PlayerChar.txt");
        yield return www;

        string m_charstring = www.text;

        m_charJsonData = JsonMapper.ToObject(m_charstring);

        if (www.isDone)
        {

            Debug.Log("CharDB isDone :\n " + m_charstring.ToString());
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
