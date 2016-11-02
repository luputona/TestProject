using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class LoadMapData : Singleton<LoadMapData>
{
    private static LoadMapData Instance;

    public JsonData m_mapJson;
    
    public List<MapInfo> m_mapInfoList = new List<MapInfo>();
    //public List<CDifficulty> m_diffList = new List<CDifficulty>();

    void Awake()
    {
       

        StartCoroutine(GetMapData());
    }

	
	IEnumerator GetMapData()
    {
        WWW www = new WWW("http://54.238.128.34/GetMapData.php");

        yield return www;

        string serverDB = www.text;

        m_mapJson = JsonMapper.ToObject(serverDB);

        if(www.isDone)
        {
            //Debug.Log("Map Data isDone");
        }
        ConstructMapData();
        
    }

    void ConstructMapData()
    {
        for (int i = 0; i < m_mapJson.Count; i++)
        {            
            m_mapInfoList.Add(new MapInfo((int)m_mapJson[i]["ID"], m_mapJson[i]["Name"].ToString()));
            //print("m_mapInfoList : " + m_mapInfoList[i].MapName);
            
            //for (int j = 0; j < m_mapJson[i]["Difficulty"].Count; j++)
            //{
            //    print("j : " + j);
            //    m_diffList.Add(new CDifficulty((int)m_mapJson[i]["Difficulty"][j]["AreaID"], m_mapJson[i]["Difficulty"][j]["AreaName"].ToString(), m_mapJson[i]["Difficulty"][j]["Info"].ToString()));
            //    print("m_diffList : " + m_diffList[i].AreaName);
            //}
        }
    }
    
    
}
public class CDifficulty
{
    public int AreaID { get; set; }
    public string AreaName { get; set; }
    public string AreaInfo { get; set; }

    public CDifficulty(int _areaid, string _areaname, string _areainfo)
    {
        this.AreaID = _areaid;
        this.AreaName = _areaname;
        this.AreaInfo = _areainfo;
    }
}
public class MapInfo
{
    public int MapID { get; set; }
    //public int AreaID { get; set; }
    //public string AreaName { get; set; }
    //public string AreaInfo { get; set; }
    public string MapName { get; set; }

    public CDifficulty[] Difficulty;

    public MapInfo(int _mapid, string _mapname)
    {
        this.MapID = _mapid;
        this.MapName = _mapname;
    }
    //public MapInfo(int _mapid, string _mapname, int _areaid, string _areaname, string _areainfo)
    //{
    //    this.MapID = _mapid;
    //    this.MapName = _mapname;

    //    Difficulty = new CDifficulty[3];
    //    for(int i = 0; i< 3; i++)
    //    {
    //        Difficulty[i] = new CDifficulty(_areaid, _areaname, _areainfo);
    //    }
    //}

    //public MapInfo(int _mapid, string _mapname, CDifficulty _difiiculty)
    //{
    //    this.MapID = _mapid;
    //    this.MapName = _mapname;

    //    Difficulty = new CDifficulty[3];
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Difficulty[i] = new CDifficulty(_difiiculty.AreaID, _difiiculty.AreaName, _difiiculty.AreaName);
    //    }
    //}

}
