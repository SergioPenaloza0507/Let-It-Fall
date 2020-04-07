using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public struct EmisorComponent : IComponentData
{
    public float arenaRestante;
    public float tiempoEntreSpawn;
    public bool activo;
    public Entity arenaEntity;
    public int color;
    

}
