using UnityEngine;

/*
public class Tile
{
    public E_Box boxType;
    public Vector3 position;
    [Range(0, 100)]
    public int rotation;

    // sometile need to be bind
    public bool useBind = false;
    [Tooltip("Print the number of the element you want to bind")]
    public int bindId;

    public Tile(E_Box type, Vector3 position, int rotation, bool useBind = false, int bindId = 0)
    {
        boxType = type;
        position = this.position;
        rotation = this.rotation;
        useBind = this.useBind;
        bindId = this.bindId;
    }
}*/

/*
// [System.Serializable]
[System.Serializable]
public class level
{
#if UNITY_EDITOR
    [HideInInspector] public bool showBoard;
#endif
    public int row = 3;
    public int column = 3;
    public E_Box[,] board = new E_Box[3, 3];
}*/

[System.Serializable]
public class CellData
{
    public E_Lv_Base value;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Level")]
public class LevelSO : ScriptableObject
{
    [Tooltip("The first number rapresent the world." +
        "\nThe second number rapresent the level.")]
    public Vector2Int levelIndex;

    // public Tile[] elementiInScene;

    // public level[] allLevels;


    public int width = 3;

    public CellData[] cells;
}

