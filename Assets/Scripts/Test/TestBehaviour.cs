using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
    [SerializeField]
    List<GameObject> testButtons;

    [SerializeField]
    GameObject requestButton;

    [SerializeField]
    GameObject loadAnimation;

    [SerializeField]
    GameObject testDialog;

    TestObject testObject;

    bool incomingFunc = false;

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

            // Toggle correct test buttons
            ToggleTestButton(0, testObject.HueTest);
            ToggleTestButton(1, testObject.BriTest);
            ToggleTestButton(2, testObject.CtTest);
            ToggleTestButton(3, testObject.OnTest);
            incomingFunc = false;
        }
    }

    public void ToggleTestButton(int button, bool state)
    {
        testButtons[button].SetActive(state);
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
}
