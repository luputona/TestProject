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
    public GameObject m_statusPopUp_Obj;
    public GameObject m_nicknamePopUp_Obj;
    public Dictionary<string, Text> m_currentUserInfoText = new Dictionary<string, Text>();
    public Dictionary<string, Text> m_currentCharSpecText = new Dictionary<string, Text>();
    public Dictionary<string, Text> m_totalSpecText = new Dictionary<string, Text>();
    public Image m_charInfoImage;

    //public string m_name;
    public int m_hp;
    public int m_mp;
    public int m_attack;
    public int m_defence; 

    public int m_gold; 
    public int m_item;

    public string m_selectCharacter;
    public string m_statusPopup_info;
    public string m_nickname;
    public int m_upgradeCost = 0;
    


    SaveData savedata = new SaveData();

    public Text testtext;
    // Use this for initialization
    void Start ()
    {
        //InitailizeCharacterInfo();
        
        GPGSMgr.GetInstance.SaveGame();
        GPGSMgr.GetInstance.LoadGame();

        StartCoroutine(UserInfoUpdate());
        m_statusPopUp_Obj.SetActive(false);
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
   

    public void ShowUserInfomation()
    {
        if(m_currentUserInfo != null)
        {
            m_currentUserInfoText["User_Current_Name_Text"].text = string.Format("{0}", LoadData.GetInstance.m_username);
            m_currentUserInfoText["Cur_Default_Hp_Text"].text = string.Format("{0}", LoadData.GetInstance.m_hp);
            m_currentUserInfoText["Cur_Default_Mp_Text"].text = string.Format("{0}", LoadData.GetInstance.m_mp);
            m_currentUserInfoText["Cur_Default_Attack_Text"].text = string.Format("{0}", LoadData.GetInstance.m_attack);
            m_currentUserInfoText["Cur_Default_Defence_Text"].text = string.Format("{0}", LoadData.GetInstance.m_defence);
            //m_currentUserInfoText["Cur_User_Level_Text"].text = string.Format("{0}", m_level);
            m_currentUserInfoText["Cur_User_Gold_Text"].text = string.Format("{0}", LoadData.GetInstance.m_gold);
            m_currentUserInfoText["Cur_User_item_Text"].text = string.Format("{0}", LoadData.GetInstance.m_item);
        }
    }
    public void ShowUserTotalStatus()
    {
        m_hp = PlayerPrefs.GetInt("hp") + LoadData.GetInstance.m_hp;
        m_mp = PlayerPrefs.GetInt("mp") + LoadData.GetInstance.m_mp;
        m_attack = PlayerPrefs.GetInt("attack") + LoadData.GetInstance.m_attack;
        m_defence = PlayerPrefs.GetInt("defence") + LoadData.GetInstance.m_defence;

        if (m_userTotalStatus != null)
        {
            m_totalSpecText["Cur_Total_Hp_Text"].text = string.Format("{0}", m_hp);
            m_totalSpecText["Cur_Total_Mp_Text"].text = string.Format("{0}", m_mp);
            m_totalSpecText["Cur_Total_Attack_Text"].text = string.Format("{0}", m_attack);
            m_totalSpecText["Cur_Total_Defence_Text"].text = string.Format("{0}", m_defence);
        }
    }

    public void HpUpButton(string _statusinfo)
    {
        UpdgradeCheck(_statusinfo, LoadData.GetInstance.m_hp, LoadData.GetInstance.m_inithp , 1);
    }
    public void MpUpButton(string _statusinfo)
    {
        UpdgradeCheck(_statusinfo, LoadData.GetInstance.m_mp, LoadData.GetInstance.m_initmp , 1);    
    }
    public void AttackUpButton(string _statusinfo)
    {
        UpdgradeCheck(_statusinfo, LoadData.GetInstance.m_attack, LoadData.GetInstance.m_initattack , 5);
    }
    public void DefenceUpButton(string _statusinfo)
    {
        UpdgradeCheck(_statusinfo, LoadData.GetInstance.m_defence, LoadData.GetInstance.m_initdefence , 5);
    }

    public void StatusUpPopupOkButton()
    {
        if(m_statusPopup_info == "HP")
        {
            ++LoadData.GetInstance.m_hp;
            LoadData.GetInstance.m_gold = LoadData.GetInstance.m_gold - m_upgradeCost;
        }
        else if(m_statusPopup_info == "MP")
        {
            ++LoadData.GetInstance.m_mp;
            LoadData.GetInstance.m_gold = LoadData.GetInstance.m_gold - m_upgradeCost;
        }
        else if (m_statusPopup_info == "Attack")
        {
            ++LoadData.GetInstance.m_attack;
            LoadData.GetInstance.m_gold = LoadData.GetInstance.m_gold - m_upgradeCost;
        }
        else if (m_statusPopup_info == "Defence")
        {
            ++LoadData.GetInstance.m_defence;
            LoadData.GetInstance.m_gold = LoadData.GetInstance.m_gold - m_upgradeCost;
        }
        if(LoadData.GetInstance.m_gold < m_upgradeCost)
        {
            m_statusPopUp_Obj.transform.FindChild("Ok_Button").GetComponent<Button>().interactable = false;
        }
        else
        {
            m_statusPopUp_Obj.transform.FindChild("Ok_Button").GetComponent<Button>().interactable = true;
        }
        
        m_statusPopUp_Obj.SetActive(false);
    }

    void UpdgradeCheck(string _statusinfo, int _curstat, int _initstat, int _stat)
    {
        m_statusPopup_info = _statusinfo;
        m_statusPopUp_Obj.SetActive(true);
        m_upgradeCost = ((_curstat - _initstat) * (_curstat + _stat) + 10) * 2;
        m_statusPopUp_Obj.transform.FindChild("Current_status_Text").GetComponent<Text>().text = string.Format("{0}", _curstat);
        m_statusPopUp_Obj.transform.FindChild("Upgrade_status_Text").GetComponent<Text>().text = string.Format("{0}", _curstat + _stat);
        m_statusPopUp_Obj.transform.FindChild("Upgrade_cost_Text").GetComponent<Text>().text = string.Format("Cost : {0}", m_upgradeCost);
    }


    public void StatusUpPopupCancelButton()
    {
        m_statusPopUp_Obj.SetActive(false);
    }
    
    public void SaveBtn()
    {
        GPGSMgr.GetInstance.SaveGame();
    }
    public void LoadBtn()
    {
        GPGSMgr.GetInstance.LoadGame();
    }

    public void UserNicknameChanged(string _name)
    {
       
        if(_name == "")
        {
            m_nickname = GPGSMgr.GetInstance.GetNameGPGS();
            //PlayerPrefs.SetString("UserName", GPGSMgr.GetInstance.GetNameGPGS());
            
        }
        else
        {
            m_nickname = _name;
            //PlayerPrefs.SetString("UserName", _name);
        }
        
    }
    public void UserNicknameChangedPopUpButton()
    {
        m_nicknamePopUp_Obj.SetActive(true);
    }

    public void UserNicknameChangedButton()
    {
        LoadData.GetInstance.m_username = m_nickname;
        m_nicknamePopUp_Obj.SetActive(false);
    }
    public void UserNicknameChangedCancelButton()
    {
        m_nicknamePopUp_Obj.SetActive(false);
    }
    //public void ShowUserInfomation(string _userName, int _level, int _hp, int _mp, int _attack, int _defence, int _gold, int _item)
    //{
    //    //m_currentUserInfoText[].text = string.Format

    //}

}
