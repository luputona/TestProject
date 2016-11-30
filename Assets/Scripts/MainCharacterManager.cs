using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainCharacterManager : MonoBehaviour
{
    public GameObject m_mainChar;
    public Text m_mainFundsGold_Text;
    public Text m_mainFundsItem_Text;
    public SpriteRenderer m_maincharacterImage;

	// Use this for initialization
	void Start ()
    {
        m_mainChar = GameObject.FindGameObjectWithTag("MainCharacter");
        m_maincharacterImage = m_mainChar.GetComponent<SpriteRenderer>();
        ChangeMainCharacter();

    }

    void Update()
    {
        m_mainFundsGold_Text.text = string.Format("{0}", LoadData.GetInstance.m_gold);
        m_mainFundsItem_Text.text = string.Format("{0}", LoadData.GetInstance.m_item);
    }

    void ChangeMainCharacter()
    {
        //추후 서버 연결 없이 플레이 할 경우 필요
        if (LoadData.GetInstance.m_maincharacter == "UnityChan")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["01_portrait_kohaku_01"];
        }
        else if (LoadData.GetInstance.m_maincharacter == "Yuko")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["02_portrait_yuko_01"];
        }
        else if (LoadData.GetInstance.m_maincharacter == "Toko")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["03_portrait_toko_01"];
        }
        else if (LoadData.GetInstance.m_maincharacter == "Cindy")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["05_성지영"];
        }
        else if (LoadData.GetInstance.m_maincharacter == "Mariabell")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["04_portrait_marie_01"];
        }
        else if (LoadData.GetInstance.m_maincharacter == "Misaki")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["06_portrait_misaki_01"];
        }
    }
    void ChangeMainCharacterInLocal()
    {

        //추후 서버 연결 없이 플레이 할 경우 필요
        if (PlayerPrefs.GetString("SelectCharacter") == "UnityChan")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["01_portrait_kohaku_01"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["02_portrait_yuko_01"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Toko")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["03_portrait_toko_01"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Cindy")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["05_성지영"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Mariabell")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["04_portrait_marie_01"];
        }
        else if (PlayerPrefs.GetString("SelectCharacter") == "Misaki")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_image["06_portrait_misaki_01"];
        }
    }
}
