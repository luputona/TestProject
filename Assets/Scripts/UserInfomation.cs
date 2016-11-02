using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다
public class UserInfomation : Singleton<UserInfomation>
{
    public GameObject[] m_currentCharSpec;
    public GameObject[] m_currentUserInfo;
    public GameObject[] m_userTotalStatus;
    public Dictionary<string, Text> m_currentUserInfoText = new Dictionary<string, Text>();
    public Dictionary<string, Text> m_currentCharSpecText = new Dictionary<string, Text>();
    public Dictionary<string, Text> m_totalSpecText = new Dictionary<string, Text>();
    public Image m_charInfoImage;

    public string m_name;
    public int m_hp;
    public int m_mp;
    public int m_attack;
    public int m_defence; 
    public int m_level; 
    public int m_gold; 
    public int m_item;
    public string m_selectCharacter;

    SaveData savedata = new SaveData();

    public Text testtext;
    // Use this for initialization
    void Start ()
    {
        //InitailizeCharacterInfo();
        
        GPGSMgr.GetInstance.SaveGame();
        GPGSMgr.GetInstance.LoadGame();

        StartCoroutine(UserInfoUpdate());

    }
    
    void Update()
    {
        ShowUserInfomation();
        ShowUserTotalStatus();


        for (int i = 0; i < savedata.m_saveInventory.Count; i++)
        {
            testtext.text = string.Format("{0}", savedata.m_saveInventory[i].Name);
        }

    }
	
	IEnumerator UserInfoUpdate()
    {
        while(true)
        {
            
            yield return null;
        }
    }

    void InitializeUserInfo()
    {   
        
    }
    public void InitailizeCharacterInfo()
    {
        //PlayerPrefs 나중에 서버연동으로 변경 
        m_currentCharSpecText["Cur_Char_Hp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("hp") );
        m_currentCharSpecText["Cur_Char_Mp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("mp"));
        m_currentCharSpecText["Cur_Char_Attack_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("attack"));
        m_currentCharSpecText["Cur_Char_Defence_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("defence"));
        m_currentCharSpecText["Char_Specs_Title_Text"].text = string.Format("{0}", PlayerPrefs.GetString("SelectCharacter"));


        if (PlayerPrefs.GetString("SelectCharacter") == "UnityChan")
        {
            m_charInfoImage.sprite = MainInvenUIManager.GetInstance.m_image["01_portrait_kohaku_01"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            m_charInfoImage.sprite = MainInvenUIManager.GetInstance.m_image["02_portrait_yuko_01"];
        }
    }

    public void InitializeUI()
    {
        int count = GameObject.FindGameObjectsWithTag("CurrentCharStatus").Length;
        m_currentCharSpec = new GameObject[count];
        m_currentCharSpec = GameObject.FindGameObjectsWithTag("CurrentCharStatus");
        for(int i = 0; i < count; i++)
        {
            m_currentCharSpecText.Add(m_currentCharSpec[i].GetComponent<Text>().name, m_currentCharSpec[i].GetComponent<Text>());
        }

        int userinfocount = GameObject.FindGameObjectsWithTag("CurrentUserStatus").Length;
        m_currentUserInfo = new GameObject[userinfocount];
        m_currentUserInfo = GameObject.FindGameObjectsWithTag("CurrentUserStatus");

        if (m_currentUserInfo == null)
        {
            m_currentUserInfo = null;
        }
        else
        {
            for (int i = 0; i < userinfocount; i++)
            {
                m_currentUserInfoText.Add(m_currentUserInfo[i].GetComponent<Text>().name, m_currentUserInfo[i].GetComponent<Text>());
            }
        }
       
        int usertotalstatuscount = GameObject.FindGameObjectsWithTag("UserTotalStatus").Length;
        m_userTotalStatus = new GameObject[usertotalstatuscount];
        m_userTotalStatus = GameObject.FindGameObjectsWithTag("UserTotalStatus");
        if(m_userTotalStatus == null)
        {
            m_userTotalStatus = null;
        }
        else
        {
            for (int i = 0; i < usertotalstatuscount; i++)
            {
                m_totalSpecText.Add(m_userTotalStatus[i].transform.name, m_userTotalStatus[i].GetComponent<Text>());
            }
        }

    }
    public void ShowCharacterSpec(int _hp, int _mp, int _attack, int _defence, string _name, Sprite _sprite)
    {
        m_currentCharSpecText["Cur_Char_Hp_Text"].text = string.Format("{0}", _hp);
        m_currentCharSpecText["Cur_Char_Mp_Text"].text = string.Format("{0}", _mp);
        m_currentCharSpecText["Cur_Char_Attack_Text"].text = string.Format("{0}", _attack);
        m_currentCharSpecText["Cur_Char_Defence_Text"].text = string.Format("{0}", _defence);
        m_currentCharSpecText["Char_Specs_Title_Text"].text = string.Format("{0}", _name);
        m_charInfoImage.sprite = _sprite;
    }
    public void GetUserData(string _userName, int _level, int _hp, int _mp, int _attack, int _defence, int _gold, int _item, string _selectcharacter)
    {
        m_name = _userName;
        m_hp = _hp;
        m_mp = _mp;
        m_attack = _attack;
        m_defence = _defence;
        m_level = _level;
        m_gold = _gold;
        m_item = _item;
        m_selectCharacter = _selectcharacter;
    }

    public void ShowUserInfomation()
    {
        if(m_currentUserInfo != null)
        {
            m_currentUserInfoText["User_Current_Name_Text"].text = string.Format("{0}", m_name);
            m_currentUserInfoText["Cur_Default_Hp_Text"].text = string.Format("{0}", m_hp);
            m_currentUserInfoText["Cur_Default_Mp_Text"].text = string.Format("{0}", m_mp);
            m_currentUserInfoText["Cur_Default_Attack_Text"].text = string.Format("{0}", m_attack);
            m_currentUserInfoText["Cur_Default_Defence_Text"].text = string.Format("{0}", m_defence);
            m_currentUserInfoText["Cur_User_Level_Text"].text = string.Format("{0}", m_level);
            m_currentUserInfoText["Cur_User_Gold_Text"].text = string.Format("{0}", m_gold);
            m_currentUserInfoText["Cur_User_item_Text"].text = string.Format("{0}", m_item);
        }
    }
    public void ShowUserTotalStatus()
    {
        if(m_userTotalStatus != null)
        {
            m_totalSpecText["Cur_Total_Hp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("hp") + m_hp);
            m_totalSpecText["Cur_Total_Mp_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("mp") + m_mp);
            m_totalSpecText["Cur_Total_Attack_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("attack") + m_attack);
            m_totalSpecText["Cur_Total_Defence_Text"].text = string.Format("{0}", PlayerPrefs.GetInt("defence") + m_defence);
        }
    }

    public void TestBtn()
    {
        ++m_hp;
        GPGSMgr.GetInstance.hp = m_hp;
    }

    public void SaveBtn()
    {
        GPGSMgr.GetInstance.SaveGame();
    }
    public void LoadBtn()
    {
        GPGSMgr.GetInstance.LoadGame();
    }
    //public void ShowUserInfomation(string _userName, int _level, int _hp, int _mp, int _attack, int _defence, int _gold, int _item)
    //{
    //    //m_currentUserInfoText[].text = string.Format

    //}
    
}
