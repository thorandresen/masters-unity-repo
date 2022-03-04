using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int ignoreMask = 1 << 6;
    int ignoreDefault = 1 << 0;

    RaycastHit lastHit = new RaycastHit();
    RaycastHit lastHitComment = new RaycastHit();

    bool showGUI = false;
    string commentText = "";
    TextMeshPro commentTextMesh;

    float timer = 2f;
    float timerComment = 2f;
    List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField]
    GameObject lineDrawer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleGazeSelection();
        HandleCommentBlock();
    }

    private void HandleGazeSelection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue, ~ignoreMask))
        {
            GameObject go = hit.collider.gameObject;

            if (!go.CompareTag("hasInfo") || gameObjects.Contains(go))
            {
                // Stop timers, reset lasthit
                timer = 2f;
                lastHit = new RaycastHit();
            }
            else if (lastHit.collider == null || hit.collider.gameObject != lastHit.collider.gameObject)
            {
                // Start timer with new object
                lastHit = hit;
            }
            else if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("ADDED TARGET TO LIST");
                gameObjects.Add(go);
                timer = 2f;

                if(gameObjects.Count == 2)
                {
                    lineDrawer.GetComponent<LineDrawBehaviour>().DrawLine(gameObjects[0], gameObjects[1]);
                }
            }
        }
    }

    private void HandleCommentBlock()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue, ~ignoreDefault))
        {
            GameObject go = hit.collider.gameObject;
            Debug.Log(go.name);

            if (!go.CompareTag("isCommentable") || gameObjects.Contains(go))
            {
                // Stop timers, reset lasthit
                timerComment = 2f;
                lastHitComment = new RaycastHit();
            }
            else if (lastHitComment.collider == null || hit.collider.gameObject != lastHitComment.collider.gameObject)
            {
                // Start timer with new object
                Debug.Log("STARTED COMMENT TIMER");
                lastHitComment = hit;
            }
            else if (timerComment > 0f)
            {
                timerComment -= Time.deltaTime;
            }
            else
            {
                Debug.Log("FINISHED COMMEN TIMER");
                // Open GUI box and assign what is written to the text derived from here.
                commentTextMesh = go.GetComponentInChildren<TextMeshPro>();
                showGUI = true;
                
                timerComment = 2f;
            }
        }
    }

    private void OnGUI()
    {
        if(showGUI)
        {
            commentText = GUI.TextField(new Rect(10, 10, 300, 200), commentText);
            commentTextMesh.text = commentText;
        }
    }
}
