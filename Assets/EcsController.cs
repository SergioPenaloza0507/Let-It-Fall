using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
public class EcsController : MonoBehaviour
{
    public Mesh renderMesh;
    public UnityEngine.Material mat;
    public GameObject objeto;
    EntityManager em;
    private BlobAssetStore blobAssetStore;
    // Start is called before the first frame update
    void Start()
    {
        blobAssetStore = new BlobAssetStore();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        int offset=0;
        for(int i=0;i<500;i++)
        {
            Entity newent = GameObjectConversionUtility.ConvertGameObjectHierarchy(objeto,settings);
            em.SetComponentData(newent, new Translation
            {
                Value = new float3(offset,0,0)
            });
         offset++;
        }
    }
  
    // Update is called once per frame
    void Update()
    {
       
    }
}
