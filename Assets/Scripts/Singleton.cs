using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;

    public static T GetInstance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(T)) as T;
                
                //if (m_instance == null)
                //{
                //    Debug.Log("nothing " + m_instance.ToString());
                //    return null;
                //}
            }
            if(m_instance == null)
            {
                GameObject obj = new GameObject();
                m_instance = obj.AddComponent(typeof(T)) as T;
            }
            return m_instance;
        }
    }
	
}
