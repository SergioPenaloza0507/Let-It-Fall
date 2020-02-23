using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPaused;
    
    void Awake(){
        if(instance == null){
            instance = this;
        }
        
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
