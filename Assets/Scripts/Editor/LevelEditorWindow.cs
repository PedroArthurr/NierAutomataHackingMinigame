using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private int gridSize = 30;
    private GameObject prefab;
    private List<List<GameObject>> levelGrid;
    private string saveFileName = "level.json";

    private float cellSize;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelEditorWindow));
    }

    private void OnGUI()
    {
        gridSize = EditorGUILayout.IntField("Grid Size", gridSize);

        if (prefab != null && prefab.GetComponent<LevelObject>() == null)
        {
            EditorGUILayout.HelpBox("O prefab selecionado não contém o componente LevelObject.", MessageType.Warning);
            prefab = null;
        }

        if (prefab == null || prefab.GetComponent<LevelObject>() != null)
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        }

        if (GUILayout.Button("New Level"))
        {
            NewLevel();
        }

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel();
        }

        if (levelGrid != null)
        {
            cellSize = Mathf.Min(position.width, position.height) / gridSize;

            Rect gridRect = new Rect((position.width - gridSize * cellSize) / 2f, 100f, gridSize * cellSize, gridSize * cellSize);
            GUI.Box(gridRect, "");

            for (int i = 0; i < levelGrid.Count; i++)
            {
                for (int j = 0; j < levelGrid[i].Count; j++)
                {
                    if (levelGrid[i][j] != null)
                    {
                        EditorGUI.DrawPreviewTexture(new Rect(gridRect.x + j * cellSize, gridRect.y + i * cellSize, cellSize, cellSize), AssetPreview.GetAssetPreview(levelGrid[i][j]));
                    }
                }
            }

            Event e = Event.current;
            if (gridRect.Contains(e.mousePosition))
            {
                int i = Mathf.FloorToInt((e.mousePosition.y - gridRect.y) / cellSize);
                int j = Mathf.FloorToInt((e.mousePosition.x - gridRect.x) / cellSize);

                if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
                {
                    if (e.type == EventType.MouseDown && e.button == 0)
                    {
                        CreateObject(i, j);
                        e.Use();
                    }
                    else if (e.type == EventType.MouseDrag && e.button == 0)
                    {
                        CreateObject(i, j);
                        e.Use();
                    }
                }
            }
        }
    }

    private void NewLevel()
    {
        levelGrid = new List<List<GameObject>>();

        for (int i = 0; i < gridSize; i++)
        {
            List<GameObject> row = new List<GameObject>();
            for (int j = 0; j < gridSize; j++)
            {
                row.Add(null);
            }
            levelGrid.Add(row);
        }

        Repaint();
    }
    private void CreateObject(int i, int j)
    {
        if (levelGrid[i][j] != null)
        {
            DestroyImmediate(levelGrid[i][j]);
        }

        if (prefab != null)
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObject.transform.position = new Vector3(j * cellSize + cellSize / 2f, i * cellSize + cellSize / 2f, 0f);
            levelGrid[i][j] = newObject;
        }
    }

    private void SaveLevel()
    {
        LevelData levelData = new LevelData();
        levelData.gridSize = gridSize;
        levelData.objects = new List<LevelPrefab>();

        for (int i = 0; i < levelGrid.Count; i++)
        {
            for (int j = 0; j < levelGrid[i].Count; j++)
            {
                if (levelGrid[i][j] != null)
                {
                    LevelObject levelObject = levelGrid[i][j].GetComponent<LevelObject>();
                    if (levelObject != null)
                    {
                        LevelPrefab levelPrefab = new LevelPrefab();
                        levelPrefab.position = new Vector2Int(i, j);
                        levelPrefab.prefabGUID = levelObject.objectGUID;
                        levelData.objects.Add(levelPrefab);
                    }
                }
            }
        }

        string json = JsonUtility.ToJson(levelData, true);
        System.IO.File.WriteAllText(Application.dataPath + "/" + saveFileName, json);
    }

    [Serializable]
    public class LevelData
    {
        public int gridSize;
        public List<LevelPrefab> objects;
    }

    [Serializable]
    public class LevelPrefab
    {
        public Vector2Int position;
        public string prefabGUID;
    }
}