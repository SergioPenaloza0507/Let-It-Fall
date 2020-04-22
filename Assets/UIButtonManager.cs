using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIButtonManager : MonoBehaviour
{
    public delegate void onResetLvl();
    public static event onResetLvl levelReseted;
    // Start is called before the first frame update
    void Start()
    {
        DestroyArena.levelRestarted = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RestartLevel() {
        DestroyArena.levelRestarted = true;
       
       // levelReseted();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void backToMenu (){
        DestroyArena.levelRestarted = true;
        SceneManager.LoadScene(0);
    }
    public void GoToLevels() {


        SceneManager.LoadScene(0);
    }



}
