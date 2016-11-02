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

    public List<MonsterInfo> m_monsterList = new List<MonsterInfo>();
    
    void Awake()
    {
        //if (Instance != null)
        //{
        //    GameObject.Destroy(gameObject);
        //}
        //else
        //{
        //    GameObject.DontDestroyOnLoad(gameObject);
        //    Instance = this;
        //}
        
        StartCoroutine(GetMonsterData());        
    }
    
    IEnumerator GetMonsterData()
    {
        WWW www = new WWW("http://54.238.128.34/GetMonsterDB.php");
        yield return www;

        string serverDB = www.text;

        m_monsterData = JsonMapper.ToObject(serverDB);
        
        if (www.isDone)
        {
            //Debug.Log("isdone");
        }

        ConstructMonsterData();
        //Debug.Log("monster DB: " + m_monsterList[1].Name);
    }
	
	
    void ConstructMonsterData()
    {
        for(int i = 0; i < m_monsterData.Count; i++)
        {
            m_monsterList.Add(new MonsterInfo(
                (int)m_monsterData[i]["ID"], 
                m_monsterData[i]["Category"].ToString(), 
                m_monsterData[i]["Name"].ToString(), 
                (int)m_monsterData[i]["HP"], 
                (int)m_monsterData[i]["Defence"],
                (int)m_monsterData[i]["Attack"], 
                (int)m_monsterData[i]["RecoverSP"],
                (int)m_monsterData[i]["GetScore"]));
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
