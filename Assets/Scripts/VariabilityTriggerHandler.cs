using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VariabilityTriggerHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> choices;

    [SerializeField]
    Material active;

    [SerializeField]
    Material normal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAllStatesToNormal()
    {
        foreach (var state in choices)
        {
            state.GetComponent<MeshRenderer>().material = normal;
        }
    }

    public void SetObjectToActive(string name)
    {
        choices.Where(x => x.name == name).FirstOrDefault().GetComponent<MeshRenderer>().material = active;
    }

    public void SetVisibilityOfSensor(bool state)
    {
        gameObject.SetActive(state);
    }
}
