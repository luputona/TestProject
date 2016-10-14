using UnityEngine;
using System.Collections;

public class ApplyShader : MonoBehaviour 
{
    void Start()
    {
        if (this.gameObject.GetComponents<ApplyShader>().Length > 1)
            Destroy(this);

        //if (AssetBundleManager.Instance == null)
        //    return;

        //if (AssetManager.Instance.loadAssetType != AssetManager.LoadAssetType.BundleRemote &&
        //    AssetManager.Instance.loadAssetType != AssetManager.LoadAssetType.BundleLocal)
        //    return;

        if (Application.platform != RuntimePlatform.WindowsEditor &&
            Application.platform != RuntimePlatform.OSXEditor)
            return;

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.AddComponent<ApplyShader>();
            }
        }
        if (transform.GetComponent<Renderer>() == null)
        {
            return;
        }

        setShader(transform.GetComponent<Renderer>());
    }

    public static void setShader(Renderer thisRenderer)
    {
        Material[] thisMaterial = thisRenderer.sharedMaterials;
        string[] shaders = new string[thisMaterial.Length];

        for (int i = 0; i < thisMaterial.Length; i++)
        {
            if (thisMaterial[i] != null)
                shaders[i] = thisMaterial[i].shader.name;
        }

        for (int i = 0; i < thisMaterial.Length; i++)
        {
            if (thisMaterial[i] != null)
                thisMaterial[i].shader = Shader.Find(shaders[i]);
        }
    }
	
}
