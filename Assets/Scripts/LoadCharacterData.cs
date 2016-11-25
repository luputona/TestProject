using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LoadCharacterData : Singleton<LoadCharacterData>
{
    private static LoadCharacterData Instance;

    private JsonData m_charData;
    public bool m_downcomplete = false;
    public List<CharacterData> m_charList = new List<CharacterData>();

    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        StartCoroutine(GetCharacterData());
        
    }
    void Start()
    {
        //DataInsert.GetInstance.m_initCharData = new CharacterData(LoadCharacterData.GetInstance.m_charList[0].Name, LoadCharacterData.GetInstance.m_charList[0].Id, LoadCharacterData.GetInstance.m_charList[0].Cost, LoadCharacterData.GetInstance.m_charList[0].Hp, LoadCharacterData.GetInstance.m_charList[0].Mp, LoadCharacterData.GetInstance.m_charList[0].Sp, LoadCharacterData.GetInstance.m_charList[0].SkillName, LoadCharacterData.GetInstance.m_charList[0].SkillMp, LoadCharacterData.GetInstance.m_charList[0].SkillDamage, LoadCharacterData.GetInstance.m_charList[0].Attack, LoadCharacterData.GetInstance.m_charList[0].AttackName, LoadCharacterData.GetInstance.m_charList[0].Defence, LoadCharacterData.GetInstance.m_charList[0].QName, LoadCharacterData.GetInstance.m_charList[0].QSp, LoadCharacterData.GetInstance.m_charList[0].QDamage, LoadCharacterData.GetInstance.m_charList[0].Profile);
        //DataInsert.GetInstance.m_initCharJosn = JsonMapper.ToJson(DataInsert.GetInstance.m_initCharData);
    }

    IEnumerator GetCharacterData()
    {
        WWW www = new WWW("http://54.238.128.34/GetCharacterDB.php");
        yield return www;

        string serverDB = www.text;

        m_charData = JsonMapper.ToObject(serverDB);
        
        if (www.isDone)
        {
            m_downcomplete = true;
            //Debug.Log("CharDB isDone");
        }
        ConstructCharacterData();
        Debug.Log("Character DB: " + m_charList[1].Name);
        
        //CharacterControllManager.GetInstance.InitializeCharacter();

        
    }


    void ConstructCharacterData()
    {
        for(int i = 0; i< m_charData.Count; i++)
        {
            m_charList.Add(new CharacterData(                
                (int)m_charData[i]["Id"],
                m_charData[i]["Name"].ToString(),
                (int)m_charData[i]["Cost"], 
                (int)m_charData[i]["Hp"],
                (int)m_charData[i]["Mp"], 
                (int)m_charData[i]["Sp"],
                m_charData[i]["SkillName"].ToString(),
                (int)m_charData[i]["SkillMp"],
                (int)m_charData[i]["SkillDamage"], 
                (int)m_charData[i]["Attack"], 
                m_charData[i]["AttackName"].ToString(),
                (int)m_charData[i]["Defence"], 
                m_charData[i]["QName"].ToString(), 
                (int)m_charData[i]["QSp"], 
                (int)m_charData[i]["QDamage"],
                m_charData[i]["Profile"].ToString()));
        }

    }

}
[System.Serializable]
public class CharacterData
{
    public string Name; //{ get; set; }
    public int Id; //{ get; set; }    
    public int Cost;// { get; set; }
    public int Hp;// { get; set; }
    public int Mp; //{ get; set; }
    public int Sp; //{ get; set; }

    public string SkillName; //{ get; set; }
    public int SkillMp; //{ get; set; }
    public int SkillDamage; //{ get; set; }

    public int Attack; //{ get; set; }
    public string AttackName;
    public int Defence; //{ get; set; }

    public string QName; //{ get; set; }
    public int QSp; //{ get; set; }
    public int QDamage; //{ get; set; }

    public string Profile;
   

    public CharacterData(int id, string name, int cost, int hp, int mp, int sp, string skillname, int skillMp, int skillDamage, int attack,string attackname , int defence, string qname, int qsp, int qDamage, string _profile )
    {
        this.Id = id;
        this.Name = name;
        this.Cost = cost;
        this.Hp = hp;
        this.Mp = mp;
        this.Sp = sp;
        this.SkillName = skillname;
        this.SkillMp = skillMp;
        this.SkillDamage = skillDamage;
        this.Attack = attack;
        this.AttackName = attackname;
        this.Defence = defence;
        this.QName = qname;
        this.QSp = qsp;
        this.QDamage = qDamage;
        this.Profile = _profile;
        

    }
}
