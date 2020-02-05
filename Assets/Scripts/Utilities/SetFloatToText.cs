using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetFloatToText : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetFloat(float val)
    {
        text.text = val.ToString();
    }
}
