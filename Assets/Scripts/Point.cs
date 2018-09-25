using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Point : MonoBehaviour {

    private int SepalL;
    private int SepalW;
    private int PetalL;
    private int PetalW;
    private int Class;

    private int CurSet;


    float midSL = 0;
    float midSW = 0;
    float midPL = 0;
    float midPW = 0;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    
    private float lamda = 1.0f;
    private float speed = .02f;

    private GameObject dataZone;
    private GameObject sphere;
    private TextMesh textMesh;

    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);



    // Constructor intitializing to 0
    public Point()
    {
        SepalL = 0;
        SepalW = 0;
        PetalL = 0;
        PetalW = 0;
        Class = 0;
    }

    // Constructor with initialized data
    public Point(int sepalL, int sepalW, int petalL, int petalW, int classy)
    {
        SepalL = sepalL;
        SepalW = sepalW;
        PetalL = petalL;
        PetalW = petalW;
        Class = classy;

        ChangeColor();
        SendToPosition(0);
    }

    // Load data after construction
    public void LoadData(int sepalL, int sepalW, int petalL, int petalW, int classy)
    {
        dataZone = GameObject.Find("DataZone");
        sphere = gameObject.transform.FindChild("Sphere").gameObject;
        textMesh = GetComponentInChildren<TextMesh>();

        SepalL = sepalL;
        SepalW = sepalW;
        PetalL = petalL;
        PetalW = petalW;
        Class = classy;

        midSL = DatabaseManager.Instance.MidSL;
        midSW = DatabaseManager.Instance.MidSW;
        midPL = DatabaseManager.Instance.MidPL;
        midPW = DatabaseManager.Instance.MidPW;

        ChangeColor();
        SendToPosition(0);
    }

    // Subtract half the max of each variable so that scaling happens from the center. TODO: this should not be just Max, this should be the mid point of Max, Min, for better scaling function.
    public void SendToPosition(int beginVectorIndex)
    {
        CurSet = beginVectorIndex;

        startPosition = gameObject.transform.localPosition;
        lamda = 0.0f;
        switch (beginVectorIndex)
        {
            case 0:
                targetPosition = new Vector3(SepalL - midSL, SepalW - midSW, PetalL - midPL);
                break;
            case 1:
                targetPosition = new Vector3(SepalW - midSW, PetalL - midPL, PetalW - midPW);
                break;
            case 2:
                targetPosition = new Vector3(PetalL - midPL, PetalW - midPW, SepalL - midSL);
                break;
            case 3:
                targetPosition = new Vector3(PetalW - midPW, SepalL - midSL, SepalW - midSW);
                break;
            default:
                break;
        }
    }


    private void ChangeColor()
    {
        Renderer rend = sphere.GetComponent<Renderer>();
        switch (Class)
        {
            case 0:
                rend.material.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
                break;
            case 1:
                rend.material.SetColor("_Color", new Color(0.0f, 1.0f, 0.0f));
                break;
            case 2:
                rend.material.SetColor("_Color", new Color(0.0f, 0.0f, 1.0f));
                break;
            default:
                break;
        }
    }

    // Start use for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (lamda < 1.0f)
        {
            
            lamda += speed;

            gameObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, lamda));
        }
        else
        {
            lamda = 1.0f;
        }

	}

    // Get the info of this point.
    public PointStruct GetPointInfo()
    {
        return new PointStruct(SepalL, SepalW, PetalL, PetalW, Class);
    }

    // Interaction

    //-------------------------------------------------
    // Called when a Hand starts hovering over this object
    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand)
    {
        textMesh.text = string.Format("SepalL: {0}, SepalW: {1}, PetalL: {2}, PetalW: {3}, Class: {4}", SepalL, SepalW, PetalL, PetalW, Class);

        Vector3 directionToTarget = Camera.main.transform.position - transform.position;
        textMesh.transform.rotation = Quaternion.LookRotation(-directionToTarget);
    }


    //-------------------------------------------------
    // Called when a Hand stops hovering over this object
    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand)
    {
        textMesh.text = "";
    }

    //-------------------------------------------------
    // Called every Update() while a Hand is hovering over this object
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            if (hand.currentAttachedObject != gameObject)
            {
               
                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(GetComponent<Interactable>());

                // Attach this object to the hand
                hand.AttachObject(gameObject, attachmentFlags);
            }
            else
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(GetComponent<Interactable>());

                
            }
        }
    }


    //-------------------------------------------------
    // Called when this GameObject becomes attached to the hand
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        //textMesh.text = "Attached to hand: " + hand.name;
        
    }


    //-------------------------------------------------
    // Called when this GameObject is detached from the hand
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {

    }


    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        hand.DetachObject(gameObject);
        Vector3 pos = gameObject.transform.localPosition;
        //pos /= dataZone.transform.localScale.x;
        switch (CurSet)
        {
            case 0:
                SepalL = (int)pos.x + (int)midSL;
                SepalW = (int)pos.y + (int)midSW;
                PetalL = (int)pos.z + (int)midPL;
                //targetPosition = new Vector3(SepalL - midSL, SepalW - midSW, PetalL - midPL);
                break;
            case 1:
                SepalW = (int)pos.x + (int)midSL;
                PetalL = (int)pos.y + (int)midPL;
                PetalW = (int)pos.z + (int)midPW;
                //targetPosition = new Vector3(SepalW - midSW, PetalL - midPL, PetalW - midPW);
                break;
            case 2:
                PetalL = (int)pos.x + (int)midPL;
                PetalW = (int)pos.y + (int)midPW;
                SepalL = (int)pos.z + (int)midSL;
                //targetPosition = new Vector3(PetalL - midPL, PetalW - midPW, SepalL - midSL);
                break;
            case 3:
                PetalW = (int)pos.x + (int)midPW;
                SepalL = (int)pos.y + (int)midSL;
                SepalW = (int)pos.z + (int)midSW;
                //targetPosition = new Vector3(PetalW - midPW, SepalL - midSL, SepalW - midSW);
                break;
            default:
                break;
        }

        textMesh.text = string.Format("SepalL: {0}, SepalW: {1}, PetalL: {2}, PetalW: {3}, Class: {4}", SepalL, SepalW, PetalL, PetalW, Class);

        Vector3 directionToTarget = Camera.main.transform.position - transform.position;
        textMesh.transform.rotation = Quaternion.LookRotation(-directionToTarget);

        // Attach this object to the hand
        hand.AttachObject(gameObject, attachmentFlags);
    }
}
