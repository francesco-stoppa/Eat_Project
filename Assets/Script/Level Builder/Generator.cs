using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Generator : MonoBehaviour
{
    [SerializeField] LevelSO level;

    public GameObject tilePrefab;
    public GameObject deteriorationTilePrefab;
    public GameObject pressureTilePrefab;

    void Awake()
    {
        if(CheckPrefabs()) return;
        Generate(level);
    }

    #region Prefabs
    bool CheckPrefabs()
    {
#if UNITY_EDITOR
        if (tilePrefab == null || deteriorationTilePrefab == null || pressureTilePrefab == null)
            GetTilePrefabs();
#endif
        if (tilePrefab == null || deteriorationTilePrefab == null || pressureTilePrefab == null)
        {
            Debug.LogError($"A [Tile] prefab is missing");
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    void GetTilePrefabs()
    {
        // Search Prefabs Assets/Prefabs
        string[] guids = AssetDatabase.FindAssets("DeteriorationTile t:prefab", new[] { "Assets/Art/Prefab" });

        // With this process I get every Prefab named Tile
        if (guids.Length == 0)
        {
            Debug.LogError("The prefab [DeteriorationTile] in the folder [Assets/Art/Prefab] not found!");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        deteriorationTilePrefab = prefab;

        // Search Prefabs Assets/Prefabs
        guids = AssetDatabase.FindAssets("PressureTile t:prefab", new[] { "Assets/Art/Prefab" });

        // With this process I get every Prefab named Tile
        if (guids.Length == 0)
        {
            Debug.LogError("The prefab [PressureTile] in the folder [Assets/Art/Prefab] not found!");
            return;
        }

        path = AssetDatabase.GUIDToAssetPath(guids[0]);
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        pressureTilePrefab = prefab;

        // Search Prefabs Assets/Prefabs
        guids = AssetDatabase.FindAssets("Tile t:prefab", new[] { "Assets/Art/Prefab" });

        // With this process I get every Prefab named Tile
        if (guids.Length == 0)
        {
            Debug.LogError("The prefab [Tile] in the folder [Assets/Art/Prefab] not found!");
            return;
        }

        path = AssetDatabase.GUIDToAssetPath(guids[0]);
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        tilePrefab = prefab;
    }
#endif
    #endregion

    void Generate(LevelSO levelToGenerated)
    {
        if (levelToGenerated == null) return;
        int i = 0;

        for (int x = 0; x < level.width; x++)
        {
            for (int y = 0; y < level.width; y++)
            {
                E_Lv_Base tileToCreate = level.cells[i].value;
                GameObject go = null;
                switch (tileToCreate)
                {
                    case E_Lv_Base.Tile:
                        go = tilePrefab;
                        break;
                    case E_Lv_Base.DeteriorationTile:
                        go = deteriorationTilePrefab;
                        break;
                    case E_Lv_Base.PressureTile:
                        go = pressureTilePrefab;
                        break;
                    case E_Lv_Base.Empty:
                        go = null;
                        break;
                    case E_Lv_Base.Not_Set:
                        go = null;
                        break;
                }

                if(i == 0 && tileToCreate == E_Lv_Base.Empty || i == 0 && tileToCreate == E_Lv_Base.Not_Set)
                {
                    Debug.LogWarning("The first Tile need to exist");
                    go = tilePrefab;
                }
                if (go != null)
                    Instantiate(go, new Vector3(x, 0, y), Quaternion.identity, transform);
                
                i++;
            }
        }
    }
}
