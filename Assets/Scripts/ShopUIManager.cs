using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ShopUIManager : Singleton<ShopUIManager>
{
    public GameObject m_charContent;
    public GameObject m_shopSlot;
    public GameObject m_shop_Status_text;
    public GameObject m_shop_Status_image;
    public Text m_userGoldText;
    public Text m_userItemText;
    public int m_charID;

    public Button m_buyBtn;
    public List<GameObject> m_shopSlotList = new List<GameObject>();
    public Dictionary<string, Text> m_shop_ShowStatus_text = new Dictionary<string, Text>();
    public Dictionary<string, Image> m_shop_ShowStatus_Image = new Dictionary<string, Image>();

    void Awake()
    {
        m_shop_Status_text = GameObject.FindGameObjectWithTag("Shop_Show_Status_Text");
        m_shop_Status_image = GameObject.FindGameObjectWithTag("Shop_Show_Status_Image");

    }


	void Start ()
    {
        //추후 에셋번들로 교체
        m_shopSlot = Resources.Load("Prefabs/UI/shop_list_BG") as GameObject;

        CreateCharacterList();
        InitCharacterInfoUI();
        
    }
	void Update()
    {
        m_userGoldText.text = string.Format("{0}", LoadData.GetInstance.m_initgold);
        m_userItemText.text = string.Format("{0}", LoadData.GetInstance.m_inititem);
    }

	IEnumerator ShopUpdate()
    {
        while(true)
        {
            

            yield return null;
        }
    }

    public void InitGetcompnent()
    {
        m_charContent = GameObject.FindGameObjectWithTag("ShopContent");
    }

    //상점에 캐릭터 목록을 db만큼 생성.
    void CreateCharacterList()
    {

        for(int i = 0; i< LoadCharacterData.GetInstance.m_charList.Count; i++)
        {
            GameObject shoplistslot = Instantiate(m_shopSlot) as GameObject;
            shoplistslot.transform.parent = m_charContent.transform;
            shoplistslot.transform.localPosition = Vector2.zero;
            shoplistslot.transform.localScale = Vector3.one;
            shoplistslot.transform.name = LoadCharacterData.GetInstance.m_charList[i].Name;
            m_shopSlotList.Add(shoplistslot);
        }
    }
    void InitCharacterInfoUI()
    {
        int childCount = m_shop_Status_text.transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            m_shop_ShowStatus_text.Add(m_shop_Status_text.transform.GetChild(i).name, m_shop_Status_text.transform.GetChild(i).GetComponent<Text>());
        }

        int childImageCount = m_shop_Status_image.transform.childCount;
        for(int i = 0; i< childImageCount; i++)
        {
            m_shop_ShowStatus_Image.Add(m_shop_Status_image.transform.GetChild(i).name, m_shop_Status_image.transform.GetChild(i).GetComponent<Image>());
        }


        for (int i = 0; i < LoadCharacterData.GetInstance.m_charList.Count; i++)
        {
            if (MainInvenUIManager.GetInstance.m_inventory.Contains(LoadCharacterData.GetInstance.m_charList[m_charID]))
            {
                m_buyBtn.interactable = false;
            }
            else
            {
                m_buyBtn.interactable = true;
            }
        }
    }

    public void CharacterTouchCheck(string _name)
    {
        if(_name == "UnityChan")
        {
            SetUIText(0);
        }
        else if (_name == "Yuko")
        {
            SetUIText(1);
        }
        else if (_name == "Toko")
        {
            SetUIText(2);
        }
        else if(_name == "Cindy")
        {
            SetUIText(3);
        }
        else if(_name == "Mariabell")
        {
            SetUIText(4);
        }
        else if (_name == "Misaki")
        {
            SetUIText(5);
        }
    }
    void SetUIText(int _id)
    {
        m_charID = LoadCharacterData.GetInstance.m_charList[_id].Id;
        m_shop_ShowStatus_text["Char_info_name_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Name);
        m_shop_ShowStatus_text["Char_info_hp_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Hp);
        m_shop_ShowStatus_text["Char_info_mp_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Mp);
        m_shop_ShowStatus_text["Char_info_attack_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Attack);
        m_shop_ShowStatus_text["Char_info_defence_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Defence);
        m_shop_ShowStatus_text["Char_info_sp_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Sp);
        m_shop_ShowStatus_text["Normal_Attack_Thumbnail_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].AttackName);
        m_shop_ShowStatus_text["Skill_Thumbnail_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].SkillName);
        m_shop_ShowStatus_text["Q_Thumbnail_Text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].QName);
        m_shop_ShowStatus_text["character_profile_text"].text = string.Format("{0}", LoadCharacterData.GetInstance.m_charList[_id].Profile);

        //m_shop_ShowStatus_text[""].text = string.Format("");
    }

    //이미 구매한 캐릭터인지 체크
    public void CheckBuyID()
    {
        for(int i = 0; i< LoadCharacterData.GetInstance.m_charList.Count; i++)
        {
            if(MainInvenUIManager.GetInstance.m_inventory.Contains(LoadCharacterData.GetInstance.m_charList[m_charID]))
            {
                m_buyBtn.interactable = false;
            }
            else
            {
                m_buyBtn.interactable = true;                
            }
        }
    }


    public void Buy()
    {
        for (int i = 0; i < LoadCharacterData.GetInstance.m_charList.Count; i++)
        {
            if (MainInvenUIManager.GetInstance.m_inventory.Contains(LoadCharacterData.GetInstance.m_charList[m_charID]))
            {
                m_buyBtn.interactable = false;
            }
            else
            {
                m_buyBtn.interactable = true;
                MainInvenUIManager.GetInstance.m_inventory.Add(LoadCharacterData.GetInstance.m_charList[m_charID]);
                MainInvenUIManager.GetInstance.AddInventoryCharacter(m_charID); //인벤에 추가된 수만큼 인벤슬롯 갱신.
                MainInvenUIManager.GetInstance.UpdateThumbnail();
               
            }
        }
        
    }
}
