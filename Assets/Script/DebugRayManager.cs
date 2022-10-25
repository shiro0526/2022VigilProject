using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayManager : MonoBehaviour
{
    public static DebugRayManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    public static void ShowRoute(List<Vector3> list,Color color , float duration) 
    {
        for (int i = 1; i < list.Count; i++)
        {
            Debug.DrawLine(list[i - 1], list[i], color, duration);
        }
        Debug.DrawLine(list[list.Count - 1], list[0], color, duration);

    }

}
