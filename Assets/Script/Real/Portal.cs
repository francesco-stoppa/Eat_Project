using System;
using UnityEngine;

public class Portal : Box
{
    [Tooltip("Where the Pawn will come out")]
    [SerializeField] GameObject exit;
    [Tooltip("The Pawn can pass through while is fat?")]
    [SerializeField] bool fatCanPass;

    #region Base System
    protected override void BaseChecks()
    {
        base.BaseChecks();
        if(exit == null)
            Debug.LogError("The [Exit] is not set.");
    }
    #endregion

    #region Teleport System
    public override bool HasPassThroughTeleportation(Pawn pawn)
    {
        if (exit == null)
        {
            Debug.LogError("REMINDER: [Exit] is not set.");
            return true;
        }

        if (!fatCanPass && pawn.HaveFullStomach()) return true;

        if ((int)Math.Round(Vector3.Dot(transform.forward, pawn.gameObject.transform.forward)) == -1) return true;

        RaycastHit hit;
        if (Physics.Raycast(exit.transform.position, exit.transform.forward, out hit, 1f)) return true;
        if (!Physics.Raycast(exit.transform.position + exit.transform.forward, -transform.up, out hit, 1f)) return true;

        pawn.gameObject.transform.position = exit.transform.position + exit.transform.forward;
        pawn.gameObject.transform.rotation = exit.transform.rotation;
        return true;
    }/*

    public override void PassThroughTeleportation(Pawn pawn)
    {
        base.PassThroughTeleportation(pawn);

        if(exit == null)
        {
            Debug.LogError("REMINDER: [Exit] is not set.");
            return;
        }

        if (!fatCanPass && pawn.HaveFullStomach()) return;

        RaycastHit hit;
        if (Physics.Raycast(exit.transform.position, exit.transform.forward, out hit, 1f)) return;
        if (!Physics.Raycast(exit.transform.position + exit.transform.forward, -transform.up, out hit, 1f)) return;

        pawn.gameObject.transform.position = exit.transform.position + exit.transform.forward;
        pawn.gameObject.transform.rotation = exit.transform.rotation;
    }*/
    #endregion
}
