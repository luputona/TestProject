using UnityEngine;
using System.Collections;

public class LoadObj : MonoBehaviour {

	// Use this for initialization
    public GameObject m_obj;
	void Start () 
    {
        InitObject();
	}
	
    void InitObject()
    {
        Instantiate(AssetLoader.GetInstance.assetObjects["Background"],new Vector3(0,0,0),Quaternion.identity);
        Instantiate(AssetLoader.GetInstance.assetObjects["MyCube"], new Vector3(5.0f, 1.0f, 5.0f), Quaternion.identity);
        Instantiate(AssetLoader.GetInstance.assetObjects["Tank"], new Vector3(5.0f, 0, 5.0f), Quaternion.identity);
        m_obj = Instantiate(AssetLoader.GetInstance.assetObjects["Live2D_haru"], new Vector3(1.0f, 2.8f, 4.0f), Quaternion.Euler(-90,0,0)) as GameObject;
        m_obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        
    }

    
}
