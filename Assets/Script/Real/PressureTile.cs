using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.UI.Image;

public class PressureTile : Box
{
    [SerializeField] GameObject objectToHide;
    [SerializeField] E_StepStatus whenHide;

    void Start()
    {
        if (whenHide != E_StepStatus.OnStepOut) return;
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1)) return;

        SetpOut(null);
    }
    #region Base System
    protected override void BaseChecks()
    {
        if (whenHide == E_StepStatus.Not_Set)
            Debug.LogError("The [When to hide] is not set.");

        if (objectToHide == null)
            Debug.LogError("The [Object to hide] is missing. \nThere is not a GameObject set.");
    }
    #endregion

    #region Walkable System
    public override void SetpOn(Pawn pawn)
    {
        // base.SetpOn(pawn);
        BaseChecks();

        objectToHide.SetActive(whenHide != E_StepStatus.OnStepOn);
    }

    public override void SetpOut(Pawn pawn)
    {
        // base.SetpOut(pawn);
        BaseChecks();

        objectToHide.SetActive(whenHide == E_StepStatus.OnStepOn);
    }
    #endregion
}
