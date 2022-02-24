using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetLightData());   
    }

    public IEnumerator ChangeLightState(bool state)
    {
        UnityWebRequest www;
        if (state)
        {
            www = UnityWebRequest.Get("http://127.0.0.1:5000/on");
        } else
        {
            www = UnityWebRequest.Get("http://127.0.0.1:5000/off");
        }
        

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

    public IEnumerator GetLightData()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://127.0.0.1:5000/getLightInformation");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            string jsonStr = Encoding.UTF8.GetString(results);
            var jsonDict = JsonConvert.DeserializeObject<Dictionary<String, System.Object>>(jsonStr);
            Debug.Log(jsonDict["on"]);
        }
    }
}
