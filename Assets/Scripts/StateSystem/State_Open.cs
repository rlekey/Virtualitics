using UnityEngine;
using System.Collections;

public class State_Open : State
{

    public State_Open() { }


    private GameObject canvas;


    // Setup for intialization
    public override void doSetup()
    {
        canvas = GameObject.Find("UI/OpenCanvas");
        canvas.SetActive(false);

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
        canvas.SetActive(true);
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
            case "OpenDefault":
                NetworkManager.Instance.DefaultInformation();
                StateManager.Instance.doEnterNext("State_Play");
                break;
            case "LoadFromServer":
                NetworkManager.Instance.NewInformation();
                StateManager.Instance.doEnterNext("State_Play");
                break;
            default:
                break;
        }

        base.doButtonClick(button);
    }
}
