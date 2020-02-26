using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Collections;
public class ECSController : MonoBehaviour
{
    static RenderMeshProxy renderMesh;
    
    static EntityManager em;
    static EntityArchetype sandArchetype;
    float timer;
    public float tiempoEntreArena,cantidadGranos;
    BlobAssetStore blobAssetStore;
    public GameObject ArenaPrefab;
    Entity granoArena;
   
    // Start is called before the first frame update
    public static void Start()
    {
       
    }
    public void Awake()
    {
        blobAssetStore = new BlobAssetStore();
       GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld,blobAssetStore);
          em = World.DefaultGameObjectInjectionWorld.EntityManager;
        granoArena = GameObjectConversionUtility.ConvertGameObjectHierarchy(ArenaPrefab,settings);
      
       
    }
    int arrayPositioner=0;
    NativeArray<Entity> granos;
    public void createArena()
    {


        granos[arrayPositioner] = em.Instantiate(granoArena);
        arrayPositioner++;
        



    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer>tiempoEntreArena && arrayPositioner<cantidadGranos)
        {

            createArena();

        }
    }
}
