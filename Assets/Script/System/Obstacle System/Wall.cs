using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Box
{
    #region Base System
    protected override void Repositioning(bool callAfterFreeFall = false)
    {
        return;
    }
    #endregion
}
