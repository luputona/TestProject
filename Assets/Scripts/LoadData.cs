using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{

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
        m_playername = GPGSMgr.GetInstance.GetNameGPGS();

        m_hp = m_inithp;
        m_mp = m_initmp;
        m_attack = m_initattack;
        m_defence = m_initdefence;
        m_item = m_inititem;
        m_gold = m_initgold;


        PlayerPrefs.GetString("SelectCharacter");
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
