using UnityEngine;
using System.Collections;

public class State_Load : State
{

    public State_Load() { }



    // Setup for intialization
    public override void doSetup()
    {

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

        base.doButtonClick(button);
    }
}
