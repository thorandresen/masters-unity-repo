using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TestDialogBehaviour : MonoBehaviour
{
    [SerializeField]
    TestBehaviour testBehaviour;

    bool bri;
    bool hue;
    bool ct;
    bool on;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCancel()
    {
        testBehaviour.ToggleAnimation(false);
        testBehaviour.ToggleRequestButton(true);
        this.gameObject.SetActive(false);
    }

    public void ClickSend()
    {
        TestObject testObject = new TestObject
        {
            BriTest = bri,
            CtTest = ct,
            HueTest = hue,
            OnTest = on
        };
        var jsonString = JsonConvert.SerializeObject(testObject);

        //StartCoroutine(SendTestData(jsonString));
        testBehaviour.HandleIncomingTest(jsonString);

        this.gameObject.SetActive(false);
    }

    IEnumerator ExecuteAfterTime(float time, string jsonString)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        testBehaviour.HandleIncomingTest(jsonString);
    }

    public void setBri(bool selected)
    {
        bri = selected;
    }
    public void setHue(bool selected)
    {
        hue = selected;
    }
    public void setCt(bool selected)
    {
        ct = selected;
    }
    public void setOn(bool selected)
    {
        on = selected;
    }

    public IEnumerator SendTestData(string json)
    {
        UnityWebRequest www;

        //www.SetRequestHeader("Content-Type", "application/json");

        www = new UnityWebRequest("http://192.168.0.246:5000/json", "POST");
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
}
