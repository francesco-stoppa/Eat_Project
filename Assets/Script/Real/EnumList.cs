public enum E_Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public enum E_Commands
{
    None,
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
    none,

    // object
    // light,
    heavy,

    // special 
    small, // not detected by the fatness
    levitate // bello un oggetto che fa quasi levitare il player così da non essere recepito dal counter delle DeteriorationTile
}


