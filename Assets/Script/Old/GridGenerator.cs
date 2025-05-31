using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    Vector3 gridPos; // <-- maybe the programmer want to make the grid in other position

    [Tooltip("Axis X")]
    [SerializeField] int columns;
    [Tooltip("Axis Y")]
    [SerializeField] int rows;
    // Object for the costruction of the level
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject winColumns;

    void Start()
    {
        gridPos = transform.position;
        for (int i = 0; i <= rows - 1; i++)
        {
            for (int j = 0; j <= columns - 1; j++)
            {
                Instantiate<GameObject>(tilePrefab, gridPos + new Vector3(i, 0, j), new Quaternion(), this.gameObject.transform);
            }
        }
        Instantiate<GameObject>(tilePrefab, gridPos + new Vector3(rows - 1, 0, columns), new Quaternion(), this.gameObject.transform);
        winColumns.transform.position = new Vector3(rows - 1, 1, columns);
    }
}
