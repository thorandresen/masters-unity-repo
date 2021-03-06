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
    public List<GameObject> gameObjects = new List<GameObject>();

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
    private GameObject requestButton;

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
    private List<Rootobject> mutliRootObject;

    [SerializeField]
    Button deploy1;

    [SerializeField]
    Button deploy2;

    [SerializeField]
    GameObject deployParent;

    private List<ExplainDTO> explainObjects = new List<ExplainDTO>();

    [SerializeField]
    List<TextMeshPro> ExplainNames;

    [SerializeField]
    List<SpriteRenderer> ExplainCogwheels;

    [SerializeField]
    Sprite cogwheelOrange;

    [SerializeField]
    Sprite cogwheelPurple;

    [SerializeField]
    List<Material> LinkCommenMaterials;

    private int deployInt = 0;

    private List<string> incomingTexts = new List<string>();
    private string incomingTextName;
    private List<string> incomingTextNames = new List<string>();

    private float pauseTimer = 0f;
    bool w = false;
    bool s = false;

    bool deploy1Name = false;
    bool deploy2Name = false;

    bool requestButtonActive;

    // Start is called before the first frame update
    void Start()
    {
        explainObjects.Add(new ExplainDTO
        {
            Name = ExplainNames[0],
            Cogwheel = ExplainCogwheels[0]
        });

        explainObjects.Add(new ExplainDTO
        {
            Name = ExplainNames[1],
            Cogwheel = ExplainCogwheels[1]
        });

        bulbImage = objectUIs[0];
        motionImage = objectUIs[1];
        requestButton = objectUIs[3];
        bulbImage.SetActive(false);
        motionImage.SetActive(false);
        requestButton.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (pauseTimer <= 0f)
        {
            w = HandleGazeSelection();
            s = HandleVariability();
        }
        else
        {
            w = false;
            s = false;
            pauseTimer -= Time.deltaTime;
        }
        
        HandleObjectUI();

        if(!w && !s)
        {
            loadingImage.fillAmount = 0f;
        }


        if(spawnPrefab)
        {
            deployParent.SetActive(true);

            if(rootObject != null)
            {
                SpawnSingleFuncObject();
            }
            else if (mutliRootObject != null)
            {
                SpawnMultiFuncObjects();
            }
        }
    }

    private void SpawnSingleFuncObject()
    {
        spawnPrefab = false;
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
        deployUI = false;
        incomingTexts.Add($"{rootObject.comment.text}");
        incomingTextNames.Add(incomingTextName);
        funcObjects.Add(funcGO);

        if (funcObjects.Count() > 1)
        {
            ldb.SetActiveStateOfAllLinks(true);
            var test = GameObject.Find("LinkPrefab2(Clone)").GetComponentInChildren<TextMeshPro>();
            test.text = rootObject.comment.text;
            deploy2Name = true;
            deploy2.GetComponentInChildren<Text>().text = incomingTextName;
            Deploy2();

        }
        else
        {
            var test = GameObject.Find("LinkPrefab(Clone)").GetComponentInChildren<TextMeshPro>();
            test.text = rootObject.comment.text;
            deploy1Name = true;
            deploy1.GetComponentInChildren<Text>().text = incomingTextName;
            Deploy1();
        }

        //spawnPrefab = false;
    }

    private void SpawnMultiFuncObjects()
    {
        incomingTextNames.Clear();
        foreach (var tempRootObject in mutliRootObject)
        {
            funcGO = Instantiate(funcPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            funcGO.GetComponent<FunctionalityBehaviour>().http = GameObject.Find("HttpObject").GetComponent<HttpBehaviour>();
            funcGO.GetComponent<FunctionalityBehaviour>().root = tempRootObject;
            funcGO.GetComponent<FunctionalityBehaviour>().stateNumber = funcObjects.Count;

            LineBehaviour lb = FindObjectOfType<LineBehaviour>();
            lb.SetGradientToDeployColors();

            LineDrawBehaviour ldb = FindObjectOfType<LineDrawBehaviour>();
            ldb.SetActiveStateOfAllLinks(true);
            commentTextMesh = GameObject.Find("LinkText").GetComponent<TextMeshPro>();
            updateCommenText = true;
            deployUI = false;
            incomingTexts.Add($"{tempRootObject.comment.text}");
            incomingTextName = tempRootObject.comment.name;
            incomingTextNames.Add(tempRootObject.comment.name);
            funcObjects.Add(funcGO);

            if (funcObjects.Count() > 1)
            {
                var test = GameObject.Find("LinkPrefab2(Clone)").GetComponentInChildren<TextMeshPro>();
                test.text = tempRootObject.comment.text;
                Deploy2();
                deploy2Name = true;
                deploy2.GetComponentInChildren<Text>().text = incomingTextName;
                
            }
            else
            {
                var test = GameObject.Find("LinkPrefab(Clone)").GetComponentInChildren<TextMeshPro>();
                test.text = tempRootObject.comment.text;
                Deploy1();
                deploy1Name = true;
                deploy1.GetComponentInChildren<Text>().text = incomingTextName;
            }

            spawnPrefab = false;
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
                pauseTimer = 1f;

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

            if (!go.CompareTag("isTargetable") || gameObjects.Contains(go) || go.GetComponent<VariabilityStateHandler>().GetActiveState())
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
                pauseTimer = 1f;
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
            if(!requestButtonActive)
            {
                requestButton.SetActive(true);
                requestButtonActive = true;
            }
            
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
            try
            {
                rootObject = JsonConvert.DeserializeObject<Rootobject>(data);
                incomingText = $"{rootObject.comment.text}";
                incomingTextName = rootObject.comment.name;
                deployUI = true;
            }
            catch (Exception e)
            {
                rootObject = null;
                mutliRootObject = JsonConvert.DeserializeObject<List<Rootobject>>(data);
                incomingText = $"<b>KODE 1:</b> \n {mutliRootObject[0].comment.text} \n \n <b>KODE 2:</b> \n {mutliRootObject[1].comment.text}";
                incomingTextName = $"{mutliRootObject[0].comment.name} & {mutliRootObject[1].comment.name}";
                deployUI = true;
            }
            
            //incomingText = $"{rootObject.comment.text}\n \nHVIS: {rootObject.trigger.state} {rootObject.trigger.operatorType} {rootObject.trigger.value}\n \nS??T: {rootObject.action.state} TIL: {rootObject.action.value}";
            //incomingText = $"{rootObject.comment.text}";
            //incomingTextName = rootObject.comment.name;
            //deployUI = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SpawnPrefab()
    {
        if(funcObjects.Count() == 2)
        {
            deploy1.gameObject.SetActive(false);
            deploy2.gameObject.SetActive(false);
            funcObjects.All(x => { Destroy(x); return true; });
            funcObjects.Clear();
            spawnPrefab = true;
        } 
        else
        {
            spawnPrefab = true;
        }
    }

    public void SpawnAndOverwritePrefab()
    {
        deploy1.gameObject.SetActive(false);
        deploy2.gameObject.SetActive(false);
        funcObjects.All(x => { Destroy(x); return true; });
        funcObjects.Clear();
        spawnPrefab = true;
    }

    public void DestroyPrefabs()
    {
        deploy1.gameObject.SetActive(false);
        deploy2.gameObject.SetActive(false);
        funcObjects.All(x => { Destroy(x); return true; });
        funcObjects.Clear();
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

        //if(gameObjects.Count == 2 && !UIbool)
        //{
        //    if (GUI.Button(new Rect(1100, 10, 200, 120), "SEND"))
        //    {
        //        var data = new Dictionary<string, object>();
        //        data.Add("comment", commentText);

        //        json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        //        StartCoroutine(_http.SendDataToAPI(json));
        //        UIbool = true;
        //    }
        //}

        //if(updateCommenText)
        //{
        //    commentTextMesh.text = incomingText;
        //    updateCommenText = false;
        //}

        if(deployUI && gameObjects.Count() == 2)
        {
            //if(funcObjects.Any())
            //{
            //    deployOverwriteButton.gameObject.SetActive(true);
            //}
            deployButton.gameObject.SetActive(true);
            deployText.GetComponent<TextMeshProUGUI>().text = incomingText;
        } 
        else
        {
            deployButton.gameObject.SetActive(false);
            deployOverwriteButton.gameObject.SetActive(false);
            deployUI = false;
        }

        if(funcObjects.Count == 1)
        {
            deploy1.gameObject.SetActive(true);    
        }
        else if (funcObjects.Count == 2) {
            deploy1.gameObject.SetActive(true);
            deploy2.gameObject.SetActive(true);
        }

        //if(deploy1Name)
        //{
        //    deploy1.GetComponentInChildren<Text>().text = incomingTextName;
        //    deploy1Name = false;
        //}

        //if (deploy2Name)
        //{
        //    deploy2.GetComponentInChildren<Text>().text = incomingTextName;
        //    deploy2Name = false;
        //}
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
        deploy1.image.color = Color.white;
        deploy2.image.color = Color.grey;
        deployInt = 0;
        VariabilityHandler handler1 = GameObject.Find(deployInt + "Action").GetComponent<VariabilityHandler>();
        VariabilityHandler handler2 = GameObject.Find(deployInt + "Sensor").GetComponent<VariabilityHandler>();
        incomingText = incomingTexts[deployInt];
        updateCommenText = true;

        if(incomingTextNames.Count != 0)
        {
            explainObjects[0].Name.text = incomingTextNames[0];
            explainObjects[0].Cogwheel.sprite = cogwheelOrange;
            explainObjects[1].Name.text = incomingTextNames[0];
            explainObjects[1].Cogwheel.sprite = cogwheelOrange;
        }

        var linkPrefab = GameObject.Find("LinkPrefab(Clone)");
        linkPrefab.GetComponent<MeshRenderer>().material = LinkCommenMaterials[1];

        var linkPrefab2 = GameObject.Find("LinkPrefab2(Clone)");
        linkPrefab2.GetComponent<MeshRenderer>().material = LinkCommenMaterials[0];

        handler1.SetAllStatesToNormalAndActiveObjectToActive();
        handler2.SetAllStatesToNormalAndActiveObjectToActive();
    }

    public void Deploy2()
    {
        deploy1.image.color = Color.grey;
        deploy2.image.color = Color.white;
        deployInt = 1;
        VariabilityHandler handler1 = GameObject.Find(deployInt + "Action").GetComponent<VariabilityHandler>();
        VariabilityHandler handler2 = GameObject.Find(deployInt + "Sensor").GetComponent<VariabilityHandler>();
        incomingText = incomingTexts[deployInt];
        updateCommenText = true;

        if (incomingTextNames.Count != 0)
        {
        explainObjects[0].Name.text = incomingTextNames[1];
        explainObjects[0].Cogwheel.sprite = cogwheelPurple;
        explainObjects[1].Name.text = incomingTextNames[1];
        explainObjects[1].Cogwheel.sprite = cogwheelPurple;
        }

        var linkPrefab = GameObject.Find("LinkPrefab(Clone)");
        linkPrefab.GetComponent<MeshRenderer>().material = LinkCommenMaterials[0];

        var linkPrefab2 = GameObject.Find("LinkPrefab2(Clone)");
        linkPrefab2.GetComponent<MeshRenderer>().material = LinkCommenMaterials[2];

        handler1.SetAllStatesToNormalAndActiveObjectToActive();
        handler2.SetAllStatesToNormalAndActiveObjectToActive();
    }

    public void RedrawLine()
    {
        lineDrawer.GetComponent<LineDrawBehaviour>().DeleteLineAndLinks();
        lineDrawer.GetComponent<LineDrawBehaviour>().DrawLine(gameObjects[0], gameObjects[1]);
    }
}