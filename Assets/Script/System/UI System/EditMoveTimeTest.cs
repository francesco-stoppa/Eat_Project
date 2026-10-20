using System;
using UnityEngine;

public class EditMoveTimeTest : MonoBehaviour
{
    MoveTimeTest _moveTimeTest;

    private void Start()
    {
        _moveTimeTest = MoveTimeTest.Instance;
        if (_moveTimeTest == null)
        {
            Debug.LogError("The [MoveTimeTest] is missing.");
            return;
        }
    }

    public void SetMoveTime(float value)
    {
        if (_moveTimeTest == null) return;

        _moveTimeTest.value = value;
    }
}
