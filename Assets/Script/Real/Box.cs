using System;
using UnityEngine;
using UnityEngine.Windows;

public class Box : MonoBehaviour
{
    [Tooltip("Small object dont make you fat")]
    public E_ObjectWeight weight;
    [Tooltip("The Pawn can walk on this object?")]
    public bool walkable = true;
    [Tooltip("The Pawn can eat this object?")]
    public bool eatable = true;

    public Func<Pawn, bool> OnSetpOutTeleportationOverride;

    void Awake()
    {
        Repositioning();
        BaseChecks();
    }

    #region Base System
    protected void Repositioning()
    {
        transform.position = Vector3Int.RoundToInt(transform.position);
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
            Debug.LogError("[Pawn] not detected.");
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

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        obstacle.SetpOut();
    }

    public virtual void Split(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
        FreeFall();
        Repositioning();
        gameObject.SetActive(true);
    }
    #endregion

    #region Walkable System
    public virtual void SetpOn(Pawn pawn)
    {
        if (!PawnCheck(pawn)) return;
    }

    public virtual void SetpOut(Pawn pawn)
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
    }
    #endregion

    void FreeFall()
    {
        if (weight == E_ObjectWeight.Levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        Vector3 newPosition = new Vector3(origin.x, 0, origin.z);
        this.transform.position = newPosition;


        if (!Physics.Raycast(origin, Vector3.down, out hit, origin.y)) return;
        GameObject objectHit = hit.collider.gameObject;

        newPosition += transform.up * objectHit.transform.position.y + transform.up;
        this.transform.position = newPosition;

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        obstacle.SetpOn();
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
