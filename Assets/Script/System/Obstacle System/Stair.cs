using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Stair : Box
{
    #region Teleport System
    public override bool HasPassThroughTeleportation(Pawn pawn)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 1f)) return true;

        pawn.gameObject.transform.position += Vector3.up + pawn.gameObject.transform.forward;

        return true;
    }/*
    public override void PassThroughTeleportation(Pawn pawn)
    {
        base.PassThroughTeleportation(pawn);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 1f)) return;

        pawn.gameObject.transform.position += Vector3.up + pawn.gameObject.transform.forward;
    }*/

    public override bool HasSetpOutTeleportation(Pawn pawn)
    {
        RaycastHit hit;
        Vector3 pawnForward = pawn.gameObject.transform.forward;

        if (Physics.Raycast(transform.position, pawnForward, out hit, pawn.DistantCheck))
        {
            if (hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead))
            {
                if (objectAhead.walkable) 
                {
                    pawn.gameObject.transform.position += pawnForward;
                }
                else
                {
                    if (!Physics.Raycast(transform.position + pawnForward, Vector3.down, out hit, 1f)) return true;
                    if (!hit.collider.gameObject.TryGetComponent<Box>(out Box tile)) return true;
                    if (tile.OnSetpOutTeleportationOverride == null) return true;
                    
                    pawn.gameObject.transform.position += pawnForward + Vector3.down;
                    // ^^ indica che sopra la tile c'è una cave
                }
                return true;
            }
        }

        if (!Physics.Raycast(transform.position + pawnForward, Vector3.down, out hit, 1f)) return true;

        pawn.gameObject.transform.position += pawnForward + Vector3.down;
        return true;
    }/*
    public override void SetpOutTeleportation(Pawn pawn)
    {
        base.SetpOutTeleportation(pawn);

        RaycastHit hit;
        Vector3 pawnForward = pawn.gameObject.transform.forward;

        if (Physics.Raycast(transform.position, pawnForward, out hit, pawn.DistantCheck))
        {
            if (hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead))
            {
                if (objectAhead.walkable)
                {
                    pawn.gameObject.transform.position += pawnForward;
                }
                return;
            }
        }
       
        if (!Physics.Raycast(transform.position + pawnForward, Vector3.down, out hit, 1f)) return;

        pawn.gameObject.transform.position += pawnForward + Vector3.down;
    }*/
    #endregion
}
