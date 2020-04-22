using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public class UIUpdateSystem : ComponentSystem
{
    public float arenaEmisor,arenaReceptor;
    EntityManager em;
    public static UIUpdateSystem instance;
    protected override void OnCreate()
    {
        instance = this;
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected override void OnUpdate()
    {

        
        Entities.WithAll<EmisorComponent, Translation>().ForEach((Entity e) =>
        {
           EmisorComponent emisor= em.GetComponentData<EmisorComponent>(e);
            arenaEmisor = emisor.arenaRestante;
        });

        Entities.WithAll<ReceptorComponent, Translation>().ForEach((Entity e) =>
        {
            ReceptorComponent receptor = em.GetComponentData<ReceptorComponent>(e);
            arenaReceptor = receptor.arenaRecogida;
        });
    }
    
   
}
