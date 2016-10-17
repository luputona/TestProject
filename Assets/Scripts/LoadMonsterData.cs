using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LoadMonsterData : Singleton<LoadMonsterData>
{
    private static LoadMonsterData Instance;  

    private JsonData m_monsterData;
    private string m_jsonString;

    public List<MonsterInfo> m_monsterList = new List<MonsterInfo>();
    
    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        //TextAsset textAsset = 
        StartCoroutine(GetMonsterData());
        
    }

    // Use this for initialization
    void Start ()
    {
        
	}

    IEnumerator GetMonsterData()
    {
        WWW www = new WWW("http://54.238.128.34/GetDB.php");
        yield return www;

        string serverDB = www.text;

        m_monsterData = JsonMapper.ToObject(serverDB);
        
        ConstructMonsterData();

        if (www.isDone)
            Debug.Log("isdone");

        Debug.Log("monster DB: " + m_monsterList[0].Name);
    }
	
	
    void ConstructMonsterData()
    {
        for(int i = 0; i < m_monsterData.Count; i++)
        {
            m_monsterList.Add(new MonsterInfo((int)m_monsterData["ID"],m_monsterData["Category"].ToString(),m_monsterData["Name"].ToString(),(int)m_monsterData["HP"],
                (int)m_monsterData["Defence"],(int)m_monsterData["Attack"], (int)m_monsterData["RecoverSP"],(int)m_monsterData["GetScore"]));
        }
    }
}
public class MonsterInfo
{
    public int Id { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Defence { get; set; }
    public int Attack { get; set; }
    public int RecoverSP { get; set; }
    public int Score { get; set; }

    public MonsterInfo(int id, string category, string name, int hp, int defence, int attack, int recoversp, int score)
    {
        this.Id = id;
        this.Category = category;
        this.Name = name;
        this.Hp = hp;
        this.Defence = defence;
        this.Attack = attack;
        this.RecoverSP = recoversp;
        this.Score = score;

    }

}
