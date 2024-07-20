using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] KeyCode currentKeycode;

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            //debug:
            Debug.Log("key read: " + e.keyCode);
            currentKeycode = e.keyCode;
        }
    }
}
