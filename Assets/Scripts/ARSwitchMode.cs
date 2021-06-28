using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSwitchMode : MonoBehaviour
{
    // Turn moving/scaling of the chessboard on/off
    public void Switch()
    {
        bool isEnabled = GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled;
        if (isEnabled)
            isEnabled = false;
        else
            isEnabled = true;

        GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled = isEnabled;
    }
}
