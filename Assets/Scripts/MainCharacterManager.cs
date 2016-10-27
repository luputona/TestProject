using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainCharacterManager : MonoBehaviour
{
    public GameObject m_mainChar;
    public SpriteRenderer m_maincharacterImage;

	// Use this for initialization
	void Start ()
    {
        m_mainChar = GameObject.FindGameObjectWithTag("MainCharacter");
        m_maincharacterImage = m_mainChar.GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
       
    }

    void ChangeMainCharacter()
    {
        if (PlayerPrefs.GetString("SelectCharacter") == "UnityChan")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_thumbnailSprite[0].sprite;
        }
        if (PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            m_maincharacterImage.sprite = MainInvenUIManager.GetInstance.m_thumbnailSprite[1].sprite;
        }
    }
}
