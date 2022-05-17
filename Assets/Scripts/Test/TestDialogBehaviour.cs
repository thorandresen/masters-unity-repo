using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log(jsonString);
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
}
