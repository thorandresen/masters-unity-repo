using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpBehaviour : MonoBehaviour
{
    private Dictionary<String, System.Object> sensorDict = null;
    private Dictionary<String, System.Object> lightDict = null;
    private float timer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(GetLightData());
            StartCoroutine(GetSensorData());
            timer = 2f;
        }
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
            lightDict = null;
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            string jsonStr = Encoding.UTF8.GetString(results);
            var jsonDict = JsonConvert.DeserializeObject<Dictionary<String, System.Object>>(jsonStr);
            lightDict = jsonDict;
        }
    }

    public IEnumerator GetSensorData()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://127.0.0.1:5000/getAllSensorInformation");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            lightDict = null;
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            string jsonStr = Encoding.UTF8.GetString(results);
            var jsonDict = JsonConvert.DeserializeObject<Dictionary<String, System.Object>>(jsonStr);
            sensorDict = jsonDict;
        }
    }

    public Dictionary<String, System.Object> GetSensorDict()
    {
        return sensorDict;
    }

    public Dictionary<String, System.Object> GetLightDict()
    {
        return lightDict;
    }
}
