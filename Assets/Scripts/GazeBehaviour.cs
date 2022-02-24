using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GazeBehaviour : MonoBehaviour
{
    List<LightInformationBehaviour> infos = new List<LightInformationBehaviour>();
    // Start is called before the first frame update
    void Start()
    {
        infos = FindObjectsOfType<LightInformationBehaviour>().ToList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);

        if (hits.Length == 0)
        {
            closeAll();
        }

        foreach (RaycastHit hit in hits)
        {
            GameObject go = hit.collider.gameObject;

            if (go.CompareTag("hasInfo"))
            {
                OpenInfo(go.GetComponent<LightInformationBehaviour>());
            }
            else
            {
                closeAll();
            }
        }
    }

    void OpenInfo(LightInformationBehaviour desiredInfo)
    {
        desiredInfo.OpenInfo();
    }

    void closeAll()
    {
        foreach (LightInformationBehaviour lightInformation in infos)
        {
            lightInformation.ClosedInfo();
        }
    }
}
