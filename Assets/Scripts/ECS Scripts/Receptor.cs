using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;

public class Receptor : MonoBehaviour
{
    private BlobAssetStore blobAssetStore;
    EntityManager em;
    Entity receptor;
    // Start is called before the first frame update
    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        receptor = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, settings);
        em.AddComponentData(receptor,new ReceptorComponent{});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
