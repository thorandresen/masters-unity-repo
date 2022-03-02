using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GazeBehaviour : MonoBehaviour
{
    List<LightInformationBehaviour> infos = new List<LightInformationBehaviour>();
    int ignoreMask = 1 << 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        infos = FindObjectsOfType<LightInformationBehaviour>().ToList();
        RaycastHit[] hits = Physics.RaycastAll(origin: transform.position, direction: transform.forward, int.MaxValue, ~ignoreMask);

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
