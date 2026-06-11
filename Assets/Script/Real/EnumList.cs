public enum E_Direction
{
    Not_Set,
    Forward,
    Back,
    Left,
    Right
}

public enum E_Commands
{
    Not_Set,
    Move,
    EatOrSplit,
    ResetLevel
}

public enum E_StepStatus
{
    OnStepOn,
    OnStepOut
}

public enum E_ObjectWeight
{
    Not_Set,

    // object
    // light,
    Heavy,

    // special 
    Small, // not detected by the fatness
    Levitate // bello un oggetto che fa quasi levitare il player così da non essere recepito dal counter delle DeteriorationTile
}

public enum E_ExitDirection
{
    Not_Set,
    Deadend,
    Corridor,
    OneTurn,
    CorridorAndOneTurn,
    EveryDirection
}

