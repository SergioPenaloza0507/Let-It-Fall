using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIUpdate : MonoBehaviour
{
    public TextMeshProUGUI arenaEmisor,arenaReceptor;
   
    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        arenaEmisor.text = UIUpdateSystem.instance.arenaEmisor.ToString();

        arenaReceptor.text = (UIUpdateSystem.instance.arenaReceptor).ToString();
    }
}
