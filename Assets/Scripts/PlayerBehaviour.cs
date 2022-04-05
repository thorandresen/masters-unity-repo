using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    int ignoreMask = 1 << 6;
    int ignoreDefault = 1 << 0;

    RaycastHit lastHit = new RaycastHit();
    RaycastHit lastHitComment = new RaycastHit();

    [SerializeField]
    Image selectButtonImage;

    [SerializeField]
    Sprite selectSprite;

    [SerializeField]
    Sprite lookSprite;

    bool isSelect = false;
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

    [SerializeField]
    FunctionalityBehaviour funcGameObject;

    [SerializeField]
    GameObject funcPrefab;

    List<GameObject> funcObjects = new List<GameObject>();

    private bool spawnPrefab = false;
    private GameObject funcGO;
    private bool deployUI = false;

    [SerializeField]
    private Button deployButton;

    [SerializeField]
    private Button deployOverwriteButton;

    [SerializeField]
    private GameObject deployText;

    private Rootobject rootObject;

    [SerializeField]
    Button deploy1;

    [SerializeField]
    Button deploy2;

    private int deployInt = 0;

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
        bool s = HandleVariability();
        HandleObjectUI();

        if(!w && !s)
        {
            loadingImage.fillAmount = 0f;
        }

        if(spawnPrefab)
        {
            funcGO = Instantiate(funcPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            funcGO.GetComponent<FunctionalityBehaviour>().http = GameObject.Find("HttpObject").GetComponent<HttpBehaviour>();
            funcGO.GetComponent<FunctionalityBehaviour>().root = rootObject;
            funcGO.GetComponent<FunctionalityBehaviour>().stateNumber = funcObjects.Count;

            LineBehaviour lb = FindObjectOfType<LineBehaviour>();
            lb.SetGradientToDeployColors();

            LineDrawBehaviour ldb = FindObjectOfType<LineDrawBehaviour>();
            ldb.SetActiveStateOfLink(true);
            commentTextMesh = GameObject.Find("LinkText").GetComponent<TextMeshPro>();
            updateCommenText = true;
            spawnPrefab = false;
            deployUI = false;

            funcObjects.Add(funcGO);
        }
    }

    private bool HandleGazeSelection()
    {
        RaycastHit hit;

        if (!isSelect)
            return false;

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

    private bool HandleVariability()
    {
        RaycastHit hit;

        if (showGUI || !isSelect)
            return false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue, ~ignoreDefault))
        {
            GameObject go = hit.collider.gameObject;

            if (!go.CompareTag("isTargetable") || gameObjects.Contains(go))
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
                // Set new to active and change the state.
                VariabilityHandler handler;
                if (go.GetComponentInParent<ActionTest>() != null)
                {
                   handler = GameObject.Find(deployInt + "Action").GetComponent<VariabilityHandler>();
                }
                else
                {
                    handler = GameObject.Find(deployInt + "Sensor").GetComponent<VariabilityHandler>();
                }
                
                
                handler.SetAllStatesToNormal();
                handler.SetObjectToActive(go.name);
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

    public async void HandleIncomingObject(string data)
    {
        try
        {
            rootObject = JsonConvert.DeserializeObject<Rootobject>(data);
            incomingText = $"{rootObject.comment.text}\n \nIF: {rootObject.trigger.state} {rootObject.trigger.operatorType} {rootObject.trigger.value}\n \nSET: {rootObject.action.state} TO: {rootObject.action.value}";
            deployUI = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SpawnPrefab()
    {
        spawnPrefab = true;
    }

    public void SpawnAndOverwritePrefab()
    {
        deploy1.gameObject.SetActive(false);
        deploy2.gameObject.SetActive(false);
        funcObjects.All(x => { Destroy(x); return true; });
        funcObjects.Clear();
        spawnPrefab = true;
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

        if(deployUI)
        {
            if(funcObjects.Any())
            {
                deployOverwriteButton.gameObject.SetActive(true);
            }
            deployButton.gameObject.SetActive(true);
            deployText.GetComponent<TextMeshProUGUI>().text = incomingText;
        } 
        else
        {
            deployButton.gameObject.SetActive(false);
            deployOverwriteButton.gameObject.SetActive(false);
        }

        if(funcObjects.Count == 1 )
        {
            deploy1.gameObject.SetActive(true);
        }
        else if (funcObjects.Count == 2) {
            deploy2.gameObject.SetActive(true);
        }
    }

    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void ToggleSelect()
    {
        isSelect = !isSelect;

        if(isSelect)
        {
            selectButtonImage.sprite = selectSprite;
        } 
        else
        {
            selectButtonImage.sprite = lookSprite;
        }
    }

    public void Deploy1()
    {
        deploy1.image.color = Color.red;
        deploy2.image.color = Color.white;
        deployInt = 0;
        VariabilityHandler handler1 = GameObject.Find(deployInt + "Action").GetComponent<VariabilityHandler>();
        VariabilityHandler handler2 = GameObject.Find(deployInt + "Sensor").GetComponent<VariabilityHandler>();

        handler1.SetAllStatesToNormalAndActiveObjectToActive();
        handler2.SetAllStatesToNormalAndActiveObjectToActive();
    }

    public void Deploy2()
    {
        deploy1.image.color = Color.white;
        deploy2.image.color = Color.red;
        deployInt = 1;
        VariabilityHandler handler1 = GameObject.Find(deployInt + "Action").GetComponent<VariabilityHandler>();
        VariabilityHandler handler2 = GameObject.Find(deployInt + "Sensor").GetComponent<VariabilityHandler>();

        handler1.SetAllStatesToNormalAndActiveObjectToActive();
        handler2.SetAllStatesToNormalAndActiveObjectToActive();
    }
}