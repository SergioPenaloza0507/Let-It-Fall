using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
public class SimpleCollisionDetectionSystem :JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem ecbSystem;
    EntityQuery arenaGroup, receptorGroup;
    EntityManager em;
    protected override void OnCreate()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        arenaGroup= GetEntityQuery(typeof(ArenaComponent),typeof(Translation));
        receptorGroup = GetEntityQuery(typeof(ReceptorComponent), typeof(Translation));
        
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        foreach (Entity e in arenaGroup.ToEntityArray(Unity.Collections.Allocator.TempJob)) {

            Translation ta= em.GetComponentData<Translation>(e);
            Translation tr= em.GetComponentData<Translation>(receptorGroup.ToEntityArray(Unity.Collections.Allocator.TempJob)[0]);
            ReceptorComponent r= em.GetComponentData<ReceptorComponent>(receptorGroup.ToEntityArray(Unity.Collections.Allocator.TempJob)[0]);
            SimpleCollisionDetector cd= em.GetComponentData<SimpleCollisionDetector>(receptorGroup.ToEntityArray(Unity.Collections.Allocator.TempJob)[0]);
            float distance = Vector3.Distance(ta.Value, tr.Value);

            if (distance < cd.collisionDetectionRadius) {
                em.AddComponentData<DestroyThisEntity>(e, new DestroyThisEntity { });
                r.arenaRecogida++;
                em.SetComponentData<ReceptorComponent>(receptorGroup.ToEntityArray(Unity.Collections.Allocator.TempJob)[0],r);
            }
         
            
        }
        

     

       


        return default;


    }
}
