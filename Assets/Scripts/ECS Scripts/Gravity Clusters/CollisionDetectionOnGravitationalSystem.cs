using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionDetectionOnGravitationalSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    const float GRAVITATIONAL_CONSTANT = 6.67408f;
    
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    public struct CollissionDetectionGravitationalRadius : ITriggerEventsJob
    {
        public ComponentDataFromEntity<GravityClusterComponent> clusterGroup;
        [ReadOnly]public ComponentDataFromEntity<GravityReceptorComponent> receptorGroup;

        public ComponentDataFromEntity<LocalToWorld> positionGroup;
        public ComponentDataFromEntity<PhysicsMass> massGroup;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity A = triggerEvent.Entities.EntityA;
            Entity B = triggerEvent.Entities.EntityB;
            
            
            
            if (clusterGroup.Exists(B) && positionGroup.Exists(B))
            {
                Debug.Log($"Entity A idex : {A.Index}, Entity B index: {B.Index}");
                if (receptorGroup.Exists(A) && positionGroup.Exists(A) && massGroup.Exists(A))
                {
                    
                    GravityClusterComponent ACluster = clusterGroup[B];
                    float3 Bposition = positionGroup[A].Position;
                    float3 Aposition = positionGroup[B].Position;
                    PhysicsMass BMass = massGroup[A];
                    float radiusSquared = (Bposition.x - Aposition.x) + (Bposition.y - Aposition.y) +
                                          (Bposition.z - Aposition.z);
                    float3 calculatedPullForce = new float3(0, 0, 0);
                    calculatedPullForce = GRAVITATIONAL_CONSTANT * ((ACluster.mass * BMass.InverseMass)/radiusSquared) + ACluster.biasedDirection;
                    
                    ApplyGravitationalForcesSystem.toMove.Add(A,calculatedPullForce);
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        CollissionDetectionGravitationalRadius job = new CollissionDetectionGravitationalRadius()
        {
            clusterGroup = GetComponentDataFromEntity<GravityClusterComponent>(),
            receptorGroup = GetComponentDataFromEntity<GravityReceptorComponent>(),
            positionGroup = GetComponentDataFromEntity<LocalToWorld>(),
            massGroup = GetComponentDataFromEntity<PhysicsMass>()
        };

        return job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }
}
