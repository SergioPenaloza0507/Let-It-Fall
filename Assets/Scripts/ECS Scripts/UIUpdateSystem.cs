using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public class UIUpdateSystem : ComponentSystem
{
    [SerializeField] TextMeshProUGUI textoEmisor, textoReceptor,textoPuntaje;
    EntityManager em;
   
    protected override void OnUpdate()
    {
        Entities.WithAll<EmisorComponent, Translation>().ForEach((Entity e) =>
        {
            Translation posicion = em.GetComponentData<Translation>(e);
            float3 pos = posicion.Value;
            textoEmisor.transform.position = new Vector3(pos.x,pos.y,pos.z);
        });
    }
    protected override void OnCreate()
    {
        textoEmisor = GameObject.FindGameObjectWithTag("emisor").GetComponent<TextMeshProUGUI>();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
   
}
