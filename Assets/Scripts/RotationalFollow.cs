using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalFollow : MonoBehaviour {

    
    public float distanceFromCamera = .8f;
    public float interpolationSpeed = .02f;


    private Vector3 targetPosition;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);

        //TODO: Terribly inefficient, should make this a single call, lookat just needs to be calculated differently. Making the canvas backwards so I can just use LookAt for now.
        gameObject.transform.LookAt(Camera.main.transform);
        //gameObject.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, interpolationSpeed);

    }
}
