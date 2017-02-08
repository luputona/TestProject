using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadingUIManager : MonoBehaviour
{
    public Text m_progressText;
    public Button m_btn;

    public float m_currentValue;

	// Use this for initialization
	void Start ()
    {
        m_progressText = this.gameObject.transform.FindChild("Progress_Text").gameObject.GetComponent<Text>();
        m_btn = this.gameObject.GetComponent<Button>();
        m_btn.interactable = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_currentValue = LoadData.GetInstance.m_currentValue;
        if(m_progressText !=null)
        {
            m_progressText.text = string.Format("{0:0}%", m_currentValue);
        }
        if(m_currentValue < 100)
        {
            m_btn.interactable = false;
        }
        else if(m_currentValue >=100)
        {
            m_btn.interactable = true;
        }


    }
}
