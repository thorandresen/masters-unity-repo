using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpBehaviour : MonoBehaviour
{
    private Dictionary<String, System.Object> sensorDict = null;
    private Dictionary<String, System.Object> lightDict = null;
    private float timer = 1f;
    private string lightmodeState = "";
    private string stateState = "";

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
            timer = 1f;
        }
    }

    public IEnumerator ChangeLightOnOffState(bool state)
    {
        UnityWebRequest www;
        if (state)
        {
            www = UnityWebRequest.Get("http://192.168.0.246:5000/on");
        } 
        else
        {
            www = UnityWebRequest.Get("http://192.168.0.246:5000/off");
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

    public IEnumerator ChangeLightState(string state, dynamic value, string lightmode)
    {
        if (state != "" && GetHashString(state + value) != stateState)
        {
            UnityWebRequest www;
            if (state == "hue")
            {
                www = UnityWebRequest.Get("http://192.168.0.246:5000/setHue?value=" + Convert.ToInt32(value));
            }
            else if (state == "ct")
            {
                www = UnityWebRequest.Get("http://192.168.0.246:5000/setCT?value=" + Convert.ToInt32(value));
            }
            else if (state == "bri")
            {
                www = UnityWebRequest.Get("http://192.168.0.246:5000/setBrightness?value=" + Convert.ToInt32(value));
            }
            else if (state == "power")
            {
                www = UnityWebRequest.Get("http://192.168.0.246:5000/setPower?value=" + Convert.ToString(value));

            }
            else
            {
                throw new Exception();
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Upload complete!");
            }

            stateState = GetHashString(state + value);
        }

        if (lightmode != "" && !lightmode.Equals(lightmodeState))
        {
            StartCoroutine(ChangeLightMode(lightmode));
        }
    }

    public IEnumerator ChangeLightMode(string mode)
    {
        UnityWebRequest www;
        switch (mode)
        {
            case "1":
                www = UnityWebRequest.Get("http://192.168.0.246:5000/normal");
                break;
            case "3":
                www = UnityWebRequest.Get("http://192.168.0.246:5000/pulse");
                break;
            case "5":
                www = UnityWebRequest.Get("http://192.168.0.246:5000/blink");
                break;
            default:
                throw new Exception();
        }

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Upload complete!");
        }

        lightmodeState = mode;
    }

    public IEnumerator GetLightData()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://192.168.0.246:5000/getLightInformation");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            lightDict = null;
            //Debug.Log(www.error);
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

        www = UnityWebRequest.Get("http://192.168.0.246:5000/getAllSensorInformation");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            lightDict = null;
            //Debug.Log(www.error);
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

    public IEnumerator SendDataToAPI(string json)
    {
        UnityWebRequest www;
        
        //www.SetRequestHeader("Content-Type", "application/json");

        www = new UnityWebRequest("http://192.168.0.246:5000/data", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.result);
        }
    }

    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}
