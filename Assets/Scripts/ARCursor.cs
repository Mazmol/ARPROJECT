using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;

    public bool useCursor = true;
    bool gameStarted = false; 
    bool objectPlaced = false; 

    void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    void MoveGame()
    {
        objectPlaced = false;
        objectToPlace.SetActive(false);
    }
    
    void Update()
    {
        if (useCursor == true && objectPlaced == false)
        {
            UpdateCursor();
        }

        if (objectPlaced == true)
        {
            cursorChildObject.SetActive(false); 
        }
        else
        {
            cursorChildObject.SetActive(true); 
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && objectPlaced == false)
        {
            if (useCursor == true && objectPlaced == false && gameStarted == false)
            {
                GameObject.Instantiate(objectToPlace, transform.position, transform.rotation);
                objectPlaced = true;
                gameStarted = true; 
            }
           /* else
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                if (hits.Count > 0 && objectPlaced == false)
                {
                    GameObject.Instantiate(objectToPlace, hits[0].pose.position, hits[0].pose.rotation);
                    //objectPlaced = true;
                }
            }*/
           if (useCursor == true && objectPlaced == false && gameStarted == true)
           {
                objectToPlace.transform.position = transform.position;
                objectToPlace.transform.rotation = transform.rotation;
                objectToPlace.SetActive(true); 
           }
        }
    }

    void UpdateCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);    

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation; 
        }
    }
}
