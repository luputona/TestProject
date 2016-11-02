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
    public Slider m_hpBar;
    public Slider m_mpBar;
    public Slider m_spBar;

    public int m_curHp;
    public float m_curMp;
    public int m_curSp;

    public int m_totalHp;
    public int m_totalMp;

    public Text[] m_curStatusText = new Text[3];

    public int m_inDamage;
    public int m_normalDamage;
    public int m_skillDamage;
    public int m_qDamage;
    public int m_recoveryMp;
    public float m_normalAttackTime;
    public float m_skillTime;

    private bool m_skillbtnCheck = true;
    private bool m_normalAttackCheck = true;
    private bool m_qCheck = true;
    private bool m_hppotionCheck = true;
    private bool m_mppotionCheck = true;

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
        
        print(PlayerPrefs.GetString("SelectCharacter"));
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateStatus();
    }
    IEnumerator Init()
    {
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
            m_recoveryMp = 10;
            m_normalAttackTime = 0.4f;
            m_skillTime = 0.8f;
            m_totalHp = UserInfomation.GetInstance.m_hp + LoadCharacterData.GetInstance.m_charList[0].Hp;
            m_totalMp = UserInfomation.GetInstance.m_mp + LoadCharacterData.GetInstance.m_charList[0].Mp;
            m_normalDamage = LoadCharacterData.GetInstance.m_charList[0].Attack;
            m_skillDamage = LoadCharacterData.GetInstance.m_charList[0].SkillDamage;
            m_qDamage = LoadCharacterData.GetInstance.m_charList[0].QDamage;
            m_curStatusText[0].text = string.Format("{0} / {1}", m_totalHp, m_totalHp);
            m_curStatusText[1].text = string.Format("{0} / {1}", m_totalMp, m_totalMp);
            m_curStatusText[2].text = string.Format("{0} / {1}", LoadCharacterData.GetInstance.m_charList[0].Sp , LoadCharacterData.GetInstance.m_charList[0].Sp);
            m_curHp = m_totalHp;
            m_curMp = m_totalMp;
            m_curSp = LoadCharacterData.GetInstance.m_charList[0].Sp;
            m_hpBar.maxValue = m_totalHp;
            m_mpBar.maxValue = m_totalMp;
            m_spBar.maxValue = LoadCharacterData.GetInstance.m_charList[0].Sp;
            m_hpBar.value = m_totalHp;
            m_mpBar.value = m_totalMp;
            m_spBar.value = LoadCharacterData.GetInstance.m_charList[0].Sp;
            
        }
        else if(PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {

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
        m_hpBar.value = m_curHp;
        m_mpBar.value = m_curMp;
        m_spBar.value = m_curSp;
        

        UITimer();

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

    public void NormalAttack()
    {
        m_normalAttackCheck = false;
        BattleSpriteAction.GetInstance.NormalAttack();

        m_skillUI.transform.FindChild("Normal_Attack_Image_BG").GetComponent<Button>().interactable = false;
        
        StartCoroutine(NormalAttackTimer());
    }
    public void SkillAttack()
    {
        m_skillbtnCheck = false;
        BattleSpriteAction.GetInstance.SkillAttack();

        m_curMp = m_curMp - LoadCharacterData.GetInstance.m_charList[m_charIndex].SkillMp;
        m_skillUI.transform.FindChild("Skill_Attack_Image_BG").GetComponent<Button>().interactable = false;
       
        StartCoroutine(SkillButtonTimer());
    }

    public void QAttack()
    {
        m_qCheck = false;
        BattleSpriteAction.GetInstance.QAttack();

        m_curSp = m_curSp - LoadCharacterData.GetInstance.m_charList[m_charIndex].QSp;
        m_skillUI.transform.FindChild("Q_Attack_Image_BG").GetComponent<Button>().interactable = false;
       
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
    }

    IEnumerator SkillButtonTimer()
    {
        yield return Yielders.Get(m_skillTime);
        m_skillbtnCheck = true;
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
