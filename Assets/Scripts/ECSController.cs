using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using TMPro;
public class ECSController : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI Entidades;
    
    float timer;
    public GameObject arenaPrefab, spawn;
    BlobAssetStore blobasset;
    static EntityManager em;
    public Transform spawnPos;
    Entity arenaEntity;

    
    public static void Start()
    {
        
    }
    public void Awake()
    {
       
        blobasset = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobasset);
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        arenaEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(arenaPrefab, settings);

    }

    float avgFrameRate;
    // Update is called once per frame
    void Update()
    {
        
        Entidades.text = em.GetAllEntities().Length.ToString();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -3));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                spawn.transform.position = Vector3.Lerp(spawn.transform.position, touchedPos, Time.deltaTime);
            }
        }

        timer += Time.deltaTime;
        
        if (timer>0.05f) {

            float3 spawnpos = new float3(spawnPos.position.x,spawnPos.position.y,spawnPos.position.z);
            Entity _arenaEntity= em.Instantiate(arenaEntity);

            em.SetComponentData(_arenaEntity, new Translation
            {
                Value = spawnpos,


            });
            timer = 0;
        }
    }
}
