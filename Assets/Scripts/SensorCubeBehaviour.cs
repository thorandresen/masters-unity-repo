using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorCubeBehaviour : MonoBehaviour
{
    float timer = 2f;
    private Dictionary<String, System.Object> sensorDict = null;

    [SerializeField]
    private HttpBehaviour http;

    [SerializeField]
    TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text.richText = true;

        if (sensorDict == null)
        {
            text.text = "N/A";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0f)
        {
            timer -= Time.deltaTime;
        } 
        else
        {
            sensorDict = http.GetSensorDict();

            if (sensorDict == null)
            {
                text.text = "N/A";
            }
            else
            {
                text.text = "<b>Motion Sensor</b> \n Presence: " + sensorDict["presence"] + "\n Light: " + sensorDict["light"] + " Temp: " + sensorDict["temp"];
            }
            
            timer = 2f;
        }
    }
}
