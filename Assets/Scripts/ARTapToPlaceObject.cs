using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject gameObjectToInstantiate;

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private float initialDistance;
    private float currentDistance;
    private Vector3 initialScale;

    public Vector3 boardScale;
    public Vector3 boardPos;
    public Quaternion boardRot;
    private float scaleFactor;
    private Vector3 scale;

    private Quaternion standardRot = Quaternion.identity;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        scaleFactor = 1.0f;
        boardScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            float factor;

            // if there is no board spawned instantiate it
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, standardRot);
                spawnedObject.name = "Chessboard";
                boardPos = hitPose.position;
                boardRot = standardRot;
            }
            // if there is a board spawned move it
            else
            {
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = standardRot;
                boardPos = hitPose.position;
                boardRot = standardRot;
            }
            // if pinch detected
            if(Input.touchCount == 2)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);

                // if any one of touchzero or touchone is cancelled or maybe ended
                if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                   touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    // set scale and factor after each resize and set them in boardmanager and boardhighlights
                    Debug.Log("Resize Faktor: " + scaleFactor);

                    spawnedObject.GetComponent<BoardManager>().setFactor(scaleFactor);
                    spawnedObject.GetComponent<BoardHighlights>().setFactor(scaleFactor);
                    // spawnedObject.GetComponent<BoardHighlights>().setScale(scale);

                    return;
                }
                // on touch started save current distance and scale
                if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initialScale = spawnedObject.transform.localScale;
                }
                else //if touch is moved calculate new scale
                {
                    currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    //if accidentally touched or pinch movement is very very small
                    if(Mathf.Approximately(initialDistance, 0))
                    {
                        return; //do nothing
                    }
                    factor = currentDistance / initialDistance;
                    spawnedObject.transform.localScale = initialScale * factor;

                    Debug.Log("Resizing: " + factor);

                    // save factor and scale for use in boardmanager and boardhighlights
                    scaleFactor = factor;
                    scale = initialScale * factor;
                }
            }
        }
    }

    public void resetSpawnedObject()
    {
        spawnedObject = null;
    }
}
