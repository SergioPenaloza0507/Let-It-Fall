using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUParticles
{
    [Obsolete]
    public class SurfaceCollider : MonoBehaviour
    {
        [SerializeField] Vector2[] definition;
        [SerializeField] float energyAbsortion;
        [SerializeField] private Vector2 extrusion;


        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.49f, 1f, 0.66f);
            if (definition != null && definition.Length > 1)
            {
                Gizmos.DrawSphere(definition[0], 0.1f);
                for (int i = 1; i < definition.Length; i++)
                {
                    Gizmos.DrawSphere(definition[i], 0.1f);
                    Gizmos.DrawLine(definition[i - 1], definition[i]);
                }
            }
        }

        public Vector2[] Definition => definition;

        public float EnergyAbsortion => energyAbsortion;
    }
}
