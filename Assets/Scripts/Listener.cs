using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Listener : MonoBehaviour
{

    public UnityEvent downEvent;
    public UnityEvent upEvent;

    void OnMouseDown()
    {
        //downEvent?.Invoke();
        System.Diagnostics.Debug.WriteLine("Test");
    }

    void OnMouseUp()
    {
        //upEvent?.Invoke();
    }

    //public bool isPressed;

    //    // Start is called before the first frame update
    //    public void OnUpdateSelected(BaseEventData data)
    //    {
    //        if (isPressed)
    //        {
    //        }
    //    }
    //    public void OnPointerDown(PointerEventData data)
    //    {
    //        isPressed = true;
    //        downEvent?.Invoke();

    //    }
    //    public void OnPointerUp(PointerEventData data)
    //    {
    //        isPressed = false;
    //        upEvent?.Invoke();
    //    }
}
