using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARSwitchMode : MonoBehaviour
{
    // Turn moving/scaling of the chessboard on/off
    public void Switch()
    {
        ARPlaneManager aRPlaneManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();

        bool isEnabled = GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled;
        bool isPlaneEnabled = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>().enabled;
        if (isEnabled)
        {
            isEnabled = false;
            GameObject.Find("SwitchMode").GetComponentInChildren<Text>().text = "Turn Movement On";
            isPlaneEnabled = false;

            foreach(ARPlane plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(!aRPlaneManager.enabled);
            }
        }

        else
        {
            isEnabled = true;
            GameObject.Find("SwitchMode").GetComponentInChildren<Text>().text = "Turn Movement Off";
            isPlaneEnabled = true;
        }

        GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>().enabled = isPlaneEnabled;
        GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled = isEnabled;
    }
}
