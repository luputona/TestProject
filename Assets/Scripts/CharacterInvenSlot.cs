using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterInvenSlot : MonoBehaviour
{
    public Image m_charlistBG;
    public GameObject[] m_charlistChild;
    public Text m_charNameText;
    public Button m_btn;

    //public Image m_charThumbnail;
 
	// Use this for initialization
	void Start ()
    {
        m_charlistBG = this.GetComponent<Image>();
        int listBGchild = m_charlistBG.transform.childCount;
        m_charlistChild = new GameObject[listBGchild];
        for(int i = 0; i < listBGchild; i++)
        {
            m_charlistChild[i] = m_charlistBG.transform.GetChild(i).gameObject;
        }

        //m_charThumbnail = m_charlistChild[0].GetComponent<Image>();
        //m_charThumbnail.sp = Resources.Load<Sprite>("image/illust/portrait_kohaku_01");

        m_charNameText = m_charlistChild[1].GetComponent<Text>();
        m_charNameText.text = string.Format("{0}", this.gameObject.name);
        m_btn = m_charlistChild[2].GetComponent<Button>();
        m_btn.onClick.AddListener(() =>
       {
           SelectCharacterEvent();
       });
    }
	
    void SelectCharacterEvent()
    {
        MainInvenUIManager.GetInstance.CharacterTouchCheck(m_btn.transform.name);
    }
}
