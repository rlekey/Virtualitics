using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CustomInteractionManager : Singleton<CustomInteractionManager> {

    public GameObject Hand1;
    public GameObject Hand2;

    // Variables for controllers
    private SteamVR_Controller.Device controller0;
    private SteamVR_Controller.Device controller1;

    // Making buttons easier to understand
    private EVRButtonId gripButton = EVRButtonId.k_EButton_Grip;


    // Event registration
    public delegate void GripPressed(int controllerIndex);
    public static event GripPressed OnGripPressed;

    public delegate void GripReleased(int controllerIndex);
    public static event GripReleased OnGripReleased;

    //public delegate void GripPressed(int controllerIndex);
    //public static event GripPressed OnGrip;

    // Use this for initialization
    void Start () {
		StartCoroutine(FindControllers());
	}
	
	// Update is called once per frame
	void Update () {
        if (controller0 != null)
        {
            if (controller0.GetPressDown(gripButton))
            {
                OnGripPressed(0);
            }
            if (controller0.GetPressUp(gripButton))
            {
                OnGripReleased(0);
            }
        }
        if (controller1 != null)
        {
            if (controller1.GetPressDown(gripButton))
            {
                OnGripPressed(1);
            }
            if (controller1.GetPressUp(gripButton))
            {
                OnGripReleased(1);
            }
        }
    }

    
    public Transform GetControllerTransform(int controllerIndex)
    {
        if (controllerIndex == 0)
        {
            return Hand1.transform;
        }
        else
        {
            //Debug.Log(Hand2.transform.position);
            return Hand2.transform;
        }
    }
    
    private IEnumerator FindControllers()
    {
        
        while (true)
        {
            // Don't need to run this every frame
            yield return new WaitForSeconds(1.0f);

            // We've found both controllers now, break out of the loop
            if (controller0 != null && controller1 != null)
                break;

            // Try to get the controllers attached to the hand objects.
            controller0 = Hand1.GetComponent<Valve.VR.InteractionSystem.Hand>().controller;
            controller1 = Hand2.GetComponent<Valve.VR.InteractionSystem.Hand>().controller;
           
        }
    }
}
