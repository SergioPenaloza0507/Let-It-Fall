using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Physics;
using Unity.Transforms;
using TMPro;
using Unity.Mathematics;
public class EcsController : MonoBehaviour
{
    public TextMeshProUGUI arenaRestante;
    public GameObject ArenaPrefab;
    EntityManager em;
    private BlobAssetStore blobAssetStore;
    public int cantidadArena;
    Entity arenaEntidad;
    // Start is called before the first frame update
    void Start()
    {
        
        blobAssetStore = new BlobAssetStore();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        arenaEntidad = GameObjectConversionUtility.ConvertGameObjectHierarchy(ArenaPrefab,settings);
        
        
    }

    public void createArena() {
        

            Entity newEnt = em.Instantiate(arenaEntidad);
            em.SetComponentData(newEnt, new Translation
            {
                Value = new float3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z),


            });

        cantidadArena -= 1;
        arenaRestante.text = cantidadArena.ToString();
    }
    float timer = 0;
    public float tiempoespera;
    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer>tiempoespera) {
            createArena();
        }
    }
}
