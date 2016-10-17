using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{
    public string m_playername { get; set; }
    public float m_playerhealth { get; set; }
    public int m_playerscore { get; set; }


    public LoadData(string _playername, float _playerhealth, int _playerscore)
    {
        m_playername = _playername;
        m_playerhealth = _playerhealth;
        m_playerscore = _playerscore;
    }

   

}
