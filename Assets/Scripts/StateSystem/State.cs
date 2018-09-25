using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State
{
    public string stateName;

    // Constructor
    public State()
    {

    }

    // Setup for intialization
    public virtual void doSetup()
    {

    }

    // Cleanup the state
    public virtual void doCleanup()
    {
       
    }

    // Enable the state to call
    public virtual void doEnable()
    {

    }

    // Call on entry to the state
    public virtual void doEnter()
    {

        doEnterDone();
    }

    // After entry has finished
    public virtual void doEnterDone()
    {
        // Finished bringing in the state.

        StateManager.Instance.doEnterNextDone();
    }

    // Call to exit
    public virtual void doExit()
    {

        // Send off the terrain
        doExitDone();
    }

    // Call after exit is finished
    public virtual void doExitDone()
    {

    }

    

    // Reciever for UI functions
    public virtual void doButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "Reset":
                StateManager.Instance.doEnterNext("State_Logo");
                break;
            default:
                break;
        }
    }

    // Update function called every frame
    public virtual void doUpdate(float elap)
    {

    }

    // Tell the state that new data is ready to be loaded
    public virtual void newData()
    {

    }

}
