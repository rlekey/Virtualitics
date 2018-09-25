using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataZone : MonoBehaviour {

    private bool interactable = false;

    // Let's grab the interaction manager to make things a bit easier.
    private CustomInteractionManager interactionManager;

    // Track our grip state
    private bool[] gripDown = new bool[2];

    // Tracking the start of a grip
    private Vector3[] controllerStart = new Vector3[2];

    // Variable to keep track of starting scale
    private float startScale;


    // Magnitude modifiers for movement and scale.
    private float scaleMag = 0.2f;
    private float moveMag = 0.2f;
    private float rotMag = 5f;

    // Use this for initialization
    void Start () {

        EnableInteraction(true);

        interactionManager = CustomInteractionManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        if (interactable)
        {
            // Both grips down, used for scale and TODO: rotation.
            if (gripDown[0] && gripDown[1])
            {
                //
                // Get scale information
                //
                // Get the difference in distance from the start to the end.
                float curDistance = Vector3.Distance(interactionManager.GetControllerTransform(0).position, interactionManager.GetControllerTransform(1).position);
                float startDistance = Vector3.Distance(controllerStart[0], controllerStart[1]);

                
                float changeInScale = curDistance - startDistance;
                changeInScale *= scaleMag;

                float targetScale = changeInScale + startScale;

                if (targetScale <= .01f)
                {
                    targetScale = .01f;
                }

                gameObject.transform.localScale = new Vector3(targetScale, targetScale, targetScale);

                //
                // Get rotational information
                //
                // Get the normalized vectors of the start and the end.
                Vector3 currentVector = Vector3.Normalize(interactionManager.GetControllerTransform(0).position - interactionManager.GetControllerTransform(1).position);
                Vector3 startVector = Vector3.Normalize(controllerStart[0] - controllerStart[1]);

                // Get Dot product, ignoring Y so we don't do anything really weird.
                float curDot = Vector2.Dot(new Vector2(currentVector.x, currentVector.z), new Vector2(startVector.x, startVector.z));

                // This tells us how fast we have to move, but not direction
                float rotAngle = 1.0f - curDot;
                rotAngle *= rotMag;


                // Lets get a perpendicular vector to start vector so we can see which direction we should move.
                Vector2 perpStartVector = new Vector2(startVector.z, -startVector.x);
                float directionDot = Vector2.Dot(new Vector2(currentVector.x, currentVector.z), perpStartVector);

                // Set it to 1 or -1 so set direction.
                if (directionDot >= 0)
                {
                    directionDot = 1;
                }
                else
                {
                    directionDot = -1;
                }

                gameObject.transform.Rotate(new Vector3(0, rotAngle * directionDot, 0));

                //Debug.Log(curDot);
            }
            // One grip down, used for movement
            else if (gripDown[0])
            {
                Vector3 curPosition0 = interactionManager.GetControllerTransform(0).position;

                Vector3 movement = controllerStart[0] - curPosition0;

                movement *= moveMag;

                gameObject.transform.position += movement;

            }
            // One grip down, used for movement
            else if (gripDown[1])
            {
                Vector3 curPosition1 = interactionManager.GetControllerTransform(1).position;

                Vector3 movement = controllerStart[1] - curPosition1;

                movement *= moveMag;

                gameObject.transform.position += movement;
            }
        }
        
    }

    // Register grip handlers
    public void EnableInteraction(bool on)
    {
        if (on)
        {
            CustomInteractionManager.OnGripPressed += GripPressed;
            CustomInteractionManager.OnGripReleased += GripReleased;
            interactable = true;
        }
        else
        {
            CustomInteractionManager.OnGripPressed -= GripPressed;
            CustomInteractionManager.OnGripReleased -= GripReleased;
            interactable = false;

        }
    }


    private void GripPressed(int controllerIndex)
    {
        gripDown[controllerIndex] = true;

        //Debug.Log("Grip Pressed " + controllerIndex);

        // We should update start position any time either controller changes button state. This helps user experience so they know where they start.
        controllerStart[0] = interactionManager.GetControllerTransform(0).position;
        controllerStart[1] = interactionManager.GetControllerTransform(1).position;

        startScale = gameObject.transform.localScale.x;
    }

    private void GripReleased(int controllerIndex)
    {
        gripDown[controllerIndex] = false;

        //Debug.Log("Grip Released" + controllerIndex);

        // We should update start position any time either controller changes button state.
        controllerStart[0] = interactionManager.GetControllerTransform(0).position;
        controllerStart[1] = interactionManager.GetControllerTransform(1).position;
    }
}
