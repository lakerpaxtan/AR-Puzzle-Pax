using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;

public class startingScript : MonoBehaviour
{
    

    public GameObject gameManager;
    public GameObject indicatorObject;
    public GameObject fullField;
    //public GameObject placementObject;
    private ARRaycastManager arOrigin;
    public GameObject ArOriginObject;

    public static Vector3 finalRay;
    private bool placementIsGood;
    private bool waitingForNextTap;
    private GameObject firstObject;

    private bool originalScene;

    //public GameObject ARCamera; 

    

    private Pose placementPose;

    void Start()
    {
        originalScene = true;
        Debug.Log("Dboi start");
        placementIsGood = false;
        arOrigin = ArOriginObject.GetComponent<ARRaycastManager>();
        //waitingForNextTap = false;
        //Debug.Log(39.78f * 1);
    }


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Dboi awake");
    }

    // Update is called once per frame
    void Update()
    {
        if(originalScene){
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            
        }
        

    }
    // private void PlaceSecondObject(){
    //     Debug.Log("Dboi placed two");
    //     var tempPosition = placementPose.position;
    //     //var measurementLength = new Vector3(0, 0, 0.2794f); //11 inches
    //     var tempObject = Instantiate(placementObject, tempPosition, placementPose.rotation);
    //     float measurementLength = 39.3701f * Vector3.Distance(firstObject.transform.position , tempObject.transform.position);
    //     Debug.Log("Dboi " + measurementLength);
    //     tempObject.transform.Rotate(new Vector3(0f, 180f, 0f));
    //     LineRenderer lineRenderer = tempObject.AddComponent<LineRenderer>();
    //     lineRenderer.material.color = Color.black;
    //     lineRenderer.widthMultiplier = 0.005f;
    //     lineRenderer.SetPosition(0, firstObject.transform.position);
    //     lineRenderer.SetPosition(1, tempObject.transform.position);

    //     Vector3 halfwayPoint = (firstObject.transform.position + tempObject.transform.position) / 2;
    //     GameObject textObject = new GameObject();
    //     textObject.transform.rotation = tempObject.transform.rotation;
    //     textObject.transform.position = halfwayPoint;
    //     textObject.transform.position += new Vector3(0f, 0.02f, 0f);
    //     textObject.transform.Rotate(new Vector3(90f, -90f, 0f));
    //     TextMeshPro tempText = textObject.AddComponent<TextMeshPro>();
    //     tempText.color = Color.green;
    //     textObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    //     tempText.SetText("{0:2} inches", measurementLength);
    //     waitingForNextTap = false;
        
    // }

    public void attemptToPlaceObject(){
        if(originalScene && placementIsGood){
    
            PlaceObjectAtCurrentRaycast();
            Debug.Log("Dboi first tapped");
                
              
            
        }
    }
    private void PlaceObjectAtCurrentRaycast(){
        Debug.Log("Dboi placed one");
        var tempPosition = placementPose.position;
        //var measurementLength = new Vector3(0, 0, 0.2794f); //11 inches
        //var tempObject = Instantiate(placementObject, tempPosition, placementPose.rotation);
        fullField.SetActive(true);
        Debug.Log("dboi ray position");
        Debug.Log(tempPosition);
        Debug.Log("dboi camera position");
        Debug.Log(ArOriginObject.transform.position);

        var deltaPosition = tempPosition - ArOriginObject.transform.position;
        Debug.Log("dboi delta position");
        Debug.Log(deltaPosition);

        ArOriginObject.transform.rotation = Quaternion.identity;
        ArOriginObject.transform.position = new Vector3(0,0,0);
        ArOriginObject.transform.position = - deltaPosition + new Vector3(4,0,4);

        Debug.Log("dboi final position");
        Debug.Log(ArOriginObject.transform.position);
        gameManager.GetComponent<FieldScript>().enabled = true;
        //indicatorObject.SetActive(false);
        Debug.Log("dboi stil alive");
        //var tempObject = GameObject.Find("AR Session Origin");
        Debug.Log("dboi right before gameobject");
        
        
        

        //firstObject = tempObject;
        //waitingForNextTap = true;
        //tempObject = Instantiate(placementObject, tempPosition, placementPose.rotation);
        //tempObject.transform.Translate(measurementLength);
        //tempObject = Instantiate(placementObject, tempPosition, placementPose.rotation);
        //tempObject.transform.Translate(measurementLength * 2);



    }

    private void UpdatePlacementPose(){
        
        if(!originalScene){
            placementIsGood = false;
            return;
        }
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f,0f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementIsGood = hits.Count > 0;
        if(placementIsGood){
            placementPose = hits[0].pose;
            placementPose.rotation = Quaternion.LookRotation(new Vector3(Camera.current.transform.forward.x, 0, Camera.current.transform.forward.z).normalized); 
        }


    }

    private void UpdatePlacementIndicator(){
        if(!originalScene){
            return;
        }
        if(placementIsGood){
            indicatorObject.SetActive(true);
            indicatorObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }else{
            indicatorObject.SetActive(false);
        }
    }
}
