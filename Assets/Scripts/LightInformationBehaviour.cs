using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInformationBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform LightInfo;

    Vector3 DesiredScale = Vector3.zero;

    const float speed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightInfo.localScale = Vector3.Lerp(LightInfo.localScale, DesiredScale, Time.deltaTime * speed);
    }

    public void OpenInfo()
    {
        DesiredScale = Vector3.one;
    }

    public void ClosedInfo()
    {
        DesiredScale = Vector3.zero;
    }
}
