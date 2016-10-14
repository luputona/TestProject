using UnityEngine;
using System.Collections;

public class ReApplyShaders : MonoBehaviour {

    public MeshRenderer[] renderers;
    public Material[] materials;
    public string[] shaders;

    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    void Start()
    {
        
        for (int i = 0; i < renderers.Length; i++ )
        {
            materials = renderers[i].sharedMaterials;
            shaders = new string[materials.Length];

            for(int j = 0; j < materials.Length; j++)
            {
                shaders[i] = materials[i].shader.name;
            }
            for(int k = 0; k < materials.Length; k++)
            {
                materials[i].shader = Shader.Find(shaders[i]);
            }
        }

           
    }
}
