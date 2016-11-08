using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class CreateMonster : MonoBehaviour {

    [System.Serializable]
    public class Monster
    {
        public GameObject[] m_monster2;
    }

    public GameObject[] m_monsterPrefabs;
    
    public Monster[] m_monsters = new Monster[3];

    public MemoryPool[] pool;
    public int m_maxMonsterNumber = 2;
    //respawn time
    private float m_respawnRate = 4.0f;
    private float m_startTime;
    private float m_respawnTimeLeft;

    private float m_respawnRate2 = 4.0f;
    private float m_startTime2;
    private float m_respawnTimeLeft2;



    // Use this for initialization
    void Start ()
    {
        pool = new MemoryPool[4];
        pool[0] = new MemoryPool();
        pool[1] = new MemoryPool();
        pool[2] = new MemoryPool();

        pool[0].Create(m_monsterPrefabs[0], 2);
        pool[1].Create(m_monsterPrefabs[1], 2);
        pool[2].Create(m_monsterPrefabs[2], 2);

        m_startTime = Time.time;

        for (int i = 0; i < m_monsters.Length; i++)
        {
            m_monsters[i] = new Monster();
            m_monsters[i].m_monster2 = new GameObject[2];
            for (int j = 0; j < m_monsters[i].m_monster2.Length; j++)
            {

                if (m_monsters[i].m_monster2[j] == null)
                {
                    m_monsters[i].m_monster2[j] = pool[i].NewItem();
                    m_monsters[i].m_monster2[j].transform.position = this.transform.position;
                    //break;
                }
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        //몬스터가 null 이면 할당해주고  null이 아니면 앞으로 이동시킴,
        //몬스터가 캐릭터와 충돌 후 사망시 , null로 처리해주고 다시 할당해줌
        //몬스터가 캐릭터 충돌 판정 체크는 몬스터에 붙인 스크립트에 bool 로 체크, 
        //충돌이면 true로 반환한걸 받아옴, 

        m_respawnTimeLeft = Time.time - m_startTime;
        m_respawnRate = Random.Range(1.0f, 80.0f);
        
        if (m_respawnTimeLeft > m_respawnRate)
        {
            //Debug.Log("random : " + m_respawnRate);
            //for (int i = 0; i < m_monsters.Length; i++)
            //{
            //    for(int j = 0; j< 2; j++)
            //    {
                    
            //        if (m_monsters[i].m_monster2[j] == null)
            //        {
            //            m_monsters[i].m_monster2[j] = pool[i].NewItem();
            //            m_monsters[i].m_monster2[j].transform.position = this.transform.position;
            //            break;
            //        }                  
                    
            //    }
            //}

            m_startTime = Time.time;
            m_respawnTimeLeft = 0.0f;
        }


        m_respawnTimeLeft2 = Time.time - m_startTime2;
        m_respawnRate2 = Random.Range(1.0f,90.0f);
       
        if (m_respawnTimeLeft2 > m_respawnRate2)
        {
            //Debug.Log("random2 : " + m_respawnRate2);
            //for (int i = 0; i < m_monsters.Length; i++)
            //{
            //    for(int j = 0; j< 2; j++)
            //    {

            //        if (m_monsters[i].m_monster2[j] == null)
            //        {
            //            m_monsters[i].m_monster2[j] = pool[i].NewItem();
            //            m_monsters[i].m_monster2[j].transform.position = this.transform.position;
            //            break;
            //        }                  

            //    }
            //}

            m_startTime2 = Time.time;
            m_respawnTimeLeft2 = 0.0f;
        }


        //for(int i = 0; i< m_monsters.Length; i++)
        //{
        //    for(int j = 0; j < m_monsters[i].m_monster2.Length; j++)
        //    {
        //        if(m_monsters[i].m_monster2[j])
        //        {
        //            // 조건문부분에 몬스터 충돌로 받아오는 bool을 체크
        //            if(true)
        //            {
        //                pool[i].RemoveItem(m_monsters[i].m_monster2[j]);
        //                m_monsters[i].m_monster2[j] = null;
        //            }
        //        }
        //    }
        //}
    }

    //메모리 풀클래스를 할당
    //DB에있는 몬스터의 ID를 조건문으로 걸러낸 후  m_monsterPrefabs에 담고 create하는 초기화 함수 
    //내부 할당은 for문으로 일괄 할당,
    void InitializeMonster()
    {
        //pool[0].Create(m_monsterPrefabs[0], m_maxMonsterNumber);
        //pool[1].Create(m_monsterPrefabs[1], m_maxMonsterNumber);
        //pool[2].Create(m_monsterPrefabs[2], m_maxMonsterNumber);
        int count = LoadMonsterData.GetInstance.m_monsterList.Count;

        pool = new MemoryPool[count];
        for(int i = 0; i< count; i++)
        {
            pool[i] = new MemoryPool();
        }
        //pool[0] = new MemoryPool();
        //pool[1] = new MemoryPool();
        //pool[2] = new MemoryPool();


        m_monsterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/");

        for (int i = 0; i < count; i++ )
        {
            pool[i].Create(m_monsterPrefabs[LoadMonsterData.GetInstance.m_monsterList[i].Id], m_maxMonsterNumber);
        }

        if(LoadMonsterData.GetInstance.m_monsterList[0].Id == 1)
        {

        }
        
    }
}
