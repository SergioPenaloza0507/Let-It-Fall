using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Obstacle : MonoBehaviour
{
    public int order;

    private void OnCollisionEnter(Collision other)
    {
        Obstacle oObs = other.gameObject.GetComponent<Obstacle>();
        if (oObs != null)
        {
            if (oObs.order < order)
            {
                Destroy(gameObject);
            }
        }
    }
}
