using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SimulationSettings : ScriptableObject
{
    [Header("General Settings")] 
    public float deltaTime;
    
    [Header("Particle Settings")]
    public int maxParticles;
    public float particleRadius;
    public float gravityMultiplier;
    
    [Header("Eulerian Box Settings")]
    public float density;
    
}
