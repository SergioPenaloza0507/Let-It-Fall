﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class ObstacleCreator : MonoBehaviour
{
    private static ObstacleCreator instance;
    
    public event Action<Vector3[]> OnPathUpdated;
    
    [Header("Path parameters")]
    [SerializeField] private float spacing = 0.1f;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] [Range(0,1)] private float maxAngleThreshold = 0.6f;
    
    [Header("Effective Physics Parameters")]
    [SerializeField] private GameObject gravityPointPrefab;
    [SerializeField] [Range(.1f, 1)] private float gravitationalClusterDensity;
    [SerializeField] [Range(.01f, 1)] private float gravitationalClusterScaleMultiplier;
    
    [Header("Mesh parameters")]
    [SerializeField] private float extrusion = 0.1f;
    [SerializeField] private float thickness = 0.1f;
    [SerializeField] private Material meshMaterial = null;
    
    

    private List<Vector3> vertices;
    private List<Vector3> normals;
    
    int obstacleIndex;
    private void Awake()
    {
        GetInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        InputHandler.Instance.OnTap += FirstVertex;
        InputHandler.Instance.OnSingleHold += AddVertex;
        InputHandler.Instance.OnRelease += CreateObstacle;

        vertices = new List<Vector3>();
        normals = new List<Vector3>();
    }

    private void OnDestroy()
    {
        InputHandler.Instance.OnTap -= FirstVertex;
        InputHandler.Instance.OnSingleHold -= AddVertex;
        InputHandler.Instance.OnRelease -= CreateObstacle;
    }
    void FirstVertex(Vector2 touchpos)
    {
        if (vertices.Count <= 0)
        {
            RaycastHit h = new RaycastHit();

            if (CastRayFromCam(touchpos, out h))
            {
                vertices.Add(h.point);
            }
        }
    }

    void AddVertex(Vector2 touchpos)
    {
        if (vertices.Count > 0)
        {
            RaycastHit h = new RaycastHit();
            
            if (CastRayFromCam(touchpos, out h))
            {
                if ((h.point - vertices[vertices.Count - 1]).magnitude >= spacing)
                {
                    if (vertices.Count > 2)
                    {
                        if (Vector3.Dot((vertices[vertices.Count - 1] - vertices[vertices.Count - 2]).normalized,
                                (vertices[vertices.Count - 2] - vertices[vertices.Count - 3]).normalized) >
                            maxAngleThreshold)
                        {
                            vertices.Add(h.point);
                        }
                        if (normals.Count < vertices.Count)
                        {
                            normals.Add(Vector3.Cross((vertices[vertices.Count - 1] - vertices[0]).normalized, Vector3.forward));
                        }
                        normals.Add(Vector3.Cross(vertices[vertices.Count - 1] - vertices[vertices.Count - 2],
                            Vector3.forward).normalized);
                    }
                    else
                    {
                        vertices.Add(h.point);
                        if (normals.Count < vertices.Count)
                        {
                            normals.Add(Vector3.Cross((vertices[vertices.Count - 1] - vertices[0]).normalized, Vector3.forward));
                        }
                        normals.Add(Vector3.Cross(vertices[vertices.Count - 1] - vertices[vertices.Count - 2],
                            Vector3.forward).normalized);
                    }
                }
            }
            OnPathUpdated?.Invoke(vertices.ToArray());
        }
    }

    void CalculateNormals()
    {
        //1. Find the bisector of the gratest angle
        //2. Create a normal vector from that bisector
        //3. Add the normal vector to the arrray
        
        normals.Add(Vector3.Cross((vertices[1]-vertices[0]).normalized,Vector3.forward));
        
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            Vector3 lastSegment = (vertices[i] - vertices[i - 1]).normalized;
            Vector3 nextSegment = (vertices[i + 1] - vertices[i]).normalized;
            normals.Add((lastSegment + nextSegment).normalized);
        }
        
        normals.Add(Vector3.Cross((vertices[vertices.Count-1]-vertices[vertices.Count-2]).normalized,Vector3.forward));
    }

    void CreateObstacle()
    {
        if (vertices.Count > 1)
        {
            
            GameObject g = new GameObject(String.Format("OBSTACLE ({0})", obstacleIndex));
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            MeshFilter mf = g.AddComponent<MeshFilter>();

            Rigidbody r = g.AddComponent<Rigidbody>();

            Mesh generated =
                MeshGenerator.GenerateFromObstacle(vertices.ToArray(), normals.ToArray(), extrusion, thickness);

            mf.mesh = generated;
            

            mr.sharedMaterial = meshMaterial;
            r.isKinematic = true;
            Obstacle o = g.AddComponent<Obstacle>();
            o.order = obstacleIndex;
            obstacleIndex++;
        }

        
        for (int i = 0; i < vertices.Count; i++)
        {
            try
            {
                
                Vector3 v = vertices[i];
                Vector3 v1 = Vector3.one;
                if (i > 0)
                {
                    v1 = (vertices[i + 1]);
                }
                if(i >= vertices.Count - 1)
                {
                    v1 = v + (v - vertices[i - 1]);
                }

                GameObject g = Instantiate(gravityPointPrefab, vertices[i * (int) (1 / gravitationalClusterDensity)],
                    Quaternion.identity);

                GravitationalClusterHybridHandler gh = g.GetComponent<GravitationalClusterHybridHandler>();
                var gravityClusterComponent = gh.Value;
                gravityClusterComponent.biasedPointTarget = new float3(v1.x,v1.y,v1.z);
                gh.Value = gravityClusterComponent;

                g.AddComponent<ConvertToEntity>();
            }
            catch (Exception error)
            {
                
            }
        }
        
        vertices.Clear();
        normals.Clear();
    }

    bool CastRayFromCam(Vector2 touchPos,out RaycastHit h)
    {
        Ray r = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit = new RaycastHit();
        
        if (Physics.Raycast(r, out hit, Mathf.Infinity, collisionLayer))
        {
            h =  hit;
            return true;
        }

        h = hit;
        return false;
    }

    public static ObstacleCreator Instance => instance;

    void GetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(vertices[0], 0.1f);
            Gizmos.DrawLine(vertices[0], vertices[0] + normals[0]);
            for (int i = 1; i < vertices.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vertices[i], 0.1f);
                
                Gizmos.DrawLine(vertices[i-1], vertices[i]);
                
                Gizmos.color = Color.red;
                Gizmos.DrawLine(vertices[i], vertices[i] + normals[i]);
            }
        }
        catch (System.Exception error)
        {
            
        }
    }
#endif
}
