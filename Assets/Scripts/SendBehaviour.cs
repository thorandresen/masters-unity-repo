using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBehaviour : MonoBehaviour
{
    private RecordHandler recordHandler;
    // Start is called before the first frame update
    void Start()
    {
        recordHandler = GameObject.Find("Record").GetComponent<RecordHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendVideo()
    {
        recordHandler.SendVideo();
    }
}
