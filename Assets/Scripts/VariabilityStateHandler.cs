using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariabilityStateHandler : MonoBehaviour
{
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetActiveState()
    {
        return isActive;
    }

    public void SetActiveState(bool state)
    {
        isActive = state;
    }
}
