using System;
using UnityEngine;

public class MoveTimeTest : MonoBehaviour
{
    public static MoveTimeTest Instance { get; private set; }

    public float value;
   
    private void Awake()
    {
        // vv singleton vv
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
