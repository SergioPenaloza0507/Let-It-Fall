using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnController : MonoBehaviour
{
    
    public GameObject sandPrefab;
    public Transform emisor;
    public GameObject receptorPrefab;
    public float tiempoEntreSpawns=0.2f;
    Entity sandEntity,receptorEntity;
    EntityManager em;
    List<Entity> arena;
    BlobAssetStore blobAssetStore;
    // Start is called before the first frame update
    void Start()
    {
        arena = new List<Entity>();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        sandEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandPrefab, settings);
        receptorEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(receptorPrefab, settings);

        Entity newReceptor = em.Instantiate(receptorEntity);
        em.AddComponentData(newReceptor, new ReceptorComponent { });
    }

    // Update is called once per frame
    float timer=0;
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer > tiempoEntreSpawns) {
            Entity newArenaEntity = em.Instantiate(sandEntity);
            arena.Add(newArenaEntity);
            Debug.Log("spawned");
            timer = 0;
            em.SetComponentData(newArenaEntity, new Translation
            {

                Value = new float3(emisor.position.x, emisor.position.y, emisor.position.z)
            });
            em.AddComponentData(newArenaEntity, new ArenaComponent { });
        }

        if (Input.GetKeyDown(KeyCode.S)) {

            /* foreach (Entity e in arena) {

                 em.DestroyEntity(e);
             }
             arena.Clear();*/
        }
    }
}
