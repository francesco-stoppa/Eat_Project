using UnityEngine;

public class DeteriorationTile : Obstacle
{
    [Tooltip("The tile will destroy on stepping out of it \nbecause i think its better")]
    [SerializeField] int howManyTimeCanWalkOnIt;
    int currentCounter;

    public override void SetpOn(Bob bob)
    {
        // ...
    }

    public override void SetpOut(Bob bob)
    {
        if (bob == null) return;

        currentCounter++;
        if (currentCounter < howManyTimeCanWalkOnIt) return;
        Destroy(this.gameObject);
    }
}
