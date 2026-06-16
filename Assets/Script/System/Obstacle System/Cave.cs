using UnityEngine;

public class Cave : Box
{
    [Tooltip("It has always one enter (the forward).")]
    [SerializeField] E_ExitDirection exitDirection;
    [Tooltip("The Pawn can pass through while is fat?")]
    [SerializeField] bool fatCanPass;
    Box bindBox;

    void Start()
    {
        BindEvent();
    }

    #region Base System
    protected override void BaseChecks()
    {
        base.BaseChecks();
        if (exitDirection == E_ExitDirection.Not_Set)
            Debug.LogError("The [Exit Direction] is not set.");
    }
    #endregion

    #region Teleport System
    public override bool HasPassThroughTeleportation(Pawn pawn)
    {
        return CheckTeleportation(pawn, true);
    }

    public bool OnHasSetpOutTeleportationOverride(Pawn pawn)
    {
        return CheckTeleportation(pawn);
    }

    bool CheckTeleportation(Pawn pawn, bool isPassingThrough = false)
    {
        PawnCheck(pawn);

        if (exitDirection == E_ExitDirection.Not_Set)
        {
            Debug.LogError("REMINDER: the [Exit Direction] is not set");
            return true;
        }

        if (!fatCanPass && pawn.HaveFullStomach()) return true;
        if (exitDirection == E_ExitDirection.EveryDirection) return false;

        // Every exitDirection has an enter (the forward)
        int roundDot = Mathf.RoundToInt(Vector3.Dot(transform.forward, pawn.gameObject.transform.forward));
        int dotCheck = 1;
        if (isPassingThrough) dotCheck = -1;

        if (roundDot == dotCheck) return false;

        dotCheck = -1;
        if (isPassingThrough) dotCheck = 1;

        if (exitDirection == E_ExitDirection.Corridor && roundDot == dotCheck ||
            exitDirection == E_ExitDirection.CorridorAndOneTurn && roundDot == dotCheck) return false;

        roundDot = Mathf.RoundToInt(Vector3.Dot(pawn.gameObject.transform.forward, transform.right));
        if (exitDirection == E_ExitDirection.OneTurn && roundDot == dotCheck ||
            exitDirection == E_ExitDirection.CorridorAndOneTurn && roundDot == dotCheck) return false;

        return true;
    }
    #endregion

    #region Eat & Split System
    public override void Eat(Pawn pawn)
    {
        base.Eat(pawn);
        BreakEvent();
    }

    public override void Split(Vector3 newPosition)
    {
        base.Split(newPosition);
        BindEvent();
    }
    #endregion

    #region Bind Events
    void BindEvent()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, -transform.up, out hit, 1f)) return;
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box standBox)) return;
        bindBox = standBox;
        bindBox.OnSetpOutTeleportationOverride += OnHasSetpOutTeleportationOverride;
    }

    void BreakEvent()
    {
        if (bindBox == null) return;
        bindBox.OnSetpOutTeleportationOverride -= OnHasSetpOutTeleportationOverride;
    }

    private void OnDisable()
    {
        BreakEvent();
    }
    #endregion
}
