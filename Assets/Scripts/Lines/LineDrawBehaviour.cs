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

    // TEST OBJECTS
    [SerializeField]
    GameObject obj1;

    [SerializeField]
    GameObject obj2;

    // Start is called before the first frame update
    void Start()
    {
        DrawLine(obj1, obj2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawLine(GameObject obj1, GameObject obj2)
    {
        LineRenderer localLine = Instantiate(linePrefab, transform.position, Quaternion.identity);

        localLink = CreateLink(obj1, obj2);

        localLine.SetPosition(0, obj1.transform.position);
        localLine.SetPosition(1, localLink.transform.position);
        localLine.SetPosition(2, obj2.transform.position);

        localLine.transform.position = Vector3.zero;
        localLine.GetComponent<LineBehaviour>().SetGameObjects(obj1, obj2, localLink);
    }

    private GameObject CreateLink(GameObject obj1, GameObject obj2)
    {
        return Instantiate(linkPrefab, Vector3.Lerp(obj1.transform.position, obj2.transform.position, 0.5f), Quaternion.identity);
    }
}
