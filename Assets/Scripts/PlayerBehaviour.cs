using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int ignoreMask = 1 << 6;
    RaycastHit lastHit;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, ~ignoreMask))
        {
            GameObject go = hit.collider.gameObject;

            if (!go.CompareTag("hasInfo"))
            {
                // Stop timers, reset lasthit
                lastHit = new RaycastHit();
            } 
            else if (hit.collider.gameObject != lastHit.collider.gameObject)
            {
                // Start timer with new object
                
                lastHit = hit;
                return;
            } 
            else if(timer > 0f)
            {
                timer -= Time.deltaTime;
                // Countdown
            }
        }
    }


}
