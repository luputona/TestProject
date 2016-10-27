using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static int m_sceneCheck;



    public void GoToScene(int _sceneNumber)
    {
       // AssetLoader.GetInstance.m_sceneChek = _sceneNumber;
        SceneManager.LoadScene(_sceneNumber);
        m_sceneCheck = _sceneNumber;
    }


	
}
