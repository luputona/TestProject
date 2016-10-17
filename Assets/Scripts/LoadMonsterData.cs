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

        TextAsset textAsset = Resources.Load
    }

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
public class MonsterInfo
{

}
