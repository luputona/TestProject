using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainUIAnimController : MonoBehaviour
{
    public enum ESHOP_UI_LIST
    {
        E_CHARTAP,
        E_VEHICLETAP,
        E_ETCTAP,
        E_ETCPANEL,
        E_VEHICLEPANEL,
        E_CHARPANEL
    }

    
    public GameObject[] m_dynamicUI_Panel;
    public GameObject[] m_shopUITap;
    public GameObject[] m_userInfoPanel_GetChild;

    public bool m_uiActiveCheck = false;

    private Animator m_userInfoAnim;
    private Animator m_shopUiAnim;
    private Animator m_mapUiAnim;
    private Animator m_invenAnim;

    public Dictionary<string, Animator> m_dicUiAnim;
    public Animator[] m_uiAnim;


    public Button[] m_mainUIBtn;
    public GameObject[] m_mainUIBtnobj;

    void Awake()
    {

    }

	// Use this for initialization
	void Start ()
    {
        InitializeUI();

    }
	
	// Update is called once per frame
	void Update ()
    {
        InteractableCheck();
    }
    

    public void GoToMain(string uiparam)
    {
        if( uiparam == "UserInfo")
        {
            m_uiActiveCheck = false;
            m_dicUiAnim["UserInfo_panel"].SetBool("EnableUserInfo", false);
        }
        if( uiparam == "ShopUI")
        {
            m_uiActiveCheck = false;
            m_dicUiAnim["Shop_Panel"].SetBool("EnableShopUI", false);
        }
        if( uiparam == "MapUI")
        {
            m_uiActiveCheck = false;
            m_dicUiAnim["Select_MapPanel"].SetBool("EnableUI", false);
        }
        if(uiparam == "Inven")
        {
            m_uiActiveCheck = false;
            m_dicUiAnim["Character_Inven_Panel"].SetBool("EnableUI", false);
        }
    }
    
    public void UserInfoUI()
    {
        m_uiActiveCheck = true;
        m_dicUiAnim["UserInfo_panel"].gameObject.SetActive(m_uiActiveCheck);
        m_dicUiAnim["UserInfo_panel"].SetBool("EnableUserInfo", m_uiActiveCheck);
    }

    public void ShopUI()
    {
        m_uiActiveCheck = true;
        m_dicUiAnim["Shop_Panel"].gameObject.SetActive(m_uiActiveCheck);
        m_dicUiAnim["Shop_Panel"].SetBool("EnableShopUI", m_uiActiveCheck);
    }

    public void MapUI()
    {
        m_uiActiveCheck = true;
        m_dicUiAnim["Select_MapPanel"].gameObject.SetActive(m_uiActiveCheck);
        m_dicUiAnim["Select_MapPanel"].SetBool("EnableUI", m_uiActiveCheck);
    }

    
    public void Inventory()
    {
        m_uiActiveCheck = true;
        m_dicUiAnim["Character_Inven_Panel"].gameObject.SetActive(m_uiActiveCheck);
        m_dicUiAnim["Character_Inven_Panel"].SetBool("EnableUI", m_uiActiveCheck);        
    }
    public void SelectMapUI()
    {

    }


    public void ShopUITapCharacter()
    {
        m_shopUITap[(int)ESHOP_UI_LIST.E_CHARPANEL].SetActive(true);
        m_shopUITap[(int)ESHOP_UI_LIST.E_ETCPANEL].SetActive(false);
        m_shopUITap[(int)ESHOP_UI_LIST.E_VEHICLEPANEL].SetActive(false);
    }
    public void ShopUITapVehicle()
    {
        m_shopUITap[(int)ESHOP_UI_LIST.E_VEHICLEPANEL].SetActive(true);
        m_shopUITap[(int)ESHOP_UI_LIST.E_CHARPANEL].SetActive(false);
        m_shopUITap[(int)ESHOP_UI_LIST.E_ETCPANEL].SetActive(false);
        
    }

    public void ShopUITapETC()
    {
        m_shopUITap[(int)ESHOP_UI_LIST.E_ETCPANEL].SetActive(true);
        m_shopUITap[(int)ESHOP_UI_LIST.E_CHARPANEL].SetActive(false);        
        m_shopUITap[(int)ESHOP_UI_LIST.E_VEHICLEPANEL].SetActive(false);
    }

    void InitializeUI()
    {
        int panelcount = GameObject.FindGameObjectsWithTag("DynamicUI_Panel").Length;
        m_dynamicUI_Panel = new GameObject[panelcount];
        m_dicUiAnim = new Dictionary<string, Animator>();
        m_dynamicUI_Panel = GameObject.FindGameObjectsWithTag("DynamicUI_Panel");

        for (int i = 0; i < panelcount; i++)
        {
            m_dicUiAnim.Add(m_dynamicUI_Panel[i].name, m_dynamicUI_Panel[i].GetComponent<Animator>());
        }

        int tapButton = m_dicUiAnim["Shop_Panel"].transform.childCount;
        m_shopUITap = new GameObject[tapButton];
        for (int i = 0; i < tapButton; i++)
        {
            m_shopUITap[i] = m_dicUiAnim["Shop_Panel"].transform.GetChild(i).gameObject;
        }

        int userInfopanelChildCount = m_dicUiAnim["UserInfo_panel"].transform.childCount;
        m_userInfoPanel_GetChild = new GameObject[userInfopanelChildCount];
        for (int i = 0; i < userInfopanelChildCount; i++)
        {
            m_userInfoPanel_GetChild[i] = m_dicUiAnim["UserInfo_panel"].transform.GetChild(i).gameObject;
        }

        int buttonCount = GameObject.FindGameObjectsWithTag("Button").Length;
        m_mainUIBtn = new Button[buttonCount];
        m_mainUIBtnobj = new GameObject[buttonCount];
        m_mainUIBtnobj = GameObject.FindGameObjectsWithTag("Button");
        for (int i = 0; i < buttonCount; i++)
        {
            m_mainUIBtn[i] = null;
            m_mainUIBtn[i] = m_mainUIBtnobj[i].GetComponent<Button>();
        }

        var enumerator = m_dicUiAnim.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.gameObject.SetActive(false);
        }
    }

    void InteractableCheck()
    {
        var enumerator = m_dicUiAnim.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if(enumerator.Current.Value.GetCurrentAnimatorStateInfo(0).IsName("EnableUI") || enumerator.Current.Value.GetCurrentAnimatorStateInfo(0).IsName("DisableUI"))
            {
                for (int i = 0; i < m_mainUIBtn.Length; i++)
                {
                    m_mainUIBtn[i].interactable = false;
                }
            }
            else if(enumerator.Current.Value.GetCurrentAnimatorStateInfo(0).IsName("IdleUI") || enumerator.Current.Value.GetCurrentAnimatorStateInfo(0).IsName("MainUIIdle"))
            {
                for (int i = 0; i < m_mainUIBtn.Length; i++)
                {
                    m_mainUIBtn[i].interactable = true;
                }
            }
            if(enumerator.Current.Value.GetCurrentAnimatorStateInfo(0).IsName("IdleUI"))
            {
                enumerator.Current.Value.gameObject.SetActive(m_uiActiveCheck);
            }
        }

   
    }

}
