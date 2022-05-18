using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HttpServer : MonoBehaviour
{
	private HttpListener listener;
	private Thread listenerThread;

	[SerializeField]
	PlayerBehaviour playerBehaviour;

	[SerializeField]
	TestBehaviour testBehaviour;

	void Start()
	{
		listener = new HttpListener();
		listener.Prefixes.Add("http://localhost:4444/");
		listener.Prefixes.Add("http://127.0.0.1:4444/");
        //listener.Prefixes.Add("http://192.168.0.121:4444/"); // Tablet
        //listener.Prefixes.Add("http://192.168.0.190:4444/"); // Laursen
        //listener.Prefixes.Add("http://192.168.0.123:4444/"); // Labtools A22
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		listener.Start();

		listenerThread = new Thread(startListener);
		listenerThread.Start();
		Debug.Log("Server Started");
	}

	void Update()
	{
	}

	private void startListener()
	{
		while (true)
		{
			var result = listener.BeginGetContext(ListenerCallback, listener);
			result.AsyncWaitHandle.WaitOne();
		}
	}

	private void ListenerCallback(IAsyncResult result)
	{
		var context = listener.EndGetContext(result);

		if (context.Request.QueryString.AllKeys.Length > 0)
			foreach (var key in context.Request.QueryString.AllKeys)
			{
				Debug.Log("Key: " + key + ", Value: " + context.Request.QueryString.GetValues(key)[0]);
			}

		Debug.Log(context.Request.Url.ToString());

		if (context.Request.HttpMethod == "POST")
		{
			Thread.Sleep(1000);
			string data_text = new StreamReader(context.Request.InputStream,
								context.Request.ContentEncoding).ReadToEnd();

			if (context.Request.Url.ToString() == "http://127.0.0.1:4444/test" || context.Request.Url.ToString() == "http://192.168.0.123:4444/test")
			{
				testBehaviour.HandleIncomingTest(data_text);
            } 
			else
            {
				playerBehaviour.HandleIncomingObject(data_text);
			}
		}

		if(context.Request.HttpMethod == "GET")
        {
			if (context.Request.Url.ToString() == "http://127.0.0.1:4444/resettest" || context.Request.Url.ToString() == "http://192.168.0.123:4444/resettest")
			{
				testBehaviour.ResetAllExtern();
			}
		}

		context.Response.Close();
	}
}
