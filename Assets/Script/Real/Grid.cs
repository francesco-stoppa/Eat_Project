using UnityEngine;

public class Grid : MonoBehaviour
{
    // [Tooltip("")]
    [SerializeField] GameObject tilePrefab;

    [Tooltip("Axis X")]
    [SerializeField] int rows;
    [Tooltip("Axis Z")]
    [SerializeField] int columns;

    [SerializeField] bool createGridOnStart;

    private void Start()
    {
        if(!createGridOnStart) return;
        GridCreation();
    }

    public void GridCreation()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("The [tile prefab] is missing.");
            return;
        }

        // Creation grid
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject tileGO = Instantiate(tilePrefab, new Vector3(x, 0, y),
                    Quaternion.identity, transform);
            }
        }
    }
}

