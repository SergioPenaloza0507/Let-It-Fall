using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class ParticleTest : MonoBehaviour
{

    [SerializeField] private int maxParticles;
    [SerializeField] private float dt;
    [SerializeField] private float minMass;
    [SerializeField] private float maxMass;
    [SerializeField] private float maxVelocity = 20;
    [SerializeField] private float drag;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float radius;
    
    [SerializeField] Gradient visualizationGradient;

    private ParticleGPU[] particles;
    [SerializeField] private SurfaceCollider[] colliders;

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        particles = new ParticleGPU[maxParticles];
        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 randompos = new Vector3( Random.Range(-3,3),Random.Range(-3,3),0);
            particles[i] = new ParticleGPU(transform.position + randompos,Vector3.zero, Random.Range(minMass,maxMass));
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > dt)
        {
            UpdateSolver();
            time = 0;
        }
    }

    void UpdateSolver()
    {
        SolveCollisions();
        SolveVelocities();

        SolvePositions();
    }

    void SolvePositions()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = particles[i].position + particles[i].velocity * dt;
        }
    }
    void SolveVelocities()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity +=
                                    ((Vector3.down * 9.8f * gravityMultiplier + (-particles[i].velocity * drag)) * dt);

            particles[i].velocity = Vector3.ClampMagnitude(particles[i].velocity, maxVelocity);
        }
    }
    
    void SolveCollisions()
    {
        int index = 0;
        for (index = 0; index < particles.Length; index++)
        {
            foreach (var p in particles)
            {
                if (Vector3.Distance(particles[index].position, p.position) < radius)
                {
                    Collide(particles[index],p);
                }
            }

            foreach (var s in colliders)
            {
                for (int i = 1; i < s.Definition.Length; i++)
                {
                    if ((s.Definition[i - 1] - s.Definition[i]).x < radius &&
                        (s.Definition[i - 1] - s.Definition[i]).y < radius)
                    {
                        print("Collided");
                        Collide(particles[i],s);
                        break;
                    }
                }
            }
        }
    }

    void Collide(ParticleGPU a, ParticleGPU b)
    {
        Vector3 energyA = (1/2) * a.mass * a.velocity;
        Vector3 energyB = (1/2) * b.mass * b.velocity;
        Vector3 temp = energyA;
        energyA += -energyB;
        energyB += -temp;
        Vector3 ta = (2 * energyA) / a.mass;
        Vector3 tb = (2 * energyB) / b.mass;
        b.velocity = new Vector3(Random.Range(-radius,radius),Random.Range(-radius,radius),Random.Range(-radius,radius)) * 10;
    }
    void Collide(ParticleGPU a, SurfaceCollider s)
    {
        Vector3 energyA = (1/2) * a.mass * -a.velocity;
        a.velocity = -a.velocity * 10;
    }
    private void OnDrawGizmos()
    {
        try
        {
            foreach (var p in particles)
            {
                Gizmos.color = visualizationGradient.Evaluate( p.velocity.magnitude / maxVelocity);
                Gizmos.DrawSphere(p.position, radius * p.mass);
                Gizmos.DrawLine(p.position,p.position  + p.velocity.normalized);
            }
        }
        catch (Exception error)
        {
            
        }
    }
}

public struct ParticleGPU
{
    public Vector3 position;
    public Vector3 velocity;
    public float mass;

    public ParticleGPU(Vector3 _position, Vector3 _velocity,float _mass)
    {
        position = _position;
        velocity = _velocity;
        mass = _mass;
    }
        
        
    public static bool operator ==(ParticleGPU a, ParticleGPU b)
    {
        return a.GetHashCode() == b.GetHashCode() && a.position == b.position && a.velocity == b.velocity;
    }

    public static bool operator !=(ParticleGPU a, ParticleGPU b)
    {
        return a.GetHashCode() != b.GetHashCode() || a.position != b.position || a.velocity != b.velocity;
    }
}
