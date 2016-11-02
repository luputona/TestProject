using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//GPGS 매니져에있는 SaveData에 있는 데이타들을 받아서 외부에서 엑세스 가능하게 한다

public class LoadData : Singleton<LoadData>
{

    public string m_playername;// { get; set; }

    public int m_hp; //{ get; set; }
    public int m_mp; //{ get; set; }
    public int m_attack; //{ get; set; }
    public int m_defence; //{ get; set; }
    public int m_level; //{ get; set; }
    public int m_gold; //{ get; set; }
    public int m_item; //{ get; set; }

    public string m_selectCharacter;
    public string m_selectVehicle;



    public LoadData()
    {
       
    }
    public void GetUserData(string _userName, int _level, int _hp, int _mp, int _attack, int _defence, int _gold, int _item, string _selectcharacter)
    {
        m_playername = _userName;
        m_hp = _hp;
        m_mp = _mp;
        m_attack = _attack;
        m_defence = _defence;
        m_level = _level;
        m_gold = _gold;
        m_item = _item;
        m_selectCharacter = _selectcharacter;

        UserInfomation.GetInstance.GetUserData(_userName,_level ,_hp, _mp, _attack, _defence, _gold, _item, _selectcharacter);
    }

    public void LoadInventory(List<CharacterData> _loadinven)
    {
        _loadinven = new List<CharacterData>();
        for(int i = 0; i < _loadinven.Count; i++ )
        {
            MainInvenUIManager.GetInstance.m_inventory.Add(_loadinven[i]);
        }
        
    }
}
