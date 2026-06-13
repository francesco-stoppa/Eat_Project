using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : Walls
{
    [SerializeField] Tunnel exit;

    public override bool CanEnter(Bob bob)
    {
        if (bob == null) return false;
        
        GameObject go = bob.gameObject;

        if (IsExitClose() || !fatCanPass && bob.Fat() ) return false; /*|| 
            Vector3.Dot(go.transform.forward, transform.forward) > -0.8f ||
            !Physics.Raycast(exit.gameObject.transform.position + 
            exit.gameObject.transform.forward, Vector3.down, out RaycastHit x, 1f)) return;*/

        if (transform.forward != go.transform.forward) return false; // << ADD

        /*RaycastHit hit;
        Vector3 origin = go.transform.position;*/

        go.transform.position = exit.gameObject.transform.position - exit.gameObject.transform.forward;
        // go.transform.rotation = exit.gameObject.transform.rotation;
        go.transform.rotation = exit.transform.rotation * Quaternion.Euler(0, 180, 0);
        /*
        if (!Physics.Raycast(origin, Vector3.down, out hit, 1f)) return false;
        GameObject objectHit = hit.collider.gameObject;

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return false;
        obstacle.SetpOut(bob);*/
        return false;
    }

    bool IsExitClose()
    {
        if(exit == null) return true;
        if (Physics.Raycast(exit.gameObject.transform.position, exit.gameObject.transform.forward, out RaycastHit hit, 1f))
            return true;
        return false;
    }
}
