using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rootobject
{
    public Comment comment { get; set; }
    public Trigger trigger { get; set; }
    public Action action { get; set; }
}

public class Comment
{
    public string name { get; set; }
    public string text { get; set; }
}

public class Trigger
{
    public int deviceId { get; set; }
    public string state { get; set; }
    public string _operator { get; set; }
    public string value { get; set; }
}

public class Action
{
    public int deviceId { get; set; }
    public string state { get; set; }
    public string value { get; set; }
}



