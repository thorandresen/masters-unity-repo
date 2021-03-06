using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBehaviour : MonoBehaviour
{
    private GameObject obj1;
    private GameObject obj2;
    private GameObject link;
    private GameObject link2;
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
            localLine.SetPosition(2, link2.transform.position);
            localLine.SetPosition(3, obj2.transform.position);
        }
    }

    public void SetGameObjects(GameObject obj1, GameObject obj2, GameObject link, GameObject link2)
    {
        this.obj1 = obj1;
        this.obj2 = obj2;
        this.link = link;
        this.link2 = link2;
    }

    public void SetGradientToDeployColors()
    {
        Gradient gradient = localLine.colorGradient;

        

        if(obj1.name == "LightCube")
        {
            gradient.SetKeys(
           new GradientColorKey[] { new GradientColorKey(new Color(154 / 255f, 220 / 255f, 255 / 255f), 0.0f), new GradientColorKey(new Color(255 / 255f, 248 / 255f, 154 / 255f), 1.0f) },
           new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(0.8f, 1.0f) }
            );
        }
        else
        {
            // gradient.SetKeys(
            //new GradientColorKey[] { new GradientColorKey(new Color(255 / 255f, 248 / 255f, 154 / 255f), 0.0f), new GradientColorKey(new Color(255 / 255f, 220 / 255f, 154 / 255f), 1.0f) },
            //new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(0.8f, 1.0f) }
            //);

            gradient.SetKeys(
           new GradientColorKey[] { new GradientColorKey(new Color(255 / 255f, 248 / 255f, 154 / 255f), 0.0f), new GradientColorKey(new Color(154 / 255f, 220 / 255f, 255 / 255f), 1.0f) },
           new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(0.8f, 1.0f) }
            );
        }
       

        localLine.colorGradient = gradient;
    }
}
