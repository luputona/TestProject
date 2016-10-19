using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestShowDB : MonoBehaviour {
    public Text text;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("ID : {0} \n name : {1} \n cost : {2} \n hp : {3} \n mp : {4} \n Skillname : {5} \n skill mp : {6} \n skilldamage : {7} \n attack : {8} \n defence : {9} \n Q name : {10} ", LoadCharacterData.GetInstance.m_charList[0].Id, LoadCharacterData.GetInstance.m_charList[0].Name, LoadCharacterData.GetInstance.m_charList[0].Cost, LoadCharacterData.GetInstance.m_charList[0].Hp, LoadCharacterData.GetInstance.m_charList[0].Mp, LoadCharacterData.GetInstance.m_charList[0].SkillName, LoadCharacterData.GetInstance.m_charList[0].SkillMp, LoadCharacterData.GetInstance.m_charList[0].SkillDamage, LoadCharacterData.GetInstance.m_charList[0].Attack, LoadCharacterData.GetInstance.m_charList[0].Defence, LoadCharacterData.GetInstance.m_charList[0].QName);
       
    }
}
