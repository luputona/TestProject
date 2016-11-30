using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static int m_sceneCheck;



    public void GoToScene(int _sceneNumber)
    {
        // AssetLoader.GetInstance.m_sceneChek = _sceneNumber;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(_sceneNumber);
        m_sceneCheck = _sceneNumber;
    }

    public void GoToScene()
    {
        // AssetLoader.GetInstance.m_sceneChek = _sceneNumber;
        SceneManager.LoadScene(m_sceneCheck);     
         
    }

    public static void SelectField(int _fieldnumber)
    {
        m_sceneCheck = _fieldnumber;
    }

    public void CompleteDownload(int _sceneNumber)
    {
        if(AssetLoader.GetInstance.m_downCheck)
        {
            SceneManager.LoadScene(_sceneNumber);
        }
        
    }
	
}
