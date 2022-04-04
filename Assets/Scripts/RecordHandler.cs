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
    }

    public void StopRecord()
    {
        recordButton.sprite = stopRecord;
        isRecording = true;
    }
}
