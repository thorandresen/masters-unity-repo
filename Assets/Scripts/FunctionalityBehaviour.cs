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
    private VariabilityHolder vh;

    private VariabilityHandler variabilityTrigger;
    private VariabilityHandler variabilityAction;

    private bool shouldOnlyRunOnce = false;
    private string lightMode = "";

    // Start is called before the first frame update
    void Start()
    {
        vh = GameObject.Find("VariabilityHolder").GetComponent<VariabilityHolder>();

        variabilityTrigger = vh.GetSensor().GetComponent<VariabilityHandler>();
        variabilityAction = vh.GetAction().GetComponent<VariabilityHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(root != null)
        {
            if (!variabilityAction.GetActiveStateOfObject() && root.action.includeVariability)
            {
                variabilityAction.SetVisibilityOfObject(true);
            }

            if (!variabilityTrigger.GetActiveStateOfObject() && root.trigger.includeVariability)
            {
                variabilityTrigger.SetVisibilityOfObject(true);
            }

            if(!shouldOnlyRunOnce)
            {
                StartCoroutine(HandleOperator());
                shouldOnlyRunOnce = true;
            }
            
        }
    }

    private IEnumerator HandleOperator()
    {
        if(root.action.includeVariability && variabilityAction.GetActiveObject() != null)
        {
            lightMode = variabilityAction.GetActiveObject().name;
        }

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
            if (retrieveStateOfTrigger() == triggerVal)
            {
                if (root.trigger.includeVariability)
                {
                    string activeObjectName = variabilityTrigger.GetActiveObject().name;
                    float triggerTimer = 0f;

                    switch(activeObjectName)
                    {
                        case "1":
                            triggerTimer = 1f;
                            break;
                        case "2":
                            triggerTimer = 3f;
                            break;
                        case "3":
                            triggerTimer = 5f;
                            break;
                        case "4":
                            triggerTimer = 7f;
                            break;
                        case "5":
                            triggerTimer = 10f;
                            break;
                    }

                    yield return new WaitForSeconds(triggerTimer);

                    if(retrieveStateOfTrigger() == triggerVal)
                    {
                        Debug.Log("SET STATE BASED ON VARIABILITY");
                        setStateOfAction();
                    }
                }
                else
                {
                    setStateOfAction();
                }
                
                //Debug.Log("State and val was EQUAL");
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

        yield return new WaitForSeconds(1);
        StartCoroutine(HandleOperator());
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
        StartCoroutine(http.ChangeLightState(root.action.state, root.action.value, lightMode));
    }
}
