using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace GPUParticles
{
    [Obsolete]
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

        private Vector3 SolvedVel;

        bool velCol;

        // Start is called before the first frame update
        void Start()
        {
            particles = new ParticleGPU[maxParticles];
            for (int i = 0; i < particles.Length; i++)
            {
                Vector3 randompos = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
                particles[i] = new ParticleGPU(transform.position + randompos, -randompos,
                    Random.Range(minMass, maxMass));
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
            if (velCol)
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
            print("Solving Velocities");
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].velocity +=
                    ((Vector3.down * 9.8f * gravityMultiplier + (-particles[i].velocity * drag)) * dt);
            }

            velCol = !velCol;
        }

        void SolveCollisions()
        {
            print("Solving Collisions");
            for (int index = 1; index < particles.Length; index++)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    if ((particles[index].position - particles[i].position).magnitude < radius && index != i)
                    {
                        Debug.Log((particles[index].position - particles[0].position).magnitude < radius);
//                    Debug.LogFormat("Current Magnitude: {0}",(particles[0].position - particles[1].position ).magnitude);
//                    Debug.LogFormat("Current Velocity(1): {0},Current Velocity(2): {1}",particles[0].velocity,particles[1].velocity);
                        Collide(particles[index], particles[i]);
                    }
                }
            }
        }


        /// <summary>
        /// Assigns velocity to particle b from particle a's kinetic energy
        /// </summary>
        /// <param name="a">Query particle</param>
        /// <param name="b">Collided particle</param>
        void Collide(ParticleGPU a, ParticleGPU b)
        {
            Vector3 energyAone = (1 / 2) * a.mass * Vector3.Scale(a.velocity, a.velocity);
            Vector3 energyBtwo = (1 / 2) * b.mass * Vector3.Scale(b.velocity, b.velocity);
            Vector3 squaredVel = (a.mass / b.mass) * Vector3.Scale(a.velocity, a.velocity);

            b.velocity = new Vector3(Mathf.Sqrt(squaredVel.x), Mathf.Sqrt(squaredVel.y), Mathf.Sqrt(squaredVel.z));
            SolvedVel = b.velocity;
        }

        void Collide(ParticleGPU a, SurfaceCollider s)
        {
            Vector3 energyA = (1 / 2) * a.mass * -a.velocity;
            a.velocity = -a.velocity * 10;
        }

        private void OnDrawGizmos()
        {
            try
            {


                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(Vector3.zero, SolvedVel);
                Gizmos.DrawCube(SolvedVel, Vector3.one * 0.1f);
                foreach (var p in particles)
                {
                    Gizmos.color = visualizationGradient.Evaluate(p.velocity.magnitude / maxVelocity);
                    Gizmos.DrawSphere(p.position, radius * p.mass);
                    Gizmos.DrawWireSphere(p.position, radius);
                    Gizmos.DrawLine(p.position, p.position + p.velocity);
                }
            }
            catch (Exception error)
            {

            }
        }
    }

    public class ParticleGPU
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;
        public bool collided;

        public ParticleGPU(Vector3 _position, Vector3 _velocity, float _mass)
        {
            position = _position;
            velocity = _velocity;
            mass = _mass;
        }
    }
}
