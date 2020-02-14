using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ObstacleCreator : MonoBehaviour
{
    [Header("Path parameters")]
    [SerializeField] private float zValue;
    [SerializeField] private int vertexDensity;
    [SerializeField] private float newVertexDelta;
    [SerializeField] private LayerMask collisionLayer;
    
    [Header("Mesh parameters")]
    [SerializeField] private float extrusion;
    [SerializeField] private float thickness;
    [SerializeField] private Material meshMaterial;

    
    
    
    [SerializeField]private List<Vector3> vertices;
    private List<Vector3> normals;
    
    int obstacleIndex;
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

    // Update is called once per frame
    void Update()
    {
        
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
                if ((h.point - vertices[vertices.Count - 1]).magnitude >= newVertexDelta)
                {
                    vertices.Add(h.point);
                    if (normals.Count < vertices.Count)
                    {
                        normals.Add(Vector3.Cross(vertices[vertices.Count - 1] - vertices[0], Vector3.forward));
                    }

                    normals.Add(Vector3.Cross(vertices[vertices.Count - 1] - vertices[vertices.Count - 2],Vector3.forward).normalized);
                }
            }
        }
    }

    void CreateObstacle()
    {
        if (vertices.Count > 1)
        {
            GameObject g = new GameObject(String.Format("OBSTACLE ({0})", obstacleIndex));
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            MeshFilter mf = g.AddComponent<MeshFilter>();
            MeshCollider col = g.AddComponent<MeshCollider>();

            Mesh generated =
                MeshGenerator.GenerateFromObstacle(vertices.ToArray(), normals.ToArray(), extrusion, thickness);

            mf.mesh = generated;

            col.sharedMesh = generated;

            mr.sharedMaterial = meshMaterial;
        }
        
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
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
