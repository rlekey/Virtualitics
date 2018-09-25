using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;



public class StateManager : Singleton<StateManager>
{
    // Create a dictionary to reference states
    private Dictionary<string, State> States = new Dictionary<string, State>();


    // Variables for keeping track of the stage
    private string s_state_current;
    private string s_state_next;


    public GameObject pointPrefab;
    

    // Use this for initialization
    void Start()
    {


        doSetup();
        doEnterNext("State_Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (s_state_current != null)
        {
            States[s_state_current].doUpdate(Time.deltaTime);
        }
        
    }

    

    // Setup stages
    private void doSetup()
    {
        // Create States
        State s_State_Start = new State_Start();
        State s_State_Open = new State_Open();
        State s_State_Load = new State_Load();
        State s_State_Play = new State_Play();


        // Add the states to the dictionary for reference
        States.Add("State_Start", s_State_Start);
        States.Add("State_Open", s_State_Open);
        States.Add("State_Load", s_State_Load);
        States.Add("State_Play", s_State_Play);


        foreach (var item in States)
        {
            item.Value.doSetup();
        }

    }


    //
    // CHANGE STATE CALLS
    //
    // Enter state, this is what to call when wanting to change state.
    public void doEnterNext(string next_state)
    {
        doExitCurrent();

        s_state_next = next_state;
        States[s_state_next].doEnter();

    }

    // Called after enter is finished
    public void doEnterNextDone()
    {
        s_state_current = s_state_next;
        s_state_next = null;
    }


    //
    // EVENT CALLS
    //

    // Button click handler
    public void doButtonClick()
    {
        var button_Clicked = EventSystem.current.currentSelectedGameObject;
        if (button_Clicked != null)
        {
            Debug.Log("Clicked on : " + button_Clicked.name);
            States[s_state_current].doButtonClick(button_Clicked.name);
        }
        else
        {
            Debug.Log("currentSelectedGameObject is null");
        }
    }

    public void newDataReady()
    {
        States[s_state_current].newData();
    }


    //
    // EXIT STATE CALLS 
    //
    private void doExitCurrent()
    {
        if (s_state_current != null)
        {
            States[s_state_current].doExit();
        }

        doExitCurrentDone();
    }

    // Called after exit is finished
    private void doExitCurrentDone()
    {

    }
}
