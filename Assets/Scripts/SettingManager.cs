using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingManager : MonoBehaviour
{
    public GameObject m_settingPanel;
    public GameObject m_cautionBG;
    public GameObject m_restartAppMsg;
    public Button m_signOutBtn;

	// Use this for initialization
	void Start ()
    {
        m_settingPanel = GameObject.FindGameObjectWithTag("SettingPanel").gameObject;
        m_cautionBG = m_settingPanel.transform.FindChild("BG").gameObject.transform.FindChild("Caution_BG").gameObject;
        m_signOutBtn = m_settingPanel.transform.FindChild("BG").gameObject.transform.FindChild("SignOut_Button").gameObject.GetComponent<Button>();
        m_restartAppMsg = m_settingPanel.transform.FindChild("RestartApp_BG").gameObject;

        m_restartAppMsg.SetActive(false);
        m_cautionBG.SetActive(false);
        m_settingPanel.SetActive(false);
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OpenedSetting()
    {
        m_settingPanel.SetActive(true);
    }

    public void ClosedSetting()
    {
        m_settingPanel.SetActive(false);
    }
   
    public void OpenedSignOutCautionMsg()
    {
        m_cautionBG.SetActive(true);
        m_signOutBtn.interactable = false;
    }

    public void ClosedSignOutCautionMsg()
    {
        m_cautionBG.SetActive(false);
        m_signOutBtn.interactable = true;
    }

    public void SignoutOkButton()
    {
        if (PlayerPrefs.HasKey("UserIdCode"))
        {
            PlayerPrefs.DeleteKey("UserIdCode");           
        }
        m_restartAppMsg.SetActive(true);
        m_cautionBG.SetActive(false);
    }
}
