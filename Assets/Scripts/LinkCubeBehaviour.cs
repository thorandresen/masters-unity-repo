using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LinkCubeBehaviour : MonoBehaviour
{

    float timer = 2f;
    private Dictionary<String, System.Object> lightDict = null;

    [SerializeField]
    private HttpBehaviour http;

    [SerializeField]
    TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text.richText = true;

        if (lightDict == null)
        {
            text.text = "N/A";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 2f;
        }
    }
}
