using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour {



    public void GoToScene(int _sceneNumber)
    {
        AssetLoader.GetInstance.m_sceneChek = _sceneNumber;
        SceneManager.LoadScene(_sceneNumber);
    }


	
}
