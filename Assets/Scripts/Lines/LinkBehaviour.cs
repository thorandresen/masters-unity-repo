using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBehaviour : MonoBehaviour
{
    GameObject g1;
    GameObject g2;
    float pos;
    bool posCorrect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(posCorrect)
        {
            transform.position = Vector3.Lerp(g1.transform.position, g2.transform.position, pos);
        }
            
        
    }

    public void SetVariables(GameObject g1, GameObject g2, float pos)
    {
        this.g1 = g1;
        this.g2 = g2;
        this.pos = pos;
        posCorrect = true;
    }
}
