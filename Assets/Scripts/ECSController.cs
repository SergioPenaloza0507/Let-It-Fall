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
    public GameObject arena;
    public Mesh render;
    public Material mat;
    static EntityManager em;
    static EntityArchetype sandArchetype;
    // Start is called before the first frame update
    public static void Start()
    {
        
    }
    public void Awake()
    {
        Debug.Log("des");
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        // sandArchetype = em.CreateArchetype(typeof(Translation),typeof(RenderMesh),typeof(LocalToWorld));

        createArena(5);
    }
    public void createArena(int cantidad)
    {
        NativeArray<Entity> newArena;
        float offset = 00f;
        for(int i=0; i<cantidad; i++)
        {
            Entity newEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(arena, World.DefaultGameObjectInjectionWorld);

            em.SetSharedComponentData(newEntity, new RenderMesh
            {
                mesh = render,
                material = mat,

            });
            em.SetComponentData(newEntity, new Translation
            {
                Value = new float3(offset, 0, 0),

            });
            offset += 1f;
        }


    }
    // Update is called once per frame
    void Update()
    {   
        
    }
}
