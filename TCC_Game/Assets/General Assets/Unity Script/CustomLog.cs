using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class CustomLog 
{
    public static void Log(this Object myObj, object msg ){
        Debug.Log($"{myObj.name}: msg");
    }
}
