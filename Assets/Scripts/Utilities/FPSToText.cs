using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSToText : MonoBehaviour
{
    private TextMeshProUGUI txt;
    
    float deltaTime;
    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        txt.text = Mathf.Ceil (fps).ToString ();
    }
}
