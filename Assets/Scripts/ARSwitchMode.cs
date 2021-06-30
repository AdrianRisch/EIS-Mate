using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARSwitchMode : MonoBehaviour
{
    // Turn moving/scaling of the chessboard on/off
    public void Switch()
    {
        bool isEnabled = GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled;
        if (isEnabled)
        {
            isEnabled = false;
            GameObject.Find("SwitchMode").GetComponentInChildren<Text>().text = "Turn Movement On";
        }

        else
        {
            isEnabled = true;
            GameObject.Find("SwitchMode").GetComponentInChildren<Text>().text = "Turn Movement Off";
        }

        GameObject.Find("AR Session Origin").GetComponent<ARTapToPlaceObject>().enabled = isEnabled;
    }
}
