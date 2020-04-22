using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
public class ArenaReceptorCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;
    EntityCommandBufferSystem ecb;
    protected override void OnCreate()
    {

        ecb = World.GetOrCreateSystem<EntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    public struct CollisionDetection : ITriggerEventsJob
    {

        public EntityCommandBuffer _ecb;
        public ComponentDataFromEntity<ReceptorComponent> ReceptorGroup; 
        [ReadOnly] public ComponentDataFromEntity<ArenaComponent> ArenaGroup;
        public void Execute(TriggerEvent triggerEvent)
        {

            
                if (ReceptorGroup.HasComponent(triggerEvent.Entities.EntityA)) {
                if (ArenaGroup.HasComponent(triggerEvent.Entities.EntityB)) {
                    ReceptorComponent receptor = ReceptorGroup[triggerEvent.Entities.EntityA];
                    ArenaComponent arena = ArenaGroup[triggerEvent.Entities.EntityB];

                    receptor.arenaRecogida++;
                    ReceptorGroup[triggerEvent.Entities.EntityA] = receptor;
                  //  _ecb.AddComponent<DestroyThisEntity>(triggerEvent.Entities.EntityB);
                   
                    if (receptor.color==arena.color) {
                       // receptor.arenaRecogida++;
                        //ReceptorGroup[triggerEvent.Entities.EntityA] = receptor;
                    }
                                     
                   //  _ecb.AddComponent<DestroyThisEntity>(triggerEvent.Entities.EntityB);

                }
            }
            if (ReceptorGroup.HasComponent(triggerEvent.Entities.EntityB))
            {
                if (ArenaGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    
                    ReceptorComponent receptor = ReceptorGroup[triggerEvent.Entities.EntityB];
                    ArenaComponent arena = ArenaGroup[triggerEvent.Entities.EntityA];

                    receptor.arenaRecogida++;

                    ReceptorGroup[triggerEvent.Entities.EntityB] = receptor;
                    //_ecb.AddComponent<DestroyThisEntity>(triggerEvent.Entities.EntityA);
                    if (receptor.color == arena.color)
                    {
                       // receptor.arenaRecogida++;
                       // ReceptorGroup[triggerEvent.Entities.EntityB] = receptor;
                    }
                 
                      //  _ecb.AddComponent<DestroyThisEntity>(triggerEvent.Entities.EntityA);
                    
                  

                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        CollisionDetection colisionJob = new CollisionDetection
        {
            ReceptorGroup = GetComponentDataFromEntity<ReceptorComponent>(),
            ArenaGroup = GetComponentDataFromEntity<ArenaComponent>(),
            

            _ecb = ecb.CreateCommandBuffer(),

        };

        JobHandle job = colisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        job.Complete();
        return job;
       
    }
}

