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
    public bool lvlCompleted, noMoreArena;
    EntityManager em;
    public static UIUpdateSystem instance;
    protected override void OnCreate()
    {
        noMoreArena = false;
        lvlCompleted = false;
        instance = this;
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected override void OnUpdate()
    {

        
        Entities.WithAll<EmisorComponent, Translation>().ForEach((Entity e) =>
        {
           EmisorComponent emisor= em.GetComponentData<EmisorComponent>(e);
            arenaEmisor = emisor.arenaRestante;
            if (arenaEmisor < 1)
            {

                noMoreArena = true;
            }
            else { noMoreArena = false; }
        });

        Entities.WithAll<ReceptorComponent, Translation>().ForEach((Entity e) =>
        {
            ReceptorComponent receptor = em.GetComponentData<ReceptorComponent>(e);
            arenaReceptor = receptor.arenaRecogida;

            if (arenaReceptor > 150)
            {

                lvlCompleted = true;
            }
            else
            {
                lvlCompleted = false;
            }
        });

       
    }
    
   
}
