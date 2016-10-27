using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public Dictionary<string, GameObject> m_difficulty_panel_child;
    public GameObject m_selectMap_Difficulty_Panel;
    public List<List<string>> m_loadmapNameList = new List<List<string>>();
 
    private string m_mapArea;


	// Use this for initialization
	void Start ()
    {
        m_selectMap_Difficulty_Panel = MainUIAnimController.m_dicUiAnim["Select_MapPanel"].transform.FindChild("Difficulty_Panel").gameObject;
        m_selectMap_Difficulty_Panel.SetActive(false);
        int difficultyPanelChild = m_selectMap_Difficulty_Panel.transform.childCount;
        m_difficulty_panel_child = new Dictionary<string,GameObject>();

        

        for (int i = 0; i < difficultyPanelChild; i++)
        {
            m_difficulty_panel_child.Add(m_selectMap_Difficulty_Panel.transform.GetChild(i).name, m_selectMap_Difficulty_Panel.transform.GetChild(i).gameObject);
        }

        InitMapName();
        //var etor = m_difficulty_panel_child.GetEnumerator();
        //while (etor.MoveNext())
        //{
        //    Debug.Log(etor.Current.Value.name);
        //}

    }
	
	// Update is called once per frame
	void Update ()
    { 
        //for(int i =0;i<m_loadmapNameList.Count;i++)
        //{
        //    for(int j = 0; j<m_loadmapNameList[i].Count; j++)
        //    {
        //        print("namelist : " + m_loadmapNameList[i][j]);
        //    }
            
        //}
    }

    void InitMapName()
    {
        for(int i = 0; i< LoadMapData.GetInstance.m_mapJson.Count; i++ )
        {
            m_loadmapNameList.Add(new List<string>());
            for(int j = 0; j< LoadMapData.GetInstance.m_mapJson[i]["Difficulty"].Count; j++)
            {
                m_loadmapNameList[i].Add(LoadMapData.GetInstance.m_mapJson[i]["Difficulty"][j]["AreaName"].ToString());
               
            }
        }
    }

    public void SelectMap(string _areaname)
    {
        m_selectMap_Difficulty_Panel.SetActive(true);
        
        m_mapArea = _areaname;

        if (_areaname == "Korea")
        {
            m_difficulty_panel_child["MapName_Text"].GetComponent<Text>().text = string.Format("{0}", LoadMapData.GetInstance.m_mapInfoList[0].MapName);
            m_difficulty_panel_child["Area01_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[0][0]);
            m_difficulty_panel_child["Area02_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[0][1]);
            m_difficulty_panel_child["Area03_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[0][2]);         
        }
        if(_areaname == "Japan")
        {
            m_difficulty_panel_child["MapName_Text"].GetComponent<Text>().text = string.Format("{0}", LoadMapData.GetInstance.m_mapInfoList[1].MapName);
            m_difficulty_panel_child["Area01_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[1][0]);
            m_difficulty_panel_child["Area02_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[1][1]);
            m_difficulty_panel_child["Area03_Text"].GetComponent<Text>().text = string.Format("{0}", m_loadmapNameList[1][2]);
        }
    }

    public void SelectMapBackButton()
    {
        m_selectMap_Difficulty_Panel.SetActive(false);
    }

    public void SelectDifficulty(string _difficultyName)
    {  
        if(_difficultyName == m_loadmapNameList[0][0])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[0][0]);
        }
        else if(_difficultyName == m_loadmapNameList[0][1])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[0][1]);
        }
        else if (_difficultyName == m_loadmapNameList[0][2])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[0][2]);
        }
        else if (_difficultyName == m_loadmapNameList[1][0])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[1][0]);
        }
        else if (_difficultyName == m_loadmapNameList[1][1])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[1][1]);
        }
        else if (_difficultyName == m_loadmapNameList[1][2])
        {
            PlayerPrefs.GetString("Difficulty", m_loadmapNameList[1][2]);
        }


    }

    
    
}
