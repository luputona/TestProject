using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UserInfomation : Singleton<UserInfomation>
{
    public GameObject[] m_currentCharSpec;
    public Dictionary<string, Text> m_m_currentCharSpecText = new Dictionary<string, Text>();
    public Image m_charInfoImage;


    private int m_hp;
    private int m_mp;
    private int m_attack;
    private int m_defence;
    private string m_name;

    void Awake()
    {
        InitializeUI();
    }
    // Use this for initialization
    void Start ()
    {
        InitailizeCharacterInfo();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    void InitializeUserInfo()
    {   
        
    }
    void InitailizeCharacterInfo()
    {
        //PlayerPrefs 나중에 서버연동으로 변경 
        m_m_currentCharSpecText["Cur_Char_Hp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("hp") );
        m_m_currentCharSpecText["Cur_Char_Mp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("mp"));
        m_m_currentCharSpecText["Cur_Char_Attack_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("attack"));
        m_m_currentCharSpecText["Cur_Char_Defence_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("defence"));
        m_m_currentCharSpecText["Char_Specs_Title_Text"].text = string.Format("{0}", PlayerPrefs.GetString("SelectCharacter"));
    }

    void InitializeUI()
    {
        int count = GameObject.FindGameObjectsWithTag("CurrentCharStatus").Length;
        m_currentCharSpec = new GameObject[count];
        m_currentCharSpec = GameObject.FindGameObjectsWithTag("CurrentCharStatus");

        for(int i = 0; i < count; i++)
        {
            m_m_currentCharSpecText.Add(m_currentCharSpec[i].GetComponent<Text>().name, m_currentCharSpec[i].GetComponent<Text>());
        }


    }
    public void ShowCharacterSpec(int _hp, int _mp, int _attack, int _defence, string _name, Image _sprite)
    {

        m_m_currentCharSpecText["Cur_Char_Hp_Text"].text = string.Format("{0}", _hp);
        m_m_currentCharSpecText["Cur_Char_Mp_Text"].text = string.Format("{0}", _mp);
        m_m_currentCharSpecText["Cur_Char_Attack_Text"].text = string.Format("{0}", _attack);
        m_m_currentCharSpecText["Cur_Char_Defence_Text"].text = string.Format("{0}", _defence);
        m_m_currentCharSpecText["Char_Specs_Title_Text"].text = string.Format("{0}", _name);
        m_charInfoImage.sprite = _sprite.sprite;
    }

    
}
