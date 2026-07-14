using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSO))]
public class LevelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelSO grid = (LevelSO)target;
        
        grid.width = EditorGUILayout.IntField("Width", grid.width);

        int requiredSize = grid.width * grid.width;

        if (grid.cells == null || grid.cells.Length != requiredSize)
        {
            System.Array.Resize(ref grid.cells, requiredSize);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid");

        for (int y = 0; y < grid.width; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < grid.width; x++)
            {
                int index = y * grid.width + x;

                if (grid.cells[index] == null)
                    grid.cells[index] = new CellData();

                grid.cells[index].value = (E_Lv_Base /*<<LOOK*/)EditorGUILayout.EnumPopup(
                grid.cells[index].value,
                GUILayout.Width(100));
            }

            EditorGUILayout.EndHorizontal();
        }        

        if (GUI.changed)
        {
            EditorUtility.SetDirty(grid);
        }
    }
}