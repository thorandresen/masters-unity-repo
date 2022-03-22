using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FunctionalityBehaviour : MonoBehaviour
{
    public Rootobject root = null;
    private float timer = 2f;
    public dynamic triggerVal;
    public dynamic actionVal;

    
    public HttpBehaviour http;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(root != null)
        {
            if (timer <= 0f)
            {
                HandleOperator();
                timer = 2f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void HandleOperator()
    {
       
        switch (root.trigger.valueType)
        {
            case "bool":
                triggerVal = Convert.ToBoolean(root.trigger.value);
                break;
            case "int":
                triggerVal = Convert.ToInt32(root.trigger.value);
                break;
        }

        switch (root.action.valueType)
        {
            case "bool":
                actionVal = Convert.ToBoolean(root.action.value);
                break;
            case "int":
                actionVal = Convert.ToInt32(root.action.value);
                break;
        }


        if (root.trigger.operatorType == "==")
        {
            if(retrieveStateOfTrigger() == triggerVal)
            {
                setStateOfAction();
                Debug.Log("State and val was EQUAL");
            }
        }
        else if (root.trigger.operatorType == ">") {
            if (retrieveStateOfTrigger() > triggerVal)
            {
                setStateOfAction();
                Debug.Log("State was GREATER than val");
            }
        }
        else if (root.trigger.operatorType == "<")
        {
            if (retrieveStateOfTrigger() < triggerVal)
            {
                setStateOfAction();
                Debug.Log("State was LESSER than val");
            }
        }
    }

    dynamic retrieveStateOfTrigger()
    {
        if(root.trigger.deviceId == "1923")
        {
            var sensorDict = http.GetSensorDict();
            var state = sensorDict[root.trigger.state];

            if(root.trigger.state == "presence")
            {
                return Convert.ToBoolean(state);
            }
            else if (root.trigger.state == "temp")
            {
                return Convert.ToInt32(state);
            }
            else if (root.trigger.state == "light")
            {
                return Convert.ToInt32(state);
            }
        }
        else
        {

        }
        return null;
    }

    void setStateOfAction()
    {
        StartCoroutine(http.ChangeLightState(root.action.state, root.action.value));
    }
}
