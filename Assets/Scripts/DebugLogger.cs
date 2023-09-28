using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public void LogMessage(bool toggled)
    {
        Debug.Log(toggled.ToString());
    }
}