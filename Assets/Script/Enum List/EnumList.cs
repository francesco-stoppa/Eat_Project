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
    Not_Set,
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

public enum E_Box
{
    Not_Set,
    Tile,
    DeteriorationTile,
    PressureTile,
    Portal,
    Stair,
    Wall,
    Corridor,
    CaveCorridor,
    CaveTurn,
    CaveDeadend
}

public enum E_Lv_Base
{
    Not_Set,
    Empty,
    Tile,
    DeteriorationTile,
    PressureTile // can't place because i need to bind the object to hide on pressed
}

