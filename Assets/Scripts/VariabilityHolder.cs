using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariabilityHolder : MonoBehaviour
{
    [SerializeField]
    GameObject sensorParent;

    [SerializeField]
    GameObject actionParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject GetSensor()
    {
        return sensorParent;
    }

    public GameObject GetAction()
    {
        return actionParent;
    }
}
