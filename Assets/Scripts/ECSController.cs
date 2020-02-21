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
    // Start is called before the first frame update
    public static void Start()
    {
       
    }
    public void Awake()
    {

      
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        sandArchetype = em.CreateArchetype(typeof(Translation),typeof(RenderMeshProxy),typeof(LocalToWorld));

        renderMesh.Value = GameObject.FindObjectOfType<RenderMeshProxy>().Value;
        createArena(50);
    }
    public void createArena(int cantidad)
    {


        float offset = 00f;
       
        for(int i=0; i<cantidad; i++)
        {
            
            Entity newEntityem = em.CreateEntity(sandArchetype);
            em.AddSharedComponentData(newEntityem, renderMesh.Value);
            em.SetComponentData(newEntityem, new Translation { Value = new float3(0,1+offset,0) });
            offset += 1;
        }


    }
    // Update is called once per frame
    void Update()
    {   
        
    }
}
