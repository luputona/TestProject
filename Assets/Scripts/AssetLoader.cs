﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;


public class AssetLoader : Singleton<AssetLoader> 
{
    public string url;
    public string monsterurl;
    public Dictionary<string, GameObject> assetObjects = new Dictionary<string, GameObject>();
    //public List<GameObject> assetObjects_list = new List<GameObject>();
    public List<GameObject> monster_list = new List<GameObject>();
    public string[] m_names;
    public string[] m_monstersnames;

    public int m_sceneChek;
    public bool m_downCheck =false;

   // public AssetBundle bundleManifest;

    public Text m_caching;
    public Text m_progress;
    public Text m_downloading;
    private float m_currentValue = 0.0f;
    private int m_assetLength;
    private int m_assetCount = 0;
    private float tempPercentage;
    private float totalPercentage;

    Hash128 hash;
    Random ran = new Random();
    void Awake()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }
        GPGSMgr.GetInstance.InitializeGPGS();
    }

    void Start()
    {        
        
        StartCoroutine(InitAssetBundle());
        

        
    }
    void Update()
    {
        
        
        //Debug.Log("tempPercentage : " + tempPercentage);
        
        if(m_progress != null)
        {
            m_progress.text = string.Format("Asset : {0:0}%", m_currentValue);
        }
        if(m_downloading != null)
        {
            m_downloading.text = string.Format("Total : {0:0}%", totalPercentage);
        }
            
        
        //Debug.Log("length : " + m_assetLength);
        //Debug.Log("count : " + m_assetCount);
        //Debug.Log(m_downCheck);
    }
   
	// Use this for initialization
    IEnumerator InitAssetBundle()
    {
        WWW www = new WWW(url + "Android");
        
        yield return www;
        
        if(www.error != null)
        {            
            Debug.Log("www error :" + www.error);
            yield break;
        }
        

        // 메니페스트 얻기
        AssetBundle bundleManifest = www.assetBundle;
        www.Dispose();
        AssetBundleManifest assetBundleManifest = bundleManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        // 메니페스트가 가지고 있는 모든 어셋번들 이름을 가져옵니다.
        // 여기에서는 두개가 나오겠죠.
        string[] assetBundleNames = assetBundleManifest.GetAllAssetBundles();
        m_assetLength = assetBundleNames.Length;
        // 어셋번들 갯수만큼 돌면서 게임오브젝트를 만들겠습니다.
        for (int i = 0; i < assetBundleNames.Length; i++)
        {
            string assetBundleName = assetBundleNames[i];
            // 이미 캐쉬되어있는지 확인
            bool bCaching = Caching.IsVersionCached(url + assetBundleName, assetBundleManifest.GetAssetBundleHash(assetBundleName));
            // 로그 찍어본다.
            //Debug.Log(assetBundleName + " Cash : " + bCaching.ToString());
            

            // 어셋번들을 다운로드 합니다.
            // 저는 URL과 해쉬값으로 로드하겠습니다.(해쉬값은 메니페스트에 있습니다.)
            using ( www = WWW.LoadFromCacheOrDownload(url + assetBundleName, assetBundleManifest.GetAssetBundleHash(assetBundleName)))
            {
                hash = assetBundleManifest.GetAssetBundleHash(assetBundleName);

                while (!www.isDone)
                {
                    m_currentValue = www.progress * 100;                                    
                    //m_progress.text = string.Format("progress :{0}%", m_currentValue);
                    //Debug.Log("progress : " + www.progress);
                    yield return null;                                       
                }
                if (www.progress == 1)
                {
                    ++m_assetCount;
                }
                tempPercentage = (float)m_assetCount / (float)m_assetLength;
                //Debug.Log("m_assetCount : "+ m_assetCount);
                totalPercentage = tempPercentage * 100;

                m_currentValue = 100f;

                //yield return www;

                AssetBundle bundle = www.assetBundle;

                string[] names = bundle.GetAllAssetNames();
                //에디터상에서 보기
                m_names = names;
                
                for (int j = 0; j < names.Length; j++)
                {
                    // 하나의 어셋번들이 가지고 있는 리스트롤 로그에 찍을꺼에요.
                    //Debug.Log("name:" + names[j]);

                    // GameObject인 것만 생성(여기에서는 프리팹만 나오겠죠)
                    GameObject gObj = bundle.LoadAsset<GameObject>(names[j]);
                    
                    if (gObj != null)
                    {                     
                        if(gObj.CompareTag("Monster"))
                        {
                            monster_list.Add(gObj);
                        }
                        else
                        {
                            assetObjects.Add(gObj.name, gObj.gameObject);
                        }
                            
                    }

                    //foreach (KeyValuePair<string, GameObject> element in assetObjects)
                    //{
                    //    Debug.Log("key : " + element.Key + " / value : " + element.Value);
                    //}
                    //for (int a = 0; a < monster_list.Count; a++)
                    //{
                    //    Debug.Log("List name : " + monster_list[a].name );                        
                    //}
                }
               
                m_caching.text = string.Format("{0} : Cach : {1}", assetBundleName, bCaching.ToString());
                if (m_assetLength == m_assetCount)
                {
                    m_downCheck = true;
                    //GPGSMgr.GetInstance.LoginGPGS();
                }
                else
                {
                    m_downCheck = false;

                }

                // 번들을 언로드해줍니다.
                bundle.Unload(false);
                www.Dispose();
                
            }
           
        }
        

    }     
}
