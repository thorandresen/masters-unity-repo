using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordHandler : MonoBehaviour
{
    [SerializeField]
    Sprite record;

    [SerializeField]
    Sprite stopRecord;

    [SerializeField]
    Image recordButton;

    [SerializeField]
    GameObject sendButton;

    [SerializeField]
    GameObject cancelButton;

    bool isRecording = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleRecord()
    {
        if(isRecording)
        {
            StartRecord();
        }
        else
        {
            StopRecord();
        }
    }

    public void StartRecord()
    {
        recordButton.sprite = record;
        isRecording = false;
        sendButton.SetActive(true);
        cancelButton.SetActive(true);
        gameObject.GetComponent<Image>().enabled = false;
    }

    public void StopRecord()
    {
        recordButton.sprite = stopRecord;
        isRecording = true;
    }

    public void SendVideo()
    {
        sendButton.SetActive(false);
        cancelButton.SetActive(false);
        //gameObject.SetActive(true);
        gameObject.GetComponent<Image>().enabled = true;
    } 
}
