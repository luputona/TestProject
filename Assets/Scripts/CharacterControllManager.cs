using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class CharacterControllManager : Singleton<CharacterControllManager>
{
    public GameObject m_potionPanel;
    public GameObject m_skillUI;
    public GameObject m_respawnPosition;
    public GameObject m_characterObj;
    public GameObject m_gameOverPanel;
    public GameObject m_gameOver_BackGroundPanel;
    public GameObject m_settingBtn;
    public GameObject m_settingPanel;
    public Slider m_hpBar;
    public Slider m_mpBar;
    public Slider m_spBar;

    public int m_curHp;
    public float m_curMp;
    public int m_curSp;
    public int m_score;
    public int m_getGold;
    public int m_totalHp;
    public int m_totalMp;
    public int m_maxSp;

    public Text[] m_curStatusText = new Text[5];
    

    public int m_defence;
    public int m_inDamage;
    public int m_normalDamage;
    public int m_skillDamage;
    public int m_qDamage;
    public int m_recoveryMp;
    public float m_normalAttackTime;
    public float m_skillTime;
    public int m_outDamage;

    private bool m_skillbtnCheck = true;
    private bool m_normalAttackCheck = true;
    private bool m_qCheck = true;
    private bool m_hppotionCheck = true;
    private bool m_mppotionCheck = true;
    private bool m_gameoverCheck = false;

    private int m_charIndex;



    void Awake()
    {
        m_respawnPosition = this.gameObject;
        
    }
	// Use this for initialization
	void Start ()
    {
        InitializeUI();
        StartCoroutine(Init());

        //PlayerPrefs.SetString("SelectCharacter", "UnityChan");
        
        print(PlayerPrefs.GetString("SelectCharacter"));
	}
	
	// Update is called once per frame
	void Update ()
    {
        CreateMonster.GetInstance.UpdateCreatMonster();
        UpdateStatus();
    }
    IEnumerator Init()
    {
        m_gameOverPanel.SetActive(m_gameoverCheck);
        m_gameOver_BackGroundPanel.SetActive(false);
        m_settingPanel.SetActive(false);


        yield return Yielders.Get(0.5f);
        InitializeCharacter();
    }

    void InitializeUI()
    {        
        m_skillUI = GameObject.FindGameObjectWithTag("SkillPanel");
        m_potionPanel = GameObject.FindGameObjectWithTag("PotionPanel");
        
        
    }

    public void InitializeCharacter()
    {
        if (PlayerPrefs.GetString("SelectCharacter") == "UnityChan")
        {
            //나중에 에셋번들로 변경.
            m_characterObj = Resources.Load("Prefabs/Character/Unitychan_Battle(Sword)") as GameObject;
            GameObject obj = Instantiate(m_characterObj, m_respawnPosition.transform.position, Quaternion.identity) as GameObject;
            m_charIndex = 0;
            m_recoveryMp = 4;
            m_normalAttackTime = 0.4f;
            m_skillTime = 0.5f;
            m_totalHp = LoadData.GetInstance.m_hp + LoadCharacterData.GetInstance.m_charList[0].Hp;
            m_totalMp = LoadData.GetInstance.m_mp + LoadCharacterData.GetInstance.m_charList[0].Mp;
            m_normalDamage = LoadCharacterData.GetInstance.m_charList[0].Attack + LoadData.GetInstance.m_attack;
            m_skillDamage = LoadCharacterData.GetInstance.m_charList[0].SkillDamage * m_normalDamage / 4;
            m_qDamage = LoadCharacterData.GetInstance.m_charList[0].QDamage * m_normalDamage / 5;
            m_defence = LoadCharacterData.GetInstance.m_charList[0].Defence + PlayerPrefs.GetInt("defence");
            m_curStatusText[0].text = string.Format("{0} / {1}", m_totalHp, m_totalHp);
            m_curStatusText[1].text = string.Format("{0} / {1}", m_totalMp, m_totalMp);
            m_curStatusText[2].text = string.Format("{0} / {1}", LoadCharacterData.GetInstance.m_charList[0].Sp , LoadCharacterData.GetInstance.m_charList[0].Sp);
            m_curHp = m_totalHp;
            m_curMp = m_totalMp;
            m_maxSp = LoadCharacterData.GetInstance.m_charList[0].Sp;
            m_hpBar.maxValue = m_totalHp;
            m_mpBar.maxValue = m_totalMp;
            m_spBar.maxValue = LoadCharacterData.GetInstance.m_charList[0].Sp;
            m_hpBar.value = m_totalHp;
            m_mpBar.value = m_totalMp;
            m_spBar.value = 0; //LoadCharacterData.GetInstance.m_charList[0].Sp;
            
        }
        else if(PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            m_characterObj = Resources.Load("Prefabs/Character/Yuko") as GameObject;
            GameObject obj = Instantiate(m_characterObj, m_respawnPosition.transform.position, Quaternion.identity) as GameObject;
            m_charIndex = 1;
            m_recoveryMp = 5;
            m_normalAttackTime = 0.5f;
            m_skillTime = 0.3f;
            m_totalHp = LoadData.GetInstance.m_hp + LoadCharacterData.GetInstance.m_charList[1].Hp;
            m_totalMp = LoadData.GetInstance.m_mp + LoadCharacterData.GetInstance.m_charList[1].Mp;
            m_normalDamage = LoadCharacterData.GetInstance.m_charList[1].Attack + LoadData.GetInstance.m_attack;
            m_skillDamage = LoadCharacterData.GetInstance.m_charList[1].SkillDamage * m_normalDamage / 4;
            m_qDamage = LoadCharacterData.GetInstance.m_charList[1].QDamage * m_normalDamage / 5;
            m_defence = LoadCharacterData.GetInstance.m_charList[1].Defence + PlayerPrefs.GetInt("defence");
            m_curStatusText[0].text = string.Format("{0} / {1}", m_totalHp, m_totalHp);
            m_curStatusText[1].text = string.Format("{0} / {1}", m_totalMp, m_totalMp);
            m_curStatusText[2].text = string.Format("{0} / {1}", LoadCharacterData.GetInstance.m_charList[1].Sp, LoadCharacterData.GetInstance.m_charList[1].Sp);
            m_curHp = m_totalHp;
            m_curMp = m_totalMp;
            m_maxSp = LoadCharacterData.GetInstance.m_charList[1].Sp;
            m_hpBar.maxValue = m_totalHp;
            m_mpBar.maxValue = m_totalMp;
            m_spBar.maxValue = LoadCharacterData.GetInstance.m_charList[1].Sp;
            m_hpBar.value = m_totalHp;
            m_mpBar.value = m_totalMp;
            m_spBar.value = 0;// LoadCharacterData.GetInstance.m_charList[1].Sp;
        }
    }

    void UpdateStatus()
    {
        m_curHp = m_curHp - m_inDamage;
                
        m_curStatusText[0].text = string.Format("{0} / {1}", m_curHp, m_totalHp);
        if(m_curMp < m_totalMp)
        {
            m_curMp += m_recoveryMp * Time.deltaTime;
        }
        else
        {
            m_curMp += 0;
        }
        m_curStatusText[1].text = string.Format("{0} / {1}", m_curMp.ToString("N0"), m_totalMp);
        m_curStatusText[2].text = string.Format("{0} / {1}", m_curSp, LoadCharacterData.GetInstance.m_charList[m_charIndex].Sp);
        m_curStatusText[3].text = string.Format("{0}", m_getGold);
        m_curStatusText[4].text = string.Format("{0}",m_score);
        m_hpBar.value = m_curHp;
        m_mpBar.value = m_curMp;
        m_spBar.value = m_curSp;

      
        UITimer();

        if(m_curHp < 0)
        {
            m_gameoverCheck = true;
            
            Result();
            m_curHp = 0;
        }
    }

    void Result()
    {
        if(m_gameoverCheck)
        {
            m_gameOverPanel.SetActive(m_gameoverCheck);
            m_gameOver_BackGroundPanel.SetActive(true);
            m_gameOverPanel.transform.FindChild("Gold_Text").GetComponent<Text>().text = string.Format("{0}", m_getGold);
            m_gameOverPanel.transform.FindChild("Score_Text").GetComponent<Text>().text = string.Format("{0}", m_score);
        }
    }

    

    void UITimer()
    {
        // mp
        if (m_curMp >= LoadCharacterData.GetInstance.m_charList[m_charIndex].SkillMp)
        {
            if (m_skillbtnCheck)
                m_skillUI.transform.FindChild("Skill_Attack_Image_BG").GetComponent<Button>().interactable = true;
        }
        else if (m_curMp < LoadCharacterData.GetInstance.m_charList[m_charIndex].SkillMp)
        {
            m_skillUI.transform.FindChild("Skill_Attack_Image_BG").GetComponent<Button>().interactable = false;
        }

        // sp 
        if(m_curSp >= LoadCharacterData.GetInstance.m_charList[m_charIndex].QSp)
        {
            if(m_skillbtnCheck)
                m_skillUI.transform.FindChild("Q_Attack_Image_BG").GetComponent<Button>().interactable = true;
        }
        else if(m_curSp < LoadCharacterData.GetInstance.m_charList[m_charIndex].QSp)
        {
            m_skillUI.transform.FindChild("Q_Attack_Image_BG").GetComponent<Button>().interactable = false;
        }

        // 일반공격
        if (m_normalAttackCheck)
        {
            m_skillUI.transform.FindChild("Normal_Attack_Image_BG").GetComponent<Button>().interactable = true;
        }
       

        if (m_mppotionCheck)
        {
            m_potionPanel.transform.FindChild("MP_Potion_Image").GetComponent<Button>().interactable = true;
        }
        if(m_hppotionCheck)
        {
            m_potionPanel.transform.FindChild("HP_Potion_Image").GetComponent<Button>().interactable = true;
        }

    }
    public void SettingOn()
    {
        m_settingPanel.SetActive(true);
        m_gameOver_BackGroundPanel.SetActive(true);

        Time.timeScale = 0;
    }
    
    public void SettingOff()
    {
        m_settingPanel.SetActive(false);
        m_gameOver_BackGroundPanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void NormalAttack()
    {
        m_normalAttackCheck = false;
        BattleSpriteAction.GetInstance.NormalAttack();

        m_skillUI.transform.FindChild("Normal_Attack_Image_BG").GetComponent<Button>().interactable = false;

        m_outDamage = m_normalDamage;
        StartCoroutine(NormalAttackTimer());
        
    }
    public void SkillAttack()
    {
        m_skillbtnCheck = false;
        BattleSpriteAction.GetInstance.SkillAttack();

        m_curMp = m_curMp - LoadCharacterData.GetInstance.m_charList[m_charIndex].SkillMp;
        m_skillUI.transform.FindChild("Skill_Attack_Image_BG").GetComponent<Button>().interactable = false;

        m_outDamage = m_skillDamage;
        StartCoroutine(SkillButtonTimer());
        
    }

    public void QAttack()
    {
        m_qCheck = false;
        BattleSpriteAction.GetInstance.QAttack();

        m_curSp = m_curSp - LoadCharacterData.GetInstance.m_charList[m_charIndex].QSp;
        m_skillUI.transform.FindChild("Q_Attack_Image_BG").GetComponent<Button>().interactable = false;

        m_outDamage = m_qDamage;
        StartCoroutine(QTimer());
    }

    public void UseHpPotion()
    {
        m_hppotionCheck = false;
        m_potionPanel.transform.FindChild("HP_Potion_Image").GetComponent<Button>().interactable = false;
        StartCoroutine(HpPotionButtonTimer());
    }

    public void UseMpPotion()
    {
        m_mppotionCheck = false;
        m_potionPanel.transform.FindChild("MP_Potion_Image").GetComponent<Button>().interactable = false;
        StartCoroutine(MpPotionButtonTimer());
    }
    IEnumerator NormalAttackTimer()
    {
        yield return Yielders.Get(m_normalAttackTime);
        m_normalAttackCheck = true;
        BattleSpriteAction.GetInstance.m_emotion = MOTIONCHECK.E_IDLE;
    }

    IEnumerator SkillButtonTimer()
    {
        yield return Yielders.Get(m_skillTime);
        m_skillbtnCheck = true;
        BattleSpriteAction.GetInstance.m_emotion = MOTIONCHECK.E_IDLE;
    }
    IEnumerator QTimer()
    {
        yield return Yielders.Get(1.0f);
        m_qCheck = true;
    }
    IEnumerator HpPotionButtonTimer()
    {
        yield return Yielders.Get(0.5f);
        m_hppotionCheck = true;       
    }
    IEnumerator MpPotionButtonTimer()
    {
        yield return Yielders.Get(0.5f);        
        m_mppotionCheck = true;
    }
}
