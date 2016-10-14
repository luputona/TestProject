using UnityEngine;
using System.Collections;
using Vuforia;

public class CreateObject : MonoBehaviour
{
    public TrackableBehaviour m_trackableBehaviour;
    public DefaultTrackableEventHandler m_defaultTrackableEventHandler;
    public GameObject m_obj;

	// Use this for initialization
	void Start () 
    {
        m_defaultTrackableEventHandler = this.GetComponent<DefaultTrackableEventHandler>();
        m_trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (m_trackableBehaviour)
        {
            m_trackableBehaviour.RegisterTrackableEventHandler(m_defaultTrackableEventHandler);
        }

        //m_obj.transform.localScale = new Vector3(0, 0, 0);
        
	}
	
	public void GetGameObject()
    {
        if (m_trackableBehaviour.TrackableName == "Case")
        {
            if (m_obj == null)
            {
                m_obj = AssetLoader.GetInstance.assetObjects["Tank"];
                m_obj = Instantiate(m_obj, this.transform.position, Quaternion.Euler(0.0f, 180.0f, 0.0f)) as GameObject;
                m_obj.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                
                m_obj.transform.parent = this.gameObject.transform;                
            }
            else
            {
                m_obj.SetActive(true);
                m_obj.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
        }
        if (m_trackableBehaviour.TrackableName == "1000en_back" || m_trackableBehaviour.TrackableName == "1000enn_front")
        {           
            if(m_obj == null)
            {
                m_obj = AssetLoader.GetInstance.assetObjects["Live2D_haru"];
                m_obj = Instantiate(m_obj, new Vector3(this.gameObject.transform.position.x, 10.0f, this.gameObject.transform.position.z), Quaternion.Euler(270.0f, this.transform.rotation.y, this.transform.rotation.z)) as GameObject;
                m_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                m_obj.transform.parent = this.gameObject.transform;
                 
            }
            else
            {
                m_obj.SetActive(true);
                m_obj.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            }            
        }
    }

    public void DisposeGameObject()
    {
        if(m_obj != null)
        {
            m_obj.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
