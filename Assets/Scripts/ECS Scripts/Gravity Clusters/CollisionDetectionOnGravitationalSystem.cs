using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Collections;

//[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(BuildPhysicsWorld)), UpdateBefore(typeof(StepPhysicsWorld))]
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

        public PhysicsWorld pw;
        
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
                
                if (receptorGroup.Exists(A) && positionGroup.Exists(A) && massGroup.Exists(A))
                {
                    GravityClusterComponent ACluster = clusterGroup[B];
                    float3 Bposition = positionGroup[A].Position;
                    float3 Aposition = positionGroup[B].Position;
                    PhysicsMass BMass = massGroup[A];

                    float3 dir = Bposition - Aposition;
                    float radiusSquared = (dir.x * dir.x) + (dir.y * dir.y) +
                                          (dir.z * dir.z);
                    float3 calculatedPullForce = new float3(0, 0, 0);
                    
                    calculatedPullForce = -(dir/Mathf.Sqrt(radiusSquared)) * ((GRAVITATIONAL_CONSTANT * (ACluster.mass * BMass.InverseMass))/radiusSquared);

                    try
                    {
                        pw.ApplyLinearImpulse(pw.GetRigidBodyIndex(A), calculatedPullForce);
                    }
                    catch (Exception error)
                    {
                        
                    }
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deps = JobHandle.CombineDependencies(inputDeps, buildPhysicsWorld.FinalJobHandle);
        
        JobHandle job = new CollissionDetectionGravitationalRadius()
        {
            pw = buildPhysicsWorld.PhysicsWorld,
            clusterGroup = GetComponentDataFromEntity<GravityClusterComponent>(),
            receptorGroup = GetComponentDataFromEntity<GravityReceptorComponent>(),
            positionGroup = GetComponentDataFromEntity<LocalToWorld>(),
            massGroup = GetComponentDataFromEntity<PhysicsMass>()
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, deps);
        
        job.Complete();
        return job;
    }
}
