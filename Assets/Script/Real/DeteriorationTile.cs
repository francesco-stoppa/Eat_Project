using UnityEngine;

public class DeteriorationTile : Box
{
    [Tooltip("The tile will destroy on stepping out of it \nbecause i think its better")]
    [SerializeField] int howManyTimeCanWalkOnIt;
    int currentCounter;


    #region Base System
    protected override void BaseChecks()
    {
        base.BaseChecks();

        if (howManyTimeCanWalkOnIt > 0) return;

        Debug.LogError($"The [Pawn] can not walk on this [{gameObject.name}] tile. \nTile intern counter: <{howManyTimeCanWalkOnIt}>");
        Destroy(gameObject);
    }

    #endregion

    #region Walkable System
    public override void SetpOut(Pawn pawn)
    {
        base.SetpOut(pawn);

        currentCounter++;
        if (currentCounter < howManyTimeCanWalkOnIt) return;
        Destroy(gameObject);
    }
    #endregion
}
