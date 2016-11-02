using UnityEngine;
using System.Collections;

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
	[SerializeField] LayerMask groundMask;

	public Animator animator;
	[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;

	public int hp = 4;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		float axis = 1;
		bool isDown = Input.GetAxisRaw ("Vertical") < 0;

		if (Input.GetButtonDown ("Jump")) {
			rig2d.velocity = new Vector2 (rig2d.velocity.x, 5);
		}

		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1f, groundMask);

		// update animator parameters
		animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
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
	}

    public void NormalAttack()
    {
        animator.SetTrigger(hashAttack1);
    }
    public void SkillAttack()
    {
        animator.SetTrigger(hashAttack2);
    }
    public void QAttack()
    {
        animator.SetTrigger(hashAttack3);
    }
}
