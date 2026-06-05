using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : Door
{
    [SerializeField] protected bool fatCanPass;
    public override void Enter(Bob bob)
    {
        //...
        if (!fatCanPass && bob.Fat()) return;

        GameObject go = bob.gameObject;
        /* Vector3 toPortal = (transform.position - go.transform.position).normalized;
        float dot = Vector3.Dot(go.transform.forward, toPortal);
        if (Mathf.Abs(dot) < 0.8f) return; */

        // bob.gameObject.transform.position = this.transform.position;
        Vector3 forward = transform.forward;
        Vector3 bobForward = bob.gameObject.transform.forward;
        

    }

    public override bool CanEnter(Bob bob)
    {
        if (!fatCanPass && bob.Fat()) return false;

        Vector3 forward = transform.forward;
        Vector3 bobForward = bob.gameObject.transform.forward;

        // Debug.Log($"Wall: {bobForward} - Bob: {bobForward}.");

        if(forward == bobForward || forward == -bobForward) return true;

        return false;
    }
}
