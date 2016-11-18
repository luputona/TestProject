using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image m_slotBG;
    public GameObject[] m_slotChild;
    public Text m_charNameText;
    public Text m_costText;
    public Button m_btn;

	// Use this for initialization
	void Start ()
    {
        m_slotBG = this.GetComponent<Image>();
        int slotchildcount = m_slotBG.transform.childCount;
        m_slotChild = new GameObject[slotchildcount];
        for(int i = 0; i< slotchildcount; i++)
        {
            m_slotChild[i] = m_slotBG.transform.GetChild(i).gameObject;
        }

        m_charNameText = m_slotChild[1].GetComponent<Text>();
        m_costText = m_slotChild[2].GetComponent<Text>();
        m_btn = m_slotChild[4].GetComponent<Button>();
        m_btn.name = m_slotBG.name;
        m_slotChild[0].transform.name = m_slotBG.name;

        m_charNameText.text = string.Format("{0}", this.gameObject.name);

        for(int i = 0; i< LoadCharacterData.GetInstance.m_charList.Count; i++)
        {
            if(LoadCharacterData.GetInstance.m_charList[i].Name == this.gameObject.name)
            {
                m_costText.text = string.Format("{0}",LoadCharacterData.GetInstance.m_charList[i].Cost);
            }
        }
        m_btn.onClick.AddListener(() =>
        {
            SelectCharacterEvent();
            
        }
        );        

	}
    void SelectCharacterEvent()
    {
        ShopUIManager.GetInstance.CharacterTouchCheck(m_btn.transform.name);
        ShopUIManager.GetInstance.CheckBuyID();
        print(m_btn.name);
    }


}
