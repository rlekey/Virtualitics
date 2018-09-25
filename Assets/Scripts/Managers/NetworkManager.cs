using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager> {

    public string defaultURL = "http://52.53.134.46:3500";

    private string response = "";
    //private bool connected = false;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
        if (response != "")
        {
            //Debug.Log(response);
            DatabaseManager.Instance.LoadCSV(response);

            response = "";
        }
	}
    
    public void DefaultInformation ()
    {
        StartCoroutine(GetDefaultInfo());
    }

    public void NewInformation()
    {
        StartCoroutine(GetNewInfo());
    }

    public void SendNewData(string info)
    {
        StartCoroutine(SendNewInfo(info));
    }

    /*
    private void TestConnection()
    {
        ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
        //connectionTestResult = Network.TestConnection("http://52.53.134.46:3500");

        if (connectionTestResult != ConnectionTesterStatus.PublicIPIsConnectable)
        {
            connected = false;
        }
        else
        {
            connected = true;
        }
    }
    */

    //
    // Get default data.
    //
    // Overload so we don't have to pass in a default URL
    private IEnumerator GetDefaultInfo()
    {

        WWW www = new WWW(defaultURL);
        yield return www;

        response = www.text;
        if (response == "")
            response = "noconnection";
    }


    //
    // Get new Data
    //

    // Overload so we don't have to pass in a default URL
    private IEnumerator GetNewInfo()
    {

        WWW www = new WWW(defaultURL + "/new-data");
        yield return www;

        response = www.text;
        if (response == "")
            response = "noconnection";
    }

    //
    // Post new data
    //

    private IEnumerator SendNewInfo(string info)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", info);

        WWW www = new WWW(defaultURL + "/submit-new", form);
        yield return www;
    }
}
