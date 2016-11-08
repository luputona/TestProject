using UnityEngine;
using System.Collections;

public enum MOTIONCHECK
{
    E_IDLE,
    E_NORMALATTACK,
    E_SKILL,
    E_Q
}
public enum DISTANCE
{
    E_LONGTYPE, //원거리
    E_SHORTTYPE, //근거리
    E_MIXTYPE // 일반공격은 근거리, 스킬은 원거리
}
 
public class BattleSpriteAction : Singleton<BattleSpriteAction>
{
	static int hashSpeed = Animator.StringToHash ("Speed");
	static int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	static int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	static int hashIsCrouch = Animator.StringToHash ("IsCrouch");
	static int hashAttack1 = Animator.StringToHash ("Attack1");
	static int hashAttack2 = Animator.StringToHash ("Attack2");
	static int hashAttack3 = Animator.StringToHash ("Attack3");

	static int hashDamage = Animator.StringToHash ("Damage");
	static int hashIsDead = Animator.StringToHash ("IsDead");

	[SerializeField] private float characterHeightOffset = 0.2f;
	//[SerializeField] LayerMask groundMask;

	public Animator animator;
	[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;


    public GameObject m_normalAttack_collCheck;
    public GameObject m_skill_collCheck;
    public GameObject[] m_bulletPrefabs;
    public GameObject[] m_bullet;
    
    public MOTIONCHECK m_emotion;
    public DISTANCE m_distance;
    public bool m_collCheck;
    public int m_bulletCount = 0;

    public MemoryPool m_bulletpool = new MemoryPool();

	void Awake ()
	{
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();

        if(m_distance == DISTANCE.E_SHORTTYPE)
        {
            m_normalAttack_collCheck = this.gameObject.transform.FindChild("Normal_CollCheck").gameObject;
            m_skill_collCheck = gameObject.transform.FindChild("Skill_CollCheck").gameObject;
        }
        if(m_distance == DISTANCE.E_LONGTYPE)
        {

        }
        if(m_distance == DISTANCE.E_MIXTYPE)
        {
            m_normalAttack_collCheck = this.gameObject.transform.FindChild("Normal_CollCheck").gameObject;

        }
        
    }

    void Start()
    {
        m_bulletPrefabs = Resources.LoadAll<GameObject>("Prefabs/Bullet/");
        
        if(m_distance == DISTANCE.E_SHORTTYPE)
        {
            m_normalAttack_collCheck.SetActive(false);
            m_skill_collCheck.SetActive(false);
        }
        if(m_distance == DISTANCE.E_LONGTYPE)
        {

        }

        if(m_distance == DISTANCE.E_MIXTYPE)
        {
            if(PlayerPrefs.GetString("SelectCharacter") == "Yuko")
            {
                m_bulletCount = 5;
                m_bullet = new GameObject[m_bulletCount];
                m_bulletpool.Create(m_bulletPrefabs[0], m_bulletCount);
                for(int i = 0; i < m_bullet.Length; i++)
                {
                    m_bullet[i] = null;
                }
            }
        }
        
    }
	void Update ()
	{
		float axis = 1;
		bool isDown = Input.GetAxisRaw ("Vertical") < 0;

		if (Input.GetButtonDown ("Jump")) {
			rig2d.velocity = new Vector2 (rig2d.velocity.x, 5);
		}

		//var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1f, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		//animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat (hashSpeed, Mathf.Abs (axis));
		//if( CharacterControllManager.GetInstance.m_normalAttackCheck )
  //      {
  //          animator.SetTrigger(hashAttack1);
  //      }
		//if(CharacterControllManager.GetInstance.m_normalAttackAnimCheck)
  //      {
  //          animator.SetTrigger(hashAttack2);
  //      }
		//if(CharacterControllManager.GetInstance.m_skillAnimCheck)
  //      {
  //          animator.SetTrigger(hashAttack3);
  //      }

		// flip sprite
		if (axis != 0)
			spriteRenderer.flipX = axis < 0;

        if(m_distance == DISTANCE.E_SHORTTYPE)
        {
            ShotTypeMotionCheck();
        }
        else if(m_distance == DISTANCE.E_LONGTYPE)
        {
            LongTypeMotionCheck();
        }
        else if(m_distance == DISTANCE.E_MIXTYPE)
        {
            MixTypeMotionCheck();
        }
       

        for(int i = 0;  i < m_bullet.Length; i++)
        {
            if(m_bullet[i])
            {
                if (m_collCheck)
                {
                    m_bulletpool.RemoveItem(m_bullet[i]);
                    m_bullet[i] = null;                    
                }
                m_collCheck = false;
            }            
        }
    }

    void ShotTypeMotionCheck()
    {
        if (m_emotion == MOTIONCHECK.E_NORMALATTACK)
        {
            m_normalAttack_collCheck.SetActive(true);
            m_skill_collCheck.SetActive(false);
        }
        else if(m_emotion == MOTIONCHECK.E_SKILL)
        {
            m_normalAttack_collCheck.SetActive(false);
            m_skill_collCheck.SetActive(true);
        }
        else if(m_emotion == MOTIONCHECK.E_Q)
        {
            m_normalAttack_collCheck.SetActive(false);
            m_skill_collCheck.SetActive(false);
        }
        else if(m_emotion == MOTIONCHECK.E_IDLE)
        {
            m_normalAttack_collCheck.SetActive(false);
            m_skill_collCheck.SetActive(false);
        }
    }

    void LongTypeMotionCheck()
    {
        if (m_emotion == MOTIONCHECK.E_NORMALATTACK)
        {
            //m_normalAttack_collCheck.SetActive(true);
            //m_skill_collCheck.SetActive(false);
        }
        else if (m_emotion == MOTIONCHECK.E_SKILL)
        {
            //m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(true);
        }
        else if (m_emotion == MOTIONCHECK.E_Q)
        {
            //m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(false);
        }
        else if (m_emotion == MOTIONCHECK.E_IDLE)
        {
            //m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(false);
        }
    }


    public void MixTypeMotionCheck()
    {
        if (m_emotion == MOTIONCHECK.E_NORMALATTACK)
        {
            m_normalAttack_collCheck.SetActive(true);
            //m_skill_collCheck.SetActive(false);
        }
        else if (m_emotion == MOTIONCHECK.E_SKILL)
        {
            m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(true);
            
        }
        else if (m_emotion == MOTIONCHECK.E_Q)
        {
            //m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(false);
        }
        else if (m_emotion == MOTIONCHECK.E_IDLE)
        {
            m_normalAttack_collCheck.SetActive(false);
            //m_skill_collCheck.SetActive(false);
        }
    }

    public void NormalAttack()
    {        
        animator.SetTrigger(hashAttack1);
        m_emotion = MOTIONCHECK.E_NORMALATTACK;
        
    }
    public void SkillAttack()
    {
        animator.SetTrigger(hashAttack2);
        m_emotion = MOTIONCHECK.E_SKILL;

        if (PlayerPrefs.GetString("SelectCharacter") == "Yuko")
        {
            for (int i = 0; i < m_bullet.Length; i++)
            {
                if (m_bullet[i] == null)
                {
                    m_bullet[i] = m_bulletpool.NewItem();
                    m_bullet[i].transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.15f);                    
                    break;
                }
            }
        }

    }
    public void QAttack()
    {
        animator.SetTrigger(hashAttack3);
        m_emotion = MOTIONCHECK.E_Q;        
    }


}
