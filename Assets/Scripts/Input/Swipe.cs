using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Swipe
{
    public Vector2 startPos;
    public Vector2 endPos;
    public float DeltaMagnitude => (startPos - endPos).magnitude;
}
