using UnityEngine;

public class LastTile : Box
{
    #region Walkable System
    public override void StepOn(Pawn pawn)
    {
        base.StepOn(pawn);
        Debug.Log("YOU WIN! \nChill just this level.");
    }
    #endregion
}
