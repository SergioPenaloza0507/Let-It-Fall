using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine.SceneManagement;
using Unity.Transforms;
public class DestroyArena : JobComponentSystem
{// job para destruir las entidades con el componente DestryThisEntity
    EndSimulationEntityCommandBufferSystem ecbSystem;
    public static bool levelRestarted;
    float t=0;
    protected override void OnCreate()
    {

        
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        
        Entities.WithoutBurst().WithAll<DestroyThisEntity>().ForEach((Entity entity) =>
        {
            ecb.RemoveComponent<ArenaComponent>(entity);
            ecb.SetSharedComponent<RenderMesh>(entity, new RenderMesh
            {
                material = null
            });


        }).Run();




        if (levelRestarted) { 

            Entities.WithoutBurst().WithAll<Translation>().ForEach((Entity entity) =>
            {
               
                ecb.DestroyEntity(entity);


            }).Run();
            
          //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        return default;
       

    }
}
