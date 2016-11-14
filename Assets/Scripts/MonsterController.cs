using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour
{
    public int Id;// { get; set; }
    public string Category;// { get; set; }
    public string Name;// { get; set; }
    public int Hp; //{ get; set; }
    public int Defence; //{ get; set; }
    public int Attack;// { get; set; }
    public int RecoverSP; //{ get; set; }
    public int Score; //{ get; set; }
    public int Gold;

    public float m_moveSpeed;
    public bool m_hpCheck = false;
    public bool m_moveCheck = true;
    public float m_bounceVector;

    public float m_random;
    public float m_startTime;
         
    public Vector3 m_vetor3;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(InitTimer());
        m_bounceVector = -1.0f;
        m_vetor3 = new Vector3(m_bounceVector, 0, 0f);

        m_random = Random.Range(1.0f, 10.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_startTime = Time.time;
        
        if (m_startTime > m_random)
        {
            if (m_moveCheck)
            {
                Move();
            }
            else
            {
                StartCoroutine(Bounce());
            }
            m_startTime = 0;

        }
        

        if(Hp > 0)
        {
            m_hpCheck = false;
            CreateMonster.GetInstance.m_monsterHpCheck = false;
        }
        else
        {
            m_startTime = Time.time;
        }
    } 

    void HpCheck()
    {
        if (Hp <= 0)
        {
            m_hpCheck = true;
            CreateMonster.GetInstance.m_monsterHpCheck = true;
            InitializeMonsterInfo();            
        }
        else
        {
            m_hpCheck = false;
            CreateMonster.GetInstance.m_monsterHpCheck = false;
        }
    }
    void Move()
    {
        
        this.transform.Translate(m_vetor3 * m_moveSpeed * Time.deltaTime);
        //m_startTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Bullet") || coll.collider.CompareTag("NormalAttack")  )
        {
            Hp = Hp - (CharacterControllManager.GetInstance.m_outDamage - Defence);
            
            m_moveCheck = false;
            HpCheck();
        }        
        else if(coll.collider.CompareTag("Player"))
        {
            m_moveCheck = false;

            CharacterControllManager.GetInstance.m_curHp = CharacterControllManager.GetInstance.m_curHp - (Attack - CharacterControllManager.GetInstance.m_defence);
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Bullet"))
        {
            Hp = Hp - (CharacterControllManager.GetInstance.m_outDamage - Defence);
            
            m_moveCheck = false;
            HpCheck();
            
        }
        else if (coll.collider.CompareTag("Player"))
        {
            m_moveCheck = false;
        }

    }

    void InitializeMonsterInfo()
    {
        if(this.transform.name == LoadMonsterData.GetInstance.m_monsterList[0].Name)
        {
            Hp = LoadMonsterData.GetInstance.m_monsterList[0].Hp;
            Defence = LoadMonsterData.GetInstance.m_monsterList[0].Defence;
            Attack = LoadMonsterData.GetInstance.m_monsterList[0].Attack;
            RecoverSP = LoadMonsterData.GetInstance.m_monsterList[0].RecoverSP;
            Score = LoadMonsterData.GetInstance.m_monsterList[0].Score;
            Gold = LoadMonsterData.GetInstance.m_monsterList[0].Gold;
            m_moveSpeed = 1.0f;
        }
        if (this.transform.name == LoadMonsterData.GetInstance.m_monsterList[1].Name)
        {
            Hp = LoadMonsterData.GetInstance.m_monsterList[1].Hp;
            Defence = LoadMonsterData.GetInstance.m_monsterList[1].Defence;
            Attack = LoadMonsterData.GetInstance.m_monsterList[1].Attack;
            RecoverSP = LoadMonsterData.GetInstance.m_monsterList[1].RecoverSP;
            Score = LoadMonsterData.GetInstance.m_monsterList[1].Score;
            Gold = LoadMonsterData.GetInstance.m_monsterList[1].Gold;
            m_moveSpeed = 1.1f;
        }
        if (this.transform.name == LoadMonsterData.GetInstance.m_monsterList[2].Name)
        {
            Hp = LoadMonsterData.GetInstance.m_monsterList[2].Hp;
            Defence = LoadMonsterData.GetInstance.m_monsterList[2].Defence;
            Attack = LoadMonsterData.GetInstance.m_monsterList[2].Attack;
            RecoverSP = LoadMonsterData.GetInstance.m_monsterList[2].RecoverSP;
            Score = LoadMonsterData.GetInstance.m_monsterList[2].Score;
            Gold = LoadMonsterData.GetInstance.m_monsterList[2].Gold;
            m_moveSpeed = 1.2f;
        }
        
    }




    IEnumerator Bounce()
    {
        m_bounceVector = 1.0f;
        m_vetor3 = new Vector3(m_bounceVector, 0, 0f);
        
        Move();
        yield return Yielders.Get(0.25f);
        m_bounceVector = -1.0f;
        
        m_vetor3 = new Vector3(m_bounceVector, 0, 0f);
        m_moveCheck = true;
    }
    IEnumerator CollTimeCheck()
    {
        yield return Yielders.Get(2.0f);
        m_hpCheck = false;
    }
    IEnumerator InitTimer()
    {
        yield return Yielders.Get(0.5f);
        InitializeMonsterInfo();
    }
}
