using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;


public class Test : MonoBehaviour {

    public string[] m_serverTime;

    public Text m_text;
    public static int m_count = 0;
    public float health = 0;

    public Text countText;
    public Text healthText;
    public Text time;

    public Text[] m_statText;
    public GameObject m_save_load_Data;

    public string second;
    public int m_second;
    public int m_min;
    public int m_hour;
    public int m_day;
    public int m_mon;
    public int m_year;
   

    void Start()
    {
        m_save_load_Data = GameObject.FindGameObjectWithTag("SaveLoadData").gameObject;
        int text_count = m_save_load_Data.transform.childCount;

        m_statText = new Text[text_count];
        for (int i = 0; i < text_count; i++)
        {
            m_statText[i] = m_save_load_Data.transform.GetChild(i).GetComponent<Text>();
        }
        //StartCoroutine(Time());
    }

	// Update is called once per frame
	void Update () 
    {
        StartCoroutine(GetTime());
        m_statText[0].text = string.Format("Name : {0}", LoadData.GetInstance.m_playername);
        //m_statText[1].text = string.Format("Health : {0}", LoadData.GetInstance.m_playerhealth);
        //m_statText[2].text = string.Format("count : {0}", LoadData.GetInstance.m_playerscore);

       

        
        //time.text = string.Format("{0}/{1:00}/{2:00}\n{3:00}/{4:00}/{5:00} ", DateTime.Now.Year, DateTime.Now.Month ,DateTime.Now.Day ,DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //Debug.Log(NtpTime().ToString());
	}
    IEnumerator Time()
    {
        while(true)
        {
            yield return Yielders.Get(1.0f);
            StartCoroutine(GetTime());
        }
    }

    IEnumerator GetTime()
    {
        WWW www = new WWW("http://ec2-54-238-128-34.ap-northeast-1.compute.amazonaws.com/TestTime/Testphp.php");
        yield return www;

        string serverTimeString = www.text;
        //print(serverTimeString);

        m_serverTime = serverTimeString.Split(';');

        //print(GetServerTime(m_serverTime[0], "Year:"));

        m_year = Int32.Parse(GetServerTime(m_serverTime[0], "Year:"));
        m_mon = Int32.Parse(GetServerTime(m_serverTime[0], "Mon:"));
        m_day = Int32.Parse(GetServerTime(m_serverTime[0], "Day:"));
        m_hour = Int32.Parse(GetServerTime(m_serverTime[0], "Hours:"));
        m_min = Int32.Parse(GetServerTime(m_serverTime[0], "Minutes:"));
        m_second = Int32.Parse(GetServerTime(m_serverTime[0], "Seconds:"));

        //m_second = Int32.Parse(GetServerTime(m_serverTime[0], "mon:"));
        
        //time.text = string.Format("{0}", serverTimeString);
        //time.text = string.Format("{0}", GetServerTime(m_serverTime[0], "seconds:"));
        time.text = string.Format("Year:{0} / Mon:{1:00} / Day:{2:00} \n Hour:{3:00} / Minute:{4:00} / Second:{5:00}", m_year,m_mon,m_day,m_hour,m_min,m_second);
        

        //Debug.Log("Time : " + www.text);
    }
    
    string GetServerTime(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        
        if(value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }

    public void CountUp()
    {
        ++m_count;
        //++health;
        //++GPGSMgr.GetInstance.count;
        //++GPGSMgr.GetInstance.health;
        m_text.text = string.Format("{0}", m_count);

        //countText.text = string.Format("count : {0}", GPGSMgr.GetInstance.count);
        //healthText.text = string.Format("Health : {0}", GPGSMgr.GetInstance.health);

    }
    public void CountDown()
    {
        --m_count;
        //--health;
        //--GPGSMgr.GetInstance.count;// -= m_count;
        //--GPGSMgr.GetInstance.health; //-= health;

        m_text.text = string.Format("{0}", m_count);

        //countText.text = string.Format("count : {0}", GPGSMgr.GetInstance.count);
        //healthText.text = string.Format("Health : {0}", GPGSMgr.GetInstance.health);
    }

    
    public static double NtpTime()
    {
        try
        {
            byte[] ntpData = new byte[48];

            //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)
            ntpData[0] = 0x1B;

            IPAddress[] addresses = Dns.GetHostEntry("0.asia.pool.ntp.org").AddressList;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(new IPEndPoint(addresses[0], 123));
            socket.ReceiveTimeout = 1000;

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intc = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong frac = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            return (double)((intc * 1000) + ((frac * 1000) / 0x100000000L));
        }
        catch (Exception exception)
        {
            Debug.Log("Could not get NTP time");
            Debug.Log(exception);
            return LocalTime();
        }
    }

    public static double LocalTime()
    {
        return DateTime.Now.Subtract(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}

