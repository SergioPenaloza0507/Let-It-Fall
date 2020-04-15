using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct GravityClusterComponent : IComponentData
{
    public float3 biasedDirection;
    public float mass;
}
