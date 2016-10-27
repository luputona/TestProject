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


    void Awake()
    {
        InitInvenPanel();
    }
	// Use this for initialization
	void Start ()
    {
        //인벤토리 UI리스트 프리팹 로드, 추후 에셋번들로 교체
        m_charslot = Resources.Load("Prefabs/UI/char_list_BG") as GameObject;
        m_charData = LoadCharacterData.GetInstance.GetComponent<LoadCharacterData>();
        //m_select_charstatus = GameObject.Find("Select_Character_Status_BG");


        CreateCharList();

        m_mainChar = GameObject.FindGameObjectWithTag("MainCharacter");
        m_maincharacterImage = m_mainChar.GetComponent<SpriteRenderer>();



        StartCoroutine(MainUpdate());
    }

    IEnumerator MainUpdate()
    {
        while (true)
        {

            yield return null;
        }
        
    }

    
    void CreateCharList()
    {
        for (int i = 0; i < m_charData.m_charList.Count; i++)
        {
            //현재 가지고있는 캐릭터의 인벤토리
            m_inventory.Add(m_charData.m_charList[i]);
        }
       
        //인벤토리 UI리스트 클론 생성, 인벤토리 리스트에 담긴 수 만큼 생성
        for (int i = 0; i < m_inventory.Count; i++)
        {
            GameObject charslot = Instantiate(m_charslot) as GameObject;
            charslot.transform.parent = m_charContent.transform;
            charslot.transform.localPosition = Vector2.zero;
            charslot.transform.localScale = Vector3.one;
            charslot.transform.name = m_inventory[i].Name;
            m_slotList.Add(charslot);
        }
        InitThumbnailSprite();
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
    public void InitThumbnailSprite()
    {
        //GameObject childBtnName = _slot.transform.GetChild(2).gameObject;
        m_slotList[0].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("image/illust/portrait_kohaku_01");
        m_slotList[1].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("image/illust/portrait_Yuko_01");

        for(int i = 0; i<m_slotList.Count; i++)
        {
            m_slotList[i].transform.GetChild(2).name = m_slotList[i].name;
        }      
    }

    public void CharacterTouchCheck(string _charname)
    {
        //int hp;
        //int mp;
        //int attack;
        //int defence;
        //string attackName;
        //string skillName;
        //string qName;
        
        if (_charname == "UnityChan")
        {
            attack = m_charData.m_charList[0].Attack;
            defence = m_charData.m_charList[0].Defence;
            hp = m_charData.m_charList[0].Hp;
            mp = m_charData.m_charList[0].Mp;
            attackName = m_charData.m_charList[0].AttackName;
            skillName = m_charData.m_charList[0].SkillName;
            qName = m_charData.m_charList[0].QName;
            index = 0;
        }
        if (_charname == "Yuko")
        {
            attack = m_charData.m_charList[1].Attack;
            defence = m_charData.m_charList[1].Defence;
            hp = m_charData.m_charList[1].Hp;
            mp = m_charData.m_charList[1].Mp;
            attackName = m_charData.m_charList[1].AttackName;
            skillName = m_charData.m_charList[1].SkillName;
            qName = m_charData.m_charList[1].QName;
            index = 1;
        }
        m_childBtnName = _charname;
        ShowCharacterStatus(attack, defence, hp, mp, attackName, skillName, qName, index);


        //저장루틴 나중에 서버연동으로 변경
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("mp", mp);
        PlayerPrefs.SetInt("attack", attack);
        PlayerPrefs.SetInt("defence", defence);
        
    }
    void ShowCharacterStatus(int _attack, int _defence, int _hp, int _mp, string _attackname, string _skillname, string _qname, int _index)
    {
        m_invenStatusText["Attack_Status_Text"].text = string.Format("{0}", _attack);
        m_invenStatusText["Defence_Status_Text"].text = string.Format("{0}", _defence);
        m_invenStatusText["Hp_Status_Text"].text = string.Format("{0}", _hp);
        m_invenStatusText["Mp_Status_Text"].text = string.Format("{0}", _mp);
        m_invenStatusText["Normal_Attack_Text"].text = string.Format("{0}", _attackname);
        m_invenStatusText["Skill_Text"].text = string.Format("{0}", _skillname);
        m_invenStatusText["Q_Text"].text = string.Format("{0}", _qname);

        UserInfomation.GetInstance.ShowCharacterSpec(_hp,_mp,_attack,_defence, PlayerPrefs.GetString("SelectCharacter"), m_slotList[_index].transform.GetChild(0).GetComponent<Image>());
    }
    public void SelectCharacterButton(string _charname)
    {
        _charname = m_childBtnName;
        PlayerPrefs.SetString("SelectCharacter", _charname); // 추후 구글 서버쪽으로 저장되게 변경
        


        if (PlayerPrefs.GetString("SelectCharacter") == "UnityChan")
        {
            m_maincharacterImage.sprite = m_slotList[0].transform.GetChild(0).GetComponent<Image>().sprite;
        }
        if (PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            m_maincharacterImage.sprite = m_slotList[1].transform.GetChild(0).GetComponent<Image>().sprite;
        }

        print(PlayerPrefs.GetString("SelectCharacter"));
    }
    

}
