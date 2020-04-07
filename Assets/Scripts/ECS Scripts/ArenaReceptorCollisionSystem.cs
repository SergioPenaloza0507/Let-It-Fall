using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
public class ArenaReceptorCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    public struct CollisionDetection : ITriggerEventsJob
    {
       
        
        public ComponentDataFromEntity<ReceptorComponent> ReceptorGroup;
        [ReadOnly]public ComponentDataFromEntity<ArenaComponent> ArenaGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            if (ReceptorGroup.HasComponent(triggerEvent.Entities.EntityA)) {
                if (ArenaGroup.HasComponent(triggerEvent.Entities.EntityB)) {
                    ReceptorComponent receptor = ReceptorGroup[triggerEvent.Entities.EntityA];
                    ArenaComponent arena = ArenaGroup[triggerEvent.Entities.EntityB];
                    if(receptor.color==arena.color) {
                        receptor.arenaRecogida++;
                        ReceptorGroup[triggerEvent.Entities.EntityA] = receptor;
                    }
                    
                   
                    DestroyEntitiesSystem.EntitiesToDestroy.Add(triggerEvent.Entities.EntityB);

                    

                }
            }
            if (ReceptorGroup.HasComponent(triggerEvent.Entities.EntityB))
            {
                if (ArenaGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    
                    ReceptorComponent receptor = ReceptorGroup[triggerEvent.Entities.EntityB];
                    ArenaComponent arena = ArenaGroup[triggerEvent.Entities.EntityA];
                    if (receptor.color == arena.color)
                    {
                        receptor.arenaRecogida++;
                        ReceptorGroup[triggerEvent.Entities.EntityB] = receptor;
                    }
                    DestroyEntitiesSystem.EntitiesToDestroy.Add(triggerEvent.Entities.EntityA);

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



        };

        return colisionJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld,inputDeps) ;
    }
}
