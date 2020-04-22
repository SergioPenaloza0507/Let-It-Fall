using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIUpdate : MonoBehaviour
{
    public TextMeshProUGUI arenaEmisor,arenaReceptor;
    public Canvas WinCanvas, LoseCanvas;
    float timer;
    // Start is called before the first frame update
    void Start()
    {

        WinCanvas.enabled = false;
        LoseCanvas.enabled =false;

    }

    // Update is called once per frame
    void Update()
    {
        arenaEmisor.text = UIUpdateSystem.instance.arenaEmisor.ToString();

        arenaReceptor.text = (UIUpdateSystem.instance.arenaReceptor).ToString();
        if (UIUpdateSystem.instance.lvlCompleted)
        {

            WinCanvas.enabled = true;

        }
        else { WinCanvas.enabled = false;
            Debug.Log("as");
        }
        if (UIUpdateSystem.instance.noMoreArena )
        {
           
            timer += Time.deltaTime;
            if (timer > 6 && UIUpdateSystem.instance.lvlCompleted == false)
            {

                LoseCanvas.enabled = true;
            }
            else { LoseCanvas.enabled = false; }

           

        }
    }
}
