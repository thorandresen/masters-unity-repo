using Newtonsoft.Json;
using System;
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
    TextMeshPro commentTextMesh = null;

    float timer = 2f;
    float timerComment = 2f;
    List<GameObject> gameObjects = new List<GameObject>();

    string json;
    bool UIbool = false;

    bool updateCommenText = false;
    string incomingText = "";

    [SerializeField]
    GameObject lineDrawer;

    [SerializeField]
    Image loadingImage;

    [SerializeField]
    HttpBehaviour _http;

    [SerializeField]
    List<GameObject> objectUIs;

    private GameObject bulbImage;
    private GameObject motionImage;

    // Start is called before the first frame update
    void Start()
    {
        bulbImage = objectUIs[0];
        motionImage = objectUIs[1];
        bulbImage.SetActive(false);
        motionImage.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool w = HandleGazeSelection();
        bool s = HandleCommentBlock();
        HandleObjectUI();

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

    private void HandleObjectUI()
    {
        if(gameObjects.Contains(GameObject.Find("SensorCube")))
        {
            motionImage.SetActive(true);
        }
        if (gameObjects.Contains(GameObject.Find("LightCube")))
        {
            bulbImage.SetActive(true);
        }
    }

    public void ClearListener()
    {
        motionImage.SetActive(false);
        bulbImage.SetActive(false);

        gameObjects.Clear();

        GameObject line = GameObject.Find("LinePrefab(Clone)");
        GameObject link = GameObject.Find("LinkPrefab(Clone)");
        Destroy(line);
        Destroy(link);
    }

    public void HandleIncomingObject(string data)
    {
        try
        {
            var rootObject = JsonConvert.DeserializeObject<Rootobject>(data);

            if(commentTextMesh != null)
            {
                incomingText = rootObject.comment.text;
                updateCommenText = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        
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

        if(gameObjects.Count == 2 && !UIbool)
        {
            if (GUI.Button(new Rect(1100, 10, 200, 120), "SEND"))
            {
                var data = new Dictionary<string, object>();
                data.Add("motion-sensor", _http.GetSensorDict());
                data.Add("phillips-hue", _http.GetLightDict());
                data.Add("comment", commentText);

                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                StartCoroutine(_http.SendDataToAPI(json));
                UIbool = true;
            }
        }

        if(updateCommenText)
        {
            commentTextMesh.text = incomingText;
            updateCommenText = false;
        }
    }

    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}