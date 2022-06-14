using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawBehaviour : MonoBehaviour
{

    [SerializeField]
    LineRenderer linePrefab;

    [SerializeField]
    GameObject linkPrefab;

    GameObject localLink;
    GameObject localLink2;

    //// TEST OBJECTS
    //[SerializeField]
    //GameObject obj1;

    //[SerializeField]
    //GameObject obj2;

    // Start is called before the first frame update
    void Start()
    {
        //DrawLine(obj1, obj2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawLine(GameObject obj1, GameObject obj2)
    {
        LineRenderer localLine = Instantiate(linePrefab, transform.position, Quaternion.identity);

        localLink = CreateLink(obj1, obj2, 0.3f);
        localLink2 = CreateLink(obj1, obj2, 0.7f);
        localLink2.name = "LinkPrefab2(Clone)";

        localLink.GetComponent<LinkBehaviour>().SetVariables(obj1, obj2, 0.3f);
        localLink2.GetComponent<LinkBehaviour>().SetVariables(obj1, obj2, 0.7f);

        SetActiveStateOfAllLinks(false);

        localLine.SetPosition(0, obj1.transform.position);
        localLine.SetPosition(1, localLink.transform.position);
        localLine.SetPosition(2, localLink2.transform.position);
        localLine.SetPosition(3, obj2.transform.position);

        localLine.transform.position = Vector3.zero;
        localLine.GetComponent<LineBehaviour>().SetGameObjects(obj1, obj2, localLink, localLink2);
    }

    private GameObject CreateLink(GameObject obj1, GameObject obj2, float pos)
    {
        return Instantiate(linkPrefab, Vector3.Lerp(obj1.transform.position, obj2.transform.position, pos), Quaternion.identity);
    }

    public void SetActiveStateOfLink(bool state)
    {
        localLink.SetActive(state);
    }
    public void SetActiveStateOfAllLinks(bool state)
    {
        localLink.SetActive(state);
        localLink2.SetActive(state);
    }

    public void DeleteLineAndLinks()
    {
        Destroy(GameObject.Find("LinePrefab(Clone)"));
        Destroy(localLink);
        Destroy(localLink2);
    }
}
