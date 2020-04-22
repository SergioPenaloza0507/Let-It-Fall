using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Jobs;
using Unity.Physics;
public class SpawnSystem : MonoBehaviour
{

    public GameObject sandPrefab;
    public GameObject emisorPrefab;
    public GameObject receptorPrefab;
    [SerializeField] Mesh arenaMesh;
    public UnityEngine.Material[] colores;
    public int[] emisores ;
    public float tiempoEntreSpawns = 0.2f, offset = 1;
    Entity sandEntity,receptorEntity,emisorEntity;
    Entity [] emisorsEntities;
    EntityManager em;
    
    
    BlobAssetStore blobAssetStore;
    EntityArchetype sandArchetype;
    // Start is called before the first frame update
    void Start()
    {
        emisorsEntities = new Entity[emisores.Length];
        
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);


        sandArchetype = em.CreateArchetype(typeof(RenderMesh), typeof(Translation), typeof(Rotation),typeof(RenderBounds),typeof(LocalToWorld),typeof(ArenaComponent),typeof(GravityReceptorComponent));

        sandEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandPrefab, settings);

        receptorEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(receptorPrefab, settings);
        emisorEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(emisorPrefab,settings);
        for (int i=0; i< emisorsEntities.Length;i++) {

            emisorsEntities[i] = em.Instantiate(emisorEntity);
            em.AddComponentData(emisorsEntities[i], new EmisorComponent());
            
            em.SetComponentData(emisorsEntities[i], new Translation { Value = new float3(offset,0,0) });
            em.SetComponentData(emisorsEntities[i], new EmisorComponent {color=emisores[i], activo=true, arenaRestante=100});
            offset+=3;
        }
        Entity newReceptor = em.Instantiate(receptorEntity);
        em.AddComponentData(newReceptor, new SimpleCollisionDetector { collisionDetectionRadius=5 });
        em.AddComponentData(newReceptor, new ReceptorComponent { });
    }

    // Update is called once per frame
    float timer=0;
    void Update()
    {
        em.SetComponentData<Translation>(emisorsEntities[0], new Translation { Value = emisorPrefab.transform.position }) ;
        timer += Time.deltaTime;

        if (timer > tiempoEntreSpawns)
        {
            for (int i = 0; i < emisorsEntities.Length; i++)
            {
                if (em.GetComponentData<EmisorComponent>(emisorsEntities[i]).activo && em.GetComponentData<EmisorComponent>(emisorsEntities[i]).arenaRestante>0)
                {
                    int _color = em.GetComponentData<EmisorComponent>(emisorsEntities[i]).color;
                    Entity newArena = em.Instantiate(sandEntity);
                    PhysicsVelocity sandvelocity = em.GetComponentData<PhysicsVelocity>(newArena);
                    sandvelocity.Linear = Vector3.zero;
                    
                    em.SetComponentData(newArena,sandvelocity);
                    em.AddComponentData(newArena, new ArenaComponent { color = _color });
                    em.AddComponentData(newArena, new GravityReceptorComponent());
                    em.AddSharedComponentData(newArena, new RenderMesh { material = colores[_color], mesh = arenaMesh });
                    em.SetComponentData(newArena, new Translation { Value = em.GetComponentData<Translation>(emisorsEntities[i]).Value, });
                   
                    EmisorComponent emisor=  em.GetComponentData<EmisorComponent>(emisorsEntities[i]);
                    emisor.arenaRestante -= 1;
                    em.SetComponentData<EmisorComponent>(emisorsEntities[i], emisor);
                }
                timer = 0;
            }

           // arena.Dispose();
        }


        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
        {

            em.SetComponentData<EmisorComponent>(emisorsEntities[1], new EmisorComponent
            {
                activo = false
            });

        }
        
    }

    
}
