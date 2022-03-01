using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBehaviour : MonoBehaviour
{
    private GameObject obj1;
    private GameObject obj2;
    private GameObject link;
    private LineRenderer localLine;

    // Start is called before the first frame update
    void Start()
    {
        localLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(obj1 != null && obj2 != null)
        {
            localLine.SetPosition(0, obj1.transform.position);
            localLine.SetPosition(1, link.transform.position);
            localLine.SetPosition(2, obj2.transform.position);
        }
    }

    public void SetGameObjects(GameObject obj1, GameObject obj2, GameObject link)
    {
        this.obj1 = obj1;
        this.obj2 = obj2;
        this.link = link;
    }
}
