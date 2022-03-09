using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    string json;
    bool test = false;

    [SerializeField]
    GameObject lineDrawer;

    [SerializeField]
    Image loadingImage;

    [SerializeField]
    HttpBehaviour _http;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool w = HandleGazeSelection();
        bool s = HandleCommentBlock();

        if(!w && !s)
        {
            loadingImage.fillAmount = 0f;
        }
    }

    private bool HandleGazeSelection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue, ~ignoreMask))
        {
            GameObject go = hit.collider.gameObject;
            
            if (!go.CompareTag("hasInfo") || gameObjects.Contains(go) || go.name.Contains("LinkPrefab"))
            {
                // Stop timers, reset lasthit
                timer = 2f;
                lastHit = new RaycastHit();
                return false;
            }
            else if (lastHit.collider == null || hit.collider.gameObject != lastHit.collider.gameObject)
            {
                // Start timer with new object
                lastHit = hit;
            }
            else if (timer > 0f)
            {
                timer -= Time.deltaTime;
                loadingImage.fillAmount = map(timer, 2f, 0f, 0f, 1f);
            }
            else
            {
                Debug.Log("ADDED TARGET TO LIST");
                loadingImage.fillAmount = 0f;
                gameObjects.Add(go);
                timer = 2f;

                if (gameObjects.Count == 2)
                {
                    lineDrawer.GetComponent<LineDrawBehaviour>().DrawLine(gameObjects[0], gameObjects[1]);
                }
            }
            return true;
        }
        timer = 2f;
        return false;
    }

    private bool HandleCommentBlock()
    {
        RaycastHit hit;

        if (showGUI)
            return false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue, ~ignoreDefault))
        {
            GameObject go = hit.collider.gameObject;

            if (!go.CompareTag("isCommentable") || gameObjects.Contains(go))
            {
                // Stop timers, reset lasthit
                timerComment = 2f;
                lastHitComment = new RaycastHit();
                return false;
            }
            else if (lastHitComment.collider == null || hit.collider.gameObject != lastHitComment.collider.gameObject)
            {
                // Start timer with new object
                lastHitComment = hit;
            }
            else if (timerComment > 0f)
            {
                timerComment -= Time.deltaTime;
                loadingImage.fillAmount = map(timerComment, 2f, 0f, 0f, 1f);
            }
            else
            {
                // Open GUI box and assign what is written to the text derived from here.
                commentTextMesh = go.GetComponentInChildren<TextMeshPro>();
                showGUI = true;
                timerComment = 2f;
            }
            return true;
        }
        timerComment = 2f;
        return false;
    }

    private void OnGUI()
    {
        if(showGUI)
        {
            commentText = GUI.TextArea(new Rect(10, 10, 700, 600), commentText);
            commentTextMesh.text = commentText;

            if (GUI.Button(new Rect(250, 615, 100, 60), "YES"))
                showGUI = false;

            if (GUI.Button(new Rect(380, 615, 100, 60), "NO"))
            {
                commentTextMesh.text = "";
                commentText = "";
                showGUI = false;
            }
  
        }

        if(gameObjects.Count == 2 && !test)
        {
            if (GUI.Button(new Rect(1380, 10, 200, 120), "SEND"))
            {
                var data = new Dictionary<string, object>();
                data.Add("motion-sensor", _http.GetSensorDict());
                data.Add("phillips-hue", _http.GetLightDict());
                data.Add("comment", commentText);

                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                StartCoroutine(_http.SendDataToAPI(json));
                test = true;
            }
        }
    }

    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
