using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class CreateMonster : Singleton<CreateMonster>
{

    [System.Serializable]
    public class Monster
    {
        public GameObject[] m_monster2;
    }

    public GameObject[] m_monsterPrefabs;
    
    public Monster[] m_monsters = new Monster[3];

    public MemoryPool[] pool;
    public int m_maxMonsterNumber = 2;
    public bool m_monsterHpCheck = false;
    //respawn time
    private float m_respawnRate = 4.0f;
    private float m_startTime;
    private float m_respawnTimeLeft;

    public float m_respawnRate2 = 4.0f;
    public float m_startTime2;
    public float m_respawnTimeLeft2;

    public float m_startTime3;
    public float m_endTime;


    // Use this for initialization
    void Start ()
    {
        //m_startTime = Time.time;

        //for (int i = 0; i < m_monsters.Length; i++)
        //{
        //    m_monsters[i] = new Monster();
        //    m_monsters[i].m_monster2 = new GameObject[2];
        //    for (int j = 0; j < m_monsters[i].m_monster2.Length; j++)
        //    {

        //        if (m_monsters[i].m_monster2[j] == null)
        //        {
        //            m_monsters[i].m_monster2[j] = pool[i].NewItem();
        //            m_monsters[i].m_monster2[j].transform.position = this.transform.position;
        //            //break;
        //        }
        //    }
        //}
        StartCoroutine(DelayInit());

    }
	
	// Update is called once per frame
	public void UpdateCreatMonster ()
    {
        //몬스터가 null 이면 할당해주고  null이 아니면 앞으로 이동시킴,
        //몬스터가 캐릭터와 충돌 후 사망시 , null로 처리해주고 다시 할당해줌
        //몬스터가 캐릭터 충돌 판정 체크는 몬스터에 붙인 스크립트에 bool 로 체크, 
        //충돌이면 true로 반환한걸 받아옴, 

        //m_respawnTimeLeft = Time.time - m_startTime;
        //m_respawnRate = Random.Range(110.0f, 300.0f);

        //if (m_respawnTimeLeft > m_respawnRate)
        //{
        //    //Debug.Log("random : " + m_respawnRate);
        //    for (int i = 0; i < m_monsters.Length; i++)
        //    {
        //        for (int j = 0; j < 2; j++)
        //        {
        //            if (m_monsters[i].m_monster2[j] == null)
        //            {
        //                m_monsters[i].m_monster2[j] = pool[i].NewItem();
        //                m_monsters[i].m_monster2[j].transform.position = this.transform.position;
        //                m_monsters[i].m_monster2[j].transform.name = LoadMonsterData.GetInstance.m_monsterList[i].Name;
        //                break;
        //            }
        //        }
        //    }
        //    m_startTime = Time.time;
        //    m_respawnTimeLeft = 0.0f;
        //}


        m_respawnTimeLeft2 = Time.time - m_startTime2;
        m_respawnRate2 = Random.Range(2.0f,10.0f);
       
        if (m_respawnTimeLeft2 > m_respawnRate2)
        {
            //Debug.Log("random2 : " + m_respawnRate2);
            int random = Random.Range(0, m_monsters.Length);
            for (int i = 0; i < m_monsters.Length; i++)
            {
                //for (int j = 0; j < m_monsters[i].m_monster2.Length; j++)
                //{
                    if (m_monsters[random].m_monster2[0] == null)
                    {
                        m_monsters[random].m_monster2[0] = pool[random].NewItem();
                        m_monsters[random].m_monster2[0].transform.position = this.transform.position;
                        m_monsters[random].m_monster2[0].transform.name = LoadMonsterData.GetInstance.m_monsterList[random].Name;                        
                        break;
                    }
                    if (m_monsters[random].m_monster2[1] == null)
                    {
                        m_monsters[random].m_monster2[1] = pool[random].NewItem();
                        m_monsters[random].m_monster2[1].transform.position = this.transform.position;
                        m_monsters[random].m_monster2[1].transform.name = LoadMonsterData.GetInstance.m_monsterList[random].Name;
                        break;
                    }


                    //if (m_monsters[i].m_monster2[j] != null)
                    //{
                    //    m_monsters[i].m_monster2[j].SetActive(true);
                    //    //break;
                    //}
                    //if (LoadMonsterData.GetInstance.m_monsterList[0].Name == m_monsters[i].m_monster2[j].name)
                    //{
                    //    //print(m_monsters[i].m_monster2[j].name);
                    //}
                    //if (LoadMonsterData.GetInstance.m_monsterList[1].Name == m_monsters[i].m_monster2[j].name)
                    //{

                    //}
                    //if (LoadMonsterData.GetInstance.m_monsterList[2].Name == m_monsters[i].m_monster2[j].name)
                    //{

                    //}
               // }
            }

            m_startTime2 = Time.time;
            m_respawnTimeLeft2 = 0.0f;
        }


        for (int i = 0; i < m_monsters.Length; i++)
        {
            for (int j = 0; j < m_monsters[i].m_monster2.Length; j++)
            {
                if (m_monsters[i].m_monster2[j])
                {
                    // 조건문부분에 몬스터 충돌로 받아오는 bool을 체크
                    if (m_monsterHpCheck  && m_monsters[i].m_monster2[j].GetComponent<MonsterController>().m_hpCheck)
                    {
                        for (int k = 0; k < LoadMonsterData.GetInstance.m_monsterList.Count; k++)
                        {
                            if (m_monsters[i].m_monster2[j].name == LoadMonsterData.GetInstance.m_monsterList[k].Name)
                            {
                                CharacterControllManager.GetInstance.m_score += LoadMonsterData.GetInstance.m_monsterList[k].Score;
                                CharacterControllManager.GetInstance.m_getGold += LoadMonsterData.GetInstance.m_monsterList[k].Gold;
                                PlayerPrefs.GetInt("GetGold", CharacterControllManager.GetInstance.m_getGold);
                                PlayerPrefs.GetInt("GetScore", CharacterControllManager.GetInstance.m_score);

                                if(CharacterControllManager.GetInstance.m_curSp < CharacterControllManager.GetInstance.m_maxSp)
                                {
                                    CharacterControllManager.GetInstance.m_curSp += LoadMonsterData.GetInstance.m_monsterList[k].RecoverSP;
                                    int tempSp = CharacterControllManager.GetInstance.m_maxSp - CharacterControllManager.GetInstance.m_curSp;
                                    if (tempSp < LoadMonsterData.GetInstance.m_monsterList[k].RecoverSP )
                                    {
                                        CharacterControllManager.GetInstance.m_curSp += tempSp;
                                    }
                                }
                                
                            }
                        }
                        pool[i].RemoveItem(m_monsters[i].m_monster2[j]);
                        m_monsters[i].m_monster2[j] = null;

                        m_monsterHpCheck = false;

                        
                    }
                    
                }
            }
        }
    }

    IEnumerator DelayInit()
    {
        yield return Yielders.Get(0.5f);
        InitializeMonster();
    }

    

    //메모리 풀클래스를 할당
    //DB에있는 몬스터의 ID를 조건문으로 걸러낸 후  m_monsterPrefabs에 담고 create하는 초기화 함수 
    //내부 할당은 for문으로 일괄 할당,
    void InitializeMonster()
    {
        int count = LoadMonsterData.GetInstance.m_monsterList.Count;

        pool = new MemoryPool[count];
        for(int i = 0; i< count; i++)
        {
            pool[i] = new MemoryPool();
        }
        //에셋번들에서 로드
        //for(int i = 0; i < AssetLoader.GetInstance.monster_list.Count; i++)
        //{
        //    m_monsterPrefabs[i] = AssetLoader.GetInstance.monster_list[i];
        //}
        m_monsterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/");

        for (int i = 0; i < count; i++ )
        {
            pool[i].Create(m_monsterPrefabs[LoadMonsterData.GetInstance.m_monsterList[i].Id - 1], m_maxMonsterNumber);
        }

        for (int i = 0; i < m_monsters.Length; i++)
        {
            m_monsters[i] = new Monster();
            m_monsters[i].m_monster2 = new GameObject[2];
            //for (int j = 0; j < m_monsters[i].m_monster2.Length; j++)
            //{
            //    m_monsters[i].m_monster2[j] = null;
            //    if (m_monsters[i].m_monster2[j] == null)
            //    {
            //        m_monsters[i].m_monster2[j] = pool[i].NewItem();
            //        m_monsters[i].m_monster2[j].transform.position = this.transform.position;
            //        m_monsters[i].m_monster2[j].transform.name = LoadMonsterData.GetInstance.m_monsterList[i].Name;
                    
            //        //break;
            //    }
            //    if (m_monsters[i].m_monster2[j] != null)
            //    {
            //        m_monsters[i].m_monster2[j].SetActive(false);
            //    }
            //        //몬스터 스펙 셋팅
            //    if (LoadMonsterData.GetInstance.m_monsterList[0].Name == m_monsters[i].m_monster2[j].name)
            //    {
            //        //print(m_monsters[i].m_monster2[j].name);
            //    }
            //    if (LoadMonsterData.GetInstance.m_monsterList[1].Name == m_monsters[i].m_monster2[j].name)
            //    {

            //    }
            //    if (LoadMonsterData.GetInstance.m_monsterList[2].Name == m_monsters[i].m_monster2[j].name)
            //    {

            //    }
            //}
        }

        //추후 m_monsterList[].name하고 에셋번들 이름하고 비교로 변경
        

    }
}
