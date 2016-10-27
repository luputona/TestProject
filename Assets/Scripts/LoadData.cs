using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{


    public string m_playername { get; set; }
    public int m_playerhealth { get; set; }
    public int m_hp { get; set; }
    public int m_mp { get; set; }
    public int m_attack { get; set; }
    public int m_defence { get; set; }
    public int m_level { get; set; }



    public LoadData()
    {
       
    }

   

}
