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
    public float currentFactor;

    public Vector3 boardPos;
    public Quaternion boardRot;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
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

            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                boardPos = hitPose.position;
                boardRot = hitPose.rotation;
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
                boardPos = hitPose.position;
                boardRot = hitPose.rotation;
            }
            if(Input.touchCount == 2)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);

                //if any one of touchzero or touchone is cancelled or maybe ended do nothing
                if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                   touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    return;
                }
                if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initialScale = spawnedObject.transform.localScale;
                }
                else //if touch is moved
                {
                    currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    //if accidentally touched or pinch movement is very very small
                    if(Mathf.Approximately(initialDistance, 0))
                    {
                        return; //do nothing
                    }
                    var factor = currentDistance / initialDistance;
                    spawnedObject.transform.localScale = initialScale * factor;
                    currentFactor = factor;
                }
            }
        }
    }
}
