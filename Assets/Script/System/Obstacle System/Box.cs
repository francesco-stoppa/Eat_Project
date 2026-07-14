using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Tooltip("Small object dont make you fat")]
    public E_ObjectWeight weight;
    [Tooltip("The Pawn can walk on this object?")]
    public bool walkable = true;
    [Tooltip("The Pawn can eat this object?")]
    public bool eatable = true;

    public Func<Pawn, bool> OnSetpOutTeleportationOverride;

    // public int id;


    void Awake()
    {
        Repositioning();
        BaseChecks();
    }

    #region Base System
    protected virtual void Repositioning(bool callAfterFreeFall = false)
    {
        transform.position = Vector3Int.RoundToInt(transform.position);

        if (weight == E_ObjectWeight.Levitate || callAfterFreeFall) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        if (!Physics.Raycast(origin, Vector3.down, out hit, 1)) return;
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box box)) return;
        box.StepOn(null);
    }
    protected virtual void BaseChecks()
    {
        if (weight == E_ObjectWeight.Not_Set)
            Debug.LogError($"The object: <{gameObject.name}> does not has a [weight].");
    }

    protected bool PawnCheck(Pawn pawn)
    {
        if (pawn == null)
        {
            // vv TEMP vv
            // Debug.LogError("[Pawn] not detected.");
            return false;
        }
        return true;
    }
    #endregion

    #region Eat & Split System
    public virtual void Eat(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;

        if (!eatable) return; // << Temp

        this.gameObject.SetActive(false);


        if (weight == E_ObjectWeight.Levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        if (!Physics.Raycast(origin, Vector3.down, out hit, 1)) return;
        GameObject objectHit = hit.collider.gameObject;

        if (!objectHit.TryGetComponent<Box>(out Box box)) return;
        box.StepOut(null);
    }

    public virtual void Split(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
        FreeFall();
        Repositioning(true);
        gameObject.SetActive(true);
    }
    #endregion

    #region Walkable System
    public virtual void StepOn(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;
    }

    public virtual void StepOut(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;
    }
    #endregion

    #region Teleport System
    public virtual bool HasPassThroughTeleportation(Pawn pawn)
    {
        PawnCheck(pawn);

        return true;
        // ^^ Need to be <true> for the Cave script 
    }/*
    public virtual void PassThroughTeleportation(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;
    }*/

    public virtual bool HasSetpOutTeleportation(Pawn pawn)
    {
        PawnCheck(pawn);

        if (OnSetpOutTeleportationOverride != null)
            return OnSetpOutTeleportationOverride.Invoke(pawn);

        return false;
    }/*
    public virtual void SetpOutTeleportation(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;
    }*/
    #endregion

    #region Physic System

    #region Temp-Update()
    private void Update()
    {
        if (gameObject.transform.position.y == 0
            || weight == E_ObjectWeight.Levitate) return;

        FreeFall();
        Repositioning(true);
    }
    #endregion

    void FreeFall()
    {
        if (weight == E_ObjectWeight.Levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        Vector3 newPosition = new Vector3(origin.x, 0, origin.z);
        this.transform.position = newPosition;

        if (!Physics.Raycast(origin, Vector3.down, out hit, 10.0f)) return;
        GameObject objectHit = hit.collider.gameObject;

        newPosition += transform.up * objectHit.transform.position.y + transform.up;
        this.transform.position = newPosition;

        if (!objectHit.TryGetComponent<Box>(out Box box)) return;
        box.StepOn(null);
    }
    #endregion

    #region Event System
    private void OnDisable()
    {
        if (OnSetpOutTeleportationOverride == null) return;
        OnSetpOutTeleportationOverride = null;
    }
    #endregion
}
