using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour
{
    public bool m_monsterCollCheck = false;

    public float m_bulletSpeed = 1.0f;
    private Vector3 m_vecter;
	// Use this for initialization
	void Start ()
    {
        m_vecter = new Vector3(1.0f,0.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Translate(m_vecter * Time.deltaTime * m_bulletSpeed);
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.tag == "Monster")
        {
            print("Checkmonster : " + m_monsterCollCheck);
            BattleSpriteAction.GetInstance.m_collCheck = true;
           
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.collider.tag == "Monster")
        {
            print("Checkmonster : " + m_monsterCollCheck);
            BattleSpriteAction.GetInstance.m_collCheck = true;

        }
    }
}
