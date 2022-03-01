using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightCubeBehaviour : MonoBehaviour
{
    float timer = 2f;
    private Dictionary<String, System.Object> lightDict = null;

    [SerializeField]
    private HttpBehaviour http;

    [SerializeField]
    TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text.richText = true;

        if (lightDict == null)
        {
            text.text = "N/A";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            lightDict = http.GetLightDict();

            if (lightDict == null)
            {
                text.text = "N/A";
            }
            else
            {
                text.text = "<b>Phillips Hue</b> \n" + "State: " + lightDict["on"].ToString() + " - Brightness: " + lightDict["bri"] + "\n" + "Hue: " + lightDict["hue"] + " - Warmth: " + lightDict["ct"];
            }

            timer = 2f;
        }
    }
}
