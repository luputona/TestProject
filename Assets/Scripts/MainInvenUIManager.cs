using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainInvenUIManager : Singleton<MainInvenUIManager>
{
    public GameObject m_mainChar;
    public SpriteRenderer m_maincharacterImage;

    public List<CharacterData> m_inventory = new List<CharacterData>();

    public int m_dynamicPanel_count;
    public GameObject m_charslot;
    public GameObject m_charContent;
    public GameObject[] m_InvenStatusTextArray;
    public GameObject[] m_InvenStatusImageArray;
    public string m_childBtnName;

    public Dictionary<string, Text> m_invenStatusText = new Dictionary<string, Text>();
    public Dictionary<string, Image> m_invenStatusImage = new Dictionary<string, Image>();
    public Dictionary<string, Sprite> m_image = new Dictionary<string, Sprite>();
    public Dictionary<string, Image> m_slotDic = new Dictionary<string, Image>();
   // public List<Sprite> m_sprite = new List<Sprite>();

    public List<Sprite> m_sprite = new List<Sprite>();
    public List<GameObject> m_slotList = new List<GameObject>();
    
    public Image[] m_thumbnailSprite;



    private LoadCharacterData m_charData;
    private int hp;
    private int mp;
    private int attack;
    private int defence;
    private string attackName;
    private string skillName;
    private string qName;
    private int index;

    private Sprite[] m_spriteobj;

    
	// Use this for initialization
	void Start ()
    {
        //인벤토리 UI리스트 프리팹 로드, 추후 에셋번들로 교체
        m_charslot = Resources.Load("Prefabs/UI/char_list_BG") as GameObject;
        m_charData = LoadCharacterData.GetInstance.GetComponent<LoadCharacterData>();
        //m_select_charstatus = GameObject.Find("Select_Character_Status_BG");

        m_mainChar = GameObject.FindGameObjectWithTag("MainCharacter");
        m_maincharacterImage = m_mainChar.GetComponent<SpriteRenderer>();
        CreateCharList();
        UserInfomation.GetInstance.InitailizeCharacterInfo();

        for (int i = 0; i < m_inventory.Count; i++)
        {
            GameObject charslot = Instantiate(m_charslot) as GameObject;
            charslot.transform.parent = m_charContent.transform;
            charslot.transform.localPosition = Vector2.zero;
            charslot.transform.localScale = Vector3.one;
            charslot.transform.name = m_inventory[i].Name;
            for(int j = 0; j < charslot.transform.childCount; j++)
            {
                charslot.transform.GetChild(j).transform.name = m_inventory[i].Name;
            }
            m_slotList.Add(charslot);
            m_slotDic.Add(charslot.transform.name, charslot.transform.GetChild(0).GetComponent<Image>());

            UpdateThumbnail();
            
        }

        
        //StartCoroutine(MainUpdate());
    }

    IEnumerator MainUpdate()
    {
        while (true)
        {
            
            yield return null;
        }
        
    }

  
    //인벤토리에 구매한 캐릭터의 슬롯을 추가
    //해야 할것 : 인벤토리.add 된 목록을 서버로 전송 - 
    public void AddInventoryCharacter(int _id)
    {
        GameObject charslot = Instantiate(m_charslot) as GameObject;
        charslot.transform.parent = m_charContent.transform;
        charslot.transform.localPosition = Vector2.zero;
        charslot.transform.localScale = Vector3.one;
        

        for (int i = 0; i < m_inventory.Count; i++)
        {            
            charslot.transform.name = m_inventory[i].Name;
        }
        m_slotDic.Add(charslot.transform.name, charslot.transform.GetChild(0).GetComponent<Image>());
        m_slotList.Add(charslot);
        
    }
    
    public void CreateCharList()
    {
        //for (int i = 0; i < m_charData.m_charList.Count; i++)
        //{
        //    //현재 가지고있는 캐릭터의 인벤토리
        //    m_inventory.Add(m_charData.m_charList[i]);
        //}

        //서버에 json 형태로 저장한걸 불러와서 파싱한후 
        //인벤토리add에 m_charData.m_charList[0] 대신 파싱한 리스트를 넣는다.
        //기본으로 가지고있는 캐릭터를 하나 추가
        if(LoadData.GetInstance.m_localcharList.Count == 0)
        {
            //m_inventory.Add(LoadCharacterData.GetInstance.m_charList[0]);
        }
        else
        {
            for (int i = 0; i < LoadData.GetInstance.m_localcharList.Count; i++)
            {
                m_inventory.Add(LoadData.GetInstance.m_localcharList[i]);
            }
            //ShopUIManager.GetInstance.m_character[0] = new CharacterData(MainInvenUIManager.GetInstance.m_inventory[0].Name, MainInvenUIManager.GetInstance.m_inventory[0].Id, MainInvenUIManager.GetInstance.m_inventory[0].Cost, MainInvenUIManager.GetInstance.m_inventory[0].Hp, MainInvenUIManager.GetInstance.m_inventory[0].Mp, MainInvenUIManager.GetInstance.m_inventory[0].Sp, MainInvenUIManager.GetInstance.m_inventory[0].SkillName, MainInvenUIManager.GetInstance.m_inventory[0].SkillMp, MainInvenUIManager.GetInstance.m_inventory[0].SkillDamage, MainInvenUIManager.GetInstance.m_inventory[0].Attack, MainInvenUIManager.GetInstance.m_inventory[0].AttackName, MainInvenUIManager.GetInstance.m_inventory[0].Defence, MainInvenUIManager.GetInstance.m_inventory[0].QName, MainInvenUIManager.GetInstance.m_inventory[0].QSp, MainInvenUIManager.GetInstance.m_inventory[0].QDamage, MainInvenUIManager.GetInstance.m_inventory[0].Profile);
        }
                        
        InitThumbnailSprite();
        //인벤토리 UI리스트 클론 생성, 인벤토리 리스트에 담긴 수 만큼 생성
        
    }
    public void InitInvenPanel()
    {
        int textcount = GameObject.FindGameObjectsWithTag("InvenStatusText").Length;
        int imagecount = GameObject.FindGameObjectsWithTag("InvenStatusImage").Length;
        m_InvenStatusTextArray = new GameObject[textcount];
        m_InvenStatusImageArray = new GameObject[imagecount];
        m_InvenStatusTextArray = GameObject.FindGameObjectsWithTag("InvenStatusText");
        m_InvenStatusImageArray = GameObject.FindGameObjectsWithTag("InvenStatusImage");
        
        for (int i =0; i < textcount; i++)
        {
            m_invenStatusText.Add(m_InvenStatusTextArray[i].name , m_InvenStatusTextArray[i].GetComponent<Text>());
        }
        for(int i = 0; i< imagecount; i++)
        {
            m_invenStatusImage.Add(m_InvenStatusImageArray[i].name, m_InvenStatusImageArray[i].GetComponent<Image>());
        }
    }

    public void UpdateThumbnail()
    {
        for(int i = 0; i < m_slotList.Count; i++)
        {
            if(m_slotList[i].name == "UnityChan")
            {
                m_slotDic["UnityChan"].sprite = m_image["01_portrait_kohaku_01"];
            }
            else if (m_slotList[i].name == "Yuko")
            {
                m_slotDic["Yuko"].sprite = m_image["02_portrait_yuko_01"];
            }
            else if(m_slotList[i].name == "Toko")
            {
                m_slotDic["Toko"].sprite = m_image["03_portrait_toko_01"];
            }
            else if(m_slotList[i].name == "Cindy")
            {
                m_slotDic["Cindy"].sprite = m_image["05_성지영"];
            }
            else if(m_slotList[i].name == "Mariabell")
            {                
                m_slotDic["Mariabell"].sprite = m_image["04_portrait_marie_01"];
            }
            else if(m_slotList[i].name == "Misaki")
            {
                m_slotDic["Misaki"].sprite = m_image["06_portrait_misaki_01"];
            }
        }
    }

    public void InitThumbnailSprite()
    {
        //추후 에셋번들로 교체
        m_spriteobj = Resources.LoadAll<Sprite>("image/illust/");

       
        for(int i = 0; i<m_spriteobj.Length; i++)
        {
            m_image.Add(m_spriteobj[i].name, m_spriteobj[i]);
            
        }
        for(int i = 0; i<m_spriteobj.Length; i++)
        {
            m_sprite.Add(m_spriteobj[i]);
        }

        

        //UserInfomation.GetInstance.ShowCharacterSpec(PlayerPrefs.GetInt("hp"), PlayerPrefs.GetInt("mp"), PlayerPrefs.GetInt("attack"), PlayerPrefs.GetInt("defence"), PlayerPrefs.GetString("SelectCharacter"), m_slotDic[PlayerPrefs.GetString("SelectCharacter")].sprite);
        //m_maincharacterImage.sprite = m_slotDic[PlayerPrefs.GetString("SelectCharacter")].sprite;
    }

    public void CharacterTouchCheck(string _charname)
    {
        m_childBtnName = _charname;
        if (_charname == "UnityChan")
        {
            SetCharacterStatus(0);
        }
        else if (_charname == "Yuko")
        {
            SetCharacterStatus(1);
        }
        else if (_charname == "Toko")
        {
            SetCharacterStatus(2);
        }
        else if (_charname == "Cindy")
        {
            SetCharacterStatus(3);
        }
        else if (_charname == "Mariabell")
        {
            SetCharacterStatus(4);
        }
        else if (_charname == "Misaki")
        {
            SetCharacterStatus(5);
        }
        

        // 캐릭 저장루틴 나중에 서버연동으로 변경
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("mp", mp);
        PlayerPrefs.SetInt("attack", attack);
        PlayerPrefs.SetInt("defence", defence);
        
    }
    void SetCharacterStatus(int _id)
    {
        attack = LoadCharacterData.GetInstance.m_charList[_id].Attack;
        defence = LoadCharacterData.GetInstance.m_charList[_id].Defence;
        hp = LoadCharacterData.GetInstance.m_charList[_id].Hp;
        mp = LoadCharacterData.GetInstance.m_charList[_id].Mp;
        attackName = LoadCharacterData.GetInstance.m_charList[_id].AttackName;
        skillName = LoadCharacterData.GetInstance.m_charList[_id].SkillName;
        qName = LoadCharacterData.GetInstance.m_charList[_id].QName;
        index = _id;

        m_invenStatusText["Attack_Status_Text"].text = string.Format("{0}", attack);
        m_invenStatusText["Defence_Status_Text"].text = string.Format("{0}", defence);
        m_invenStatusText["Hp_Status_Text"].text = string.Format("{0}", hp);
        m_invenStatusText["Mp_Status_Text"].text = string.Format("{0}", mp);
        m_invenStatusText["Normal_Attack_Text"].text = string.Format("{0}", attackName);
        m_invenStatusText["Skill_Text"].text = string.Format("{0}", skillName);
        m_invenStatusText["Q_Text"].text = string.Format("{0}", qName);

        UserInfomation.GetInstance.ShowCharacterSpec(hp, mp, attack, defence, m_childBtnName, m_slotDic[m_childBtnName].sprite);
    }

    void ShowCharacterStatus(int _attack, int _defence, int _hp, int _mp, string _attackname, string _skillname, string _qname)
    {
        m_invenStatusText["Attack_Status_Text"].text = string.Format("{0}", _attack);
        m_invenStatusText["Defence_Status_Text"].text = string.Format("{0}", _defence);
        m_invenStatusText["Hp_Status_Text"].text = string.Format("{0}", _hp);
        m_invenStatusText["Mp_Status_Text"].text = string.Format("{0}", _mp);
        m_invenStatusText["Normal_Attack_Text"].text = string.Format("{0}", _attackname);
        m_invenStatusText["Skill_Text"].text = string.Format("{0}", _skillname);
        m_invenStatusText["Q_Text"].text = string.Format("{0}", _qname);
        
    }


    public void SelectCharacterButton(string _charname)
    {
        _charname = m_childBtnName;
        LoadData.GetInstance.m_maincharacter = _charname;
        PlayerPrefs.SetString("SelectCharacter", _charname); //선택한 캐릭명, 추후 구글 서버쪽으로 저장되게 변경
        
        if (m_childBtnName == "UnityChan")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }
        else if (m_childBtnName == "Yuko")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }
        else if (m_childBtnName == "Toko")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }
        else if (m_childBtnName == "Cindy")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }
        else if (m_childBtnName == "Mariabell")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }
        else if (m_childBtnName == "Misaki")
        {
            m_maincharacterImage.sprite = m_slotDic[m_childBtnName].sprite;
        }

        LoadData.GetInstance.UploadAllData();
    }
    

}
