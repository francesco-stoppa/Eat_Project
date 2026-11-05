using UnityEngine;

public class LastTile : Box
{
    #region Eat & Split System
    public override void Eat(Pawn pawn)
    {
        base.Eat(pawn);
        Debug.Log("YOU WIN! \nChill just this level.");
    }
    #endregion
}
