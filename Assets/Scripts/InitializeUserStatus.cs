using UnityEngine;
using System.Collections;

public class InitializeUserStatus : Singleton<InitializeUserStatus>
{

    public int m_inithp; //{ get; set; }
    public int m_initmp; //{ get; set; }
    public int m_initattack; //{ get; set; }
    public int m_initdefence; //{ get; set; }
    public int m_initgold; //{ get; set; }
    public int m_inititem; //{ get; set; }
    public int m_initscore;
    public int m_initstatpoint;
    public string m_initmaincharacter;
    public string m_goolgleid;

    void Awake()
    {

        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }

        m_inithp = 100;
        m_initmp = 100;
        m_initattack = 5;
        m_initdefence = 5;

        m_initgold = 10000;
        m_inititem = 1;
        m_initmaincharacter = "UnityChan";

    }
    // Use this for initialization
    void Start ()
    {
	
	}
	
}
