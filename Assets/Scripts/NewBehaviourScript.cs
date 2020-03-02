using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
public class TriggerSystem : JobComponentSystem
{
    public EntityManager em;
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    protected override void OnCreate()
    {
       em= World.DefaultGameObjectInjectionWorld.EntityManager;
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        base.OnCreate();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob collisiones = new TriggerJob
        {
           
            arena = GetComponentDataFromEntity<ArenaTag>(),
            Receptor = GetComponentDataFromEntity<ReceptorTag>(),
        };
        return collisiones.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }
    private struct TriggerJob : ITriggerEventsJob
    {
        
        [ReadOnly]
        public ComponentDataFromEntity<ArenaTag> arena;
        [ReadOnly]
        public ComponentDataFromEntity<ReceptorTag> Receptor;
        public void Execute(TriggerEvent triggerEvent)
        {
            if(arena.HasComponent(triggerEvent.Entities.EntityA) && Receptor.HasComponent(triggerEvent.Entities.EntityB))
            {
                Debug.Log("colision");
            }
            if (arena.HasComponent(triggerEvent.Entities.EntityB) && Receptor.HasComponent(triggerEvent.Entities.EntityA))
            {
                Debug.Log("colision");

            }

        }
    }

    // Start is called before the first frame update
    
}
