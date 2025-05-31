using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationRotate : MonoBehaviour
{
    [SerializeField] float spinSpeed;

    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}
