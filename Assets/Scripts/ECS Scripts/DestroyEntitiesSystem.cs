using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DestroyEntitiesSystem : MonoBehaviour
{
    public static List<Entity> EntitiesToDestroy;

    EntityManager em;
    
    // Start is called before the first frame update
    void Start()
    {
        EntitiesToDestroy = new List<Entity>();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {

        if (EntitiesToDestroy.Count > 0) {

            foreach (Entity e in EntitiesToDestroy) {

                em.DestroyEntity(e);
                EntitiesToDestroy.Clear();
            }
        
        }



    }
}
