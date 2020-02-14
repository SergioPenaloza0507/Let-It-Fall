using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawingFeedback : MonoBehaviour
{
    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        ObstacleCreator.Instance.OnPathUpdated += UpdateVertices;
        InputHandler.Instance.OnRelease += ClearVertices;
    }

    private void OnDestroy()
    {
        ObstacleCreator.Instance.OnPathUpdated -= UpdateVertices;
        InputHandler.Instance.OnRelease -= ClearVertices;
    }

    void UpdateVertices(Vector3[] vertices)
    {
        lr.positionCount = vertices.Length;
        lr.SetPositions(vertices);
    }

    void ClearVertices()
    {
        lr.positionCount = 0;
    }
}
