using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VariabilityHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> choices;

    [SerializeField]
    Material active;

    [SerializeField]
    Material normal;

    private GameObject activeObject;

    // Start is called before the first frame update
    void Start()
    {
        activeObject = choices[0];
        choices[0].GetComponent<VariabilityStateHandler>().SetActiveState(true);
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
            state.GetComponent<VariabilityStateHandler>().SetActiveState(false);
        }
    }

    public void SetObjectToActive(string name)
    {
        activeObject = choices.Where(x => x.name == name).FirstOrDefault();
        activeObject.GetComponent<MeshRenderer>().material = active;
        activeObject.GetComponent<VariabilityStateHandler>().SetActiveState(true);
        Debug.Log("CHANGED ACTIVE OBJECT TO: " + activeObject.name);
    }

    public void SetVisibilityOfObject(bool state)
    {
        gameObject.SetActive(state);
    }

    public bool GetActiveStateOfObject()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetActiveObject()
    {
        return activeObject;
    }

    public void SetAllStatesToNormalAndActiveObjectToActive()
    {
        foreach (var state in choices)
        {
            state.GetComponent<MeshRenderer>().material = normal;
            state.GetComponent<VariabilityStateHandler>().SetActiveState(false);
        }
        activeObject.GetComponent<MeshRenderer>().material = active;
        activeObject.GetComponent<VariabilityStateHandler>().SetActiveState(true);
    }
}
