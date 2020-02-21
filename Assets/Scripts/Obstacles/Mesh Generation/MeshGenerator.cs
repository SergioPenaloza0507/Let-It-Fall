using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MeshGenerator : MonoBehaviour
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public static Mesh GenerateFromObstacle(Vector3[] positions, Vector3[] normals,float extrusion,float thickness)
    {
        Mesh m = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        

        for (int i = 0; i < positions.Length; i++)
        {
            vertices.Add(positions[i] + normals[i] * thickness);
            vertices.Add(positions[i] + -normals[i] * thickness);
        }

        bool reverseTriangle = false;
        for (int i = 1; i < vertices.Count - 1; i+=1)
        {
            if (!reverseTriangle)
            {
                triangles.Add(i - 1);
                triangles.Add(i);
                triangles.Add(i + 1);
            }
            else
            {
                triangles.Add(i + 1);
                triangles.Add(i);
                triangles.Add(i - 1);
            }

            reverseTriangle = !reverseTriangle;
            print(String.Format("New Triangle Added! : ({0},{1},{2})",i-1,i,i+1));
        }

        print("triangles: " + triangles.Count);
        print("vertices: " + vertices.Count);
        m.SetVertices(vertices);
        m.SetTriangles(triangles.ToArray(),0);
        string meshName = "";
        Random r = new Random((int)Time.time);
        for (int i = 0; i < 8; i++)
        {
            meshName += chars[r.Next(0, chars.Length)];
        }

        m.name =String.Format("mesh ID {0}{1}",r.Next(1000,9999).ToString(),meshName);
        return m;
    }
}
