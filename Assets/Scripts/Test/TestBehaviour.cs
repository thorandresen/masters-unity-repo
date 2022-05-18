using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestBehaviour : MonoBehaviour
{
    [SerializeField]
    List<GameObject> testButtons;

    [SerializeField]
    List<GameObject> cogwheels;

    [SerializeField]
    GameObject requestButton;

    [SerializeField]
    GameObject loadAnimation;

    [SerializeField]
    GameObject testDialog;

    TestObject testObject;

    bool incomingFunc = false;
    private bool resetAll;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingFunc)
        {
            // Remove toggle animation
            ToggleAnimation(false);
            ToggleRequestButton(false); // if this was sent by mistake

            // Toggle correct test buttons
            ToggleTestButton(0, testObject.HueTest);
            ToggleTestButton(1, testObject.BriTest);
            ToggleTestButton(2, testObject.CtTest);
            ToggleTestButton(3, testObject.OnTest);
            incomingFunc = false;
        }

        if (resetAll)
        {
            ResetAll();
            resetAll = false;
        }
    }

    public void ToggleTestButton(int button, bool state)
    {
        testButtons[button].SetActive(state);
    }

    public void ToggleCoghweel(int button, bool state)
    {
        cogwheels[button].SetActive(state);
    }

    public void ToggleAnimation(bool state)
    {
        loadAnimation.SetActive(state);
    }

    public void ToggleRequestButton(bool state)
    {
        requestButton.SetActive(state);
    }

    public void ToggleDialog(bool state)
    {
        testDialog.SetActive(true);
    }

    public void ResetAll()
    {
        foreach (var button in testButtons)
        {
            button.SetActive(true);
            button.GetComponent<Button>().interactable = true;
        }

        foreach (var cogwheel in cogwheels)
        {
            cogwheel.SetActive(false);
        }
    }

    public void ResetAllExtern()
    {
        resetAll = true;
    }

    public void DisableAllButtons()
    {
        foreach (var button in testButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void RequestButtonClick()
    {
        ToggleRequestButton(false);

        // Set load animation to true
        ToggleAnimation(true);

        // Open dialog box
        ToggleDialog(true);
    }

    public async void HandleIncomingTest(string data)
    {
        try
        {
            // Handle incoming string
            testObject = JsonConvert.DeserializeObject<TestObject>(data);
            incomingFunc = true;
        }
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
    }

    public void ClickHueTest()
    {
        StartCoroutine(HttpHueTest());
        DisableAllButtons();
        ToggleCoghweel(0, true);
        ToggleTestButton(0, false);
    }

    public IEnumerator HttpHueTest()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://192.168.0.246:5000/setEffect");

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

    public void ClickBriTest()
    {
        StartCoroutine(HttpBriTest());
        DisableAllButtons();
        ToggleCoghweel(1, true);
        ToggleTestButton(1, false);
    }

    public IEnumerator HttpBriTest()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://192.168.0.246:5000/setEffect");

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

    public void ClickCtTest()
    {
        StartCoroutine(HttpCtTest());
        DisableAllButtons();
        ToggleCoghweel(2, true);
        ToggleTestButton(2, false);
    }

    public IEnumerator HttpCtTest()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://192.168.0.246:5000/setEffect");

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

    public void ClickOnTest()
    {
        StartCoroutine(HttpOnTest());
        DisableAllButtons();
        ToggleCoghweel(3, true);
        ToggleTestButton(3, false);
        ResetAll();
    }

    public IEnumerator HttpOnTest()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get("http://192.168.0.246:5000/setEffect");

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
}
