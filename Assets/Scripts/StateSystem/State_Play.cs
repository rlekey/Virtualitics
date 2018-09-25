using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class State_Play : State
{

    public State_Play() { }

    private GameObject canvas;


    private List<GameObject> pointsList;
    private GameObject dataZone;

    private float distanceFromCamera = 2.0f;

    private int currentSet = 0;

    // Setup for intialization
    public override void doSetup()
    {
        canvas = GameObject.Find("UI/PlayCanvas");
        canvas.SetActive(false);

        dataZone = GameObject.Find("DataZone");


        base.doSetup();
    }

    // Cleanup the state
    public override void doCleanup()
    {

        base.doCleanup();
    }

    // Enable the state to call
    public override void doEnable()
    {

        base.doEnable();
    }

    // Call on entry to the state
    public override void doEnter()
    {
        // Let's drop the Data zone right in front of the user. this way it works if he's in seated or room mode.
        dataZone.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);

        currentSet = 0;
        canvas.SetActive(true);


        canvas.GetComponentInParent<RotationalFollow>().enabled = false;
        base.doEnter();
    }

    // After entry has finished
    public override void doEnterDone()
    {

        base.doEnterDone();
    }

    // Call to exit
    public override void doExit()
    {
        canvas.SetActive(false);
        canvas.GetComponentInParent<RotationalFollow>().enabled = true;
        base.doExit();
    }

    // Call after exit is finished
    public override void doExitDone()
    {

        base.doExitDone();
    }

    // Reciever for UI functions
    public override void doButtonClick(string button)
    {
        switch (button)
        {
            // TODO: Cool feature would be to custom map the points. Here i'm just moving forward in the set. If we could set columns to x,y,z that might make more useful info
            case "NextSet":
                currentSet += 1;
                if (currentSet > 3)
                {
                    currentSet = 0;
                }

                foreach (GameObject point in pointsList)
                {
                    point.GetComponent<Point>().SendToPosition(currentSet);
                }

                SetMappingText();
                break;
            case "UploadData":
                DatabaseManager.Instance.WriteCSV(pointsList);
                break;
            default:
                break;
        }

        base.doButtonClick(button);
    }


    public override void newData()
    {
        pointsList = new List<GameObject>();
        foreach (PointStruct item in DatabaseManager.Instance.points)
        {
            GameObject point = GameObject.Instantiate(StateManager.Instance.pointPrefab, dataZone.transform, false);
            point.GetComponent<Point>().LoadData(item.SepalL, item.SepalW, item.PetalL, item.PetalW, item.Class);
            pointsList.Add(point);
        }

        SetText();

        base.newData();
    }


    // State Specific functions
    private void SetText()
    {
        canvas.transform.Find("TotalPointsMut").GetComponent<Text>().text = DatabaseManager.Instance.TotalCount.ToString();
        canvas.transform.Find("Class0Mut").GetComponent<Text>().text = DatabaseManager.Instance.Class0Count.ToString();
        canvas.transform.Find("Class1Mut").GetComponent<Text>().text = DatabaseManager.Instance.Class1Count.ToString();
        canvas.transform.Find("Class2Mut").GetComponent<Text>().text = DatabaseManager.Instance.Class2Count.ToString();

        SetMappingText();
    }

    private void SetMappingText()
    {
        switch (currentSet)
        {
            case 0:
                canvas.transform.Find("CurrentMappingMut").GetComponent<Text>().text = "SL, SW, PL";
                break;
            case 1:
                canvas.transform.Find("CurrentMappingMut").GetComponent<Text>().text = "SW, PL, PW";
                break;
            case 2:
                canvas.transform.Find("CurrentMappingMut").GetComponent<Text>().text = "PL, PW, SL";
                break;
            case 3:
                canvas.transform.Find("CurrentMappingMut").GetComponent<Text>().text = "PW, SL, SW";
                break;
            default:
                break;
        }
    }
        

}
