using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;

[UpdateAfter(typeof(CollisionDetectionOnGravitationalSystem))]
public class ApplyGravitationalForcesSystem : ComponentSystem
{
    public static Dictionary<Entity, float3> toMove = new Dictionary<Entity, float3>();
    
    private BuildPhysicsWorld buildPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }
    
    protected override void OnUpdate()
    {
        foreach (var tm in toMove)
        {
            int index = buildPhysicsWorld.PhysicsWorld.GetRigidBodyIndex(tm.Key);
            buildPhysicsWorld.PhysicsWorld.ApplyLinearImpulse(index,tm.Value);
            Debug.Log($"Applied ({tm.Value}) Impuse to: {tm.Key.Index}");
        }
        Debug.Log($"System is executing,impulseappliers:  {toMove.Count}");
        toMove.Clear();
    }
}
