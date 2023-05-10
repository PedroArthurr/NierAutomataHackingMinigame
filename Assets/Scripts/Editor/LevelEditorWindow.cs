using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    #region Variables
    private int gridSize = 30;
    private GameObject prefab;
    private GamePrefabs gamePrefabs;
    private bool eraserMode = false;
    private enum ObjectCategory { LevelObjects, Enemies, Player }
    private ObjectCategory selectedCategory;
    private List<List<GameObject>> levelGrid;
    private string saveFileName = "level.json";
    private string loadFileName = "level.json";
    private float cellSize;
    #endregion

    #region Window Setup
    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelEditorWindow));
    }
    #endregion

    #region GUI Layout
    private void OnGUI()
    {
        gridSize = EditorGUILayout.IntField("Grid Size", gridSize);

        DisplayGamePrefabsField();

        DisplayPrefabField();

        GUILayout.Space(20);

        DisplayButtons();

        GUILayout.Space(20);

        DisplayLevelGrid();

        HandleMouseEvents();
    }
    #endregion

    #region Button Layouts
    private void DisplayButtons()
    {
        EditorGUILayout.BeginHorizontal();
        DisplayStyledButtons();
        DisplayEraserButton();
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayEraserButton()
    {
        eraserMode = GUILayout.Toggle(eraserMode, "Eraser Mode", "Button");
    }

    private void DisplayStyledButtons()
    {
        if (GUILayout.Button("New Level"))
        {
            if (EditorUtility.DisplayDialog("New Level", "Are you sure you want to create a new level? Any unsaved changes will be lost.", "Yes", "No"))
            {
                NewLevel();
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Level"))
        {
            SaveLevel();
        }
        saveFileName = GUILayout.TextField(saveFileName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Level"))
        {
            LoadLevel();
        }
        loadFileName = GUILayout.TextField(loadFileName);
        GUILayout.EndHorizontal();
    }

    #endregion

    #region Prefab Field
    private void DisplayGamePrefabsField()
    {
        gamePrefabs = (GamePrefabs)EditorGUILayout.ObjectField("Game Prefabs", gamePrefabs, typeof(GamePrefabs), false);
    }

    private void DisplayPrefabField()
    {
        if (gamePrefabs != null)
        {
            selectedCategory = (ObjectCategory)EditorGUILayout.EnumPopup("Object Category", selectedCategory);

            List<LevelObject> currentList;

            switch (selectedCategory)
            {
                case ObjectCategory.LevelObjects:
                    currentList = gamePrefabs.levelObjects;
                    break;
                case ObjectCategory.Enemies:
                    currentList = gamePrefabs.enemies;
                    break;
                case ObjectCategory.Player:
                    currentList = gamePrefabs.player;
                    break;
                default:
                    currentList = new List<LevelObject>();
                    break;
            }

            int selectedIndex = -1;

            if (prefab != null && prefab.GetComponent<LevelObject>() != null)
            {
                string prefabGUID = prefab.GetComponent<LevelObject>().objectGUID;
                selectedIndex = currentList.FindIndex(obj => obj.objectGUID == prefabGUID);
            }

            GUIContent[] objectIcons = currentList.ConvertAll(obj => new GUIContent(obj.name, AssetPreview.GetAssetPreview(obj.gameObject))).ToArray();

            GUIStyle dropdownStyle = new GUIStyle(EditorStyles.popup);
            dropdownStyle.fixedHeight = 40; // Aumenta a altura do campo "Object"

            selectedIndex = EditorGUILayout.Popup(new GUIContent("Object"), selectedIndex, objectIcons, dropdownStyle);

            if (selectedIndex != -1)
            {
                prefab = currentList[selectedIndex].gameObject;
            }
            else
            {
                prefab = null;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please select a Game Prefabs asset to display the list of objects.", MessageType.Info);
        }
    }

    #endregion

    #region Mouse Events
    private void HandleMouseEvents()
    {
        Event e = Event.current;
        Rect gridRect = new Rect(10f, 170f, gridSize * cellSize, gridSize * cellSize);

        if (gridRect.Contains(e.mousePosition))
        {
            int i = Mathf.FloorToInt((e.mousePosition.y - gridRect.y) / cellSize);
            int j = Mathf.FloorToInt((e.mousePosition.x - gridRect.x) / cellSize);

            if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
            {
                // On left mouse button down or drag, create or erase object
                if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
                {
                    if (eraserMode)
                    {
                        EraseObject(i, j);
                    }
                    else
                    {
                        CreateObject(i, j);
                    }
                    e.Use();
                }
                // On right mouse button down, erase object
                else if (e.type == EventType.MouseDown && e.button == 1)
                {
                    EraseObject(i, j);
                    e.Use();
                }
            }
        }
    }

    #endregion

    #region Grid Display Objects
    private void CreateObject(int i, int j)
    {
        if (eraserMode)
        {
            return;
        }

        if (levelGrid[i][j] != null)
        {
            DestroyImmediate(levelGrid[i][j]);
        }

        if (prefab != null)
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObject.transform.position = new Vector3((position.width - gridSize * cellSize) / 2f + j * cellSize + cellSize / 2f, 100f + i * cellSize + cellSize / 2f, 0f);
            levelGrid[i][j] = newObject;
        }
    }

    private void ClearLevel()
    {
        if (levelGrid != null)
        {
            for (int i = 0; i < levelGrid.Count; i++)
            {
                for (int j = 0; j < levelGrid[i].Count; j++)
                {
                    if (levelGrid[i][j] != null)
                    {
                        DestroyImmediate(levelGrid[i][j]);
                    }
                }
            }
        }

        levelGrid = null;
    }

    private void DisplayLevelGrid()
    {
        if (levelGrid != null)
        {
            cellSize = Mathf.Min(position.width - 20f, position.height - 170f) / gridSize;

            Rect gridRect = new Rect(10f, 170f, gridSize * cellSize, gridSize * cellSize);
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
        }
    }
    private void EraseObject(int i, int j)
    {
        if (levelGrid[i][j] != null)
        {
            DestroyImmediate(levelGrid[i][j]);
            levelGrid[i][j] = null;
        }
    }
    #endregion

    #region Saving and Loading
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

        string path = Application.dataPath + "/" + saveFileName;
        System.IO.File.WriteAllText(path, json);

        Debug.Log("Saved level to: " + path);
        EditorUtility.DisplayDialog("Level Saved", "Level saved to: " + path, "OK");
    }

    private void LoadLevel()
    {
        string filePath = Application.dataPath + "/" + loadFileName;
        if (System.IO.File.Exists(filePath))
        {
            ClearLevel();

            string json = System.IO.File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            gridSize = levelData.gridSize;
            NewLevel();

            if (levelData != null)
            {
                foreach (LevelPrefab levelPrefab in levelData.objects)
                {
                    if (levelPrefab != null)
                    {
                        GameObject prefabObject = null;

                        foreach (LevelObject levelObject in gamePrefabs.levelObjects.Concat(gamePrefabs.enemies).Concat(gamePrefabs.player))
                        {
                            if (levelObject.objectGUID == levelPrefab.prefabGUID)
                            {
                                prefabObject = levelObject.gameObject;
                                break;
                            }
                        }
                        if (prefabObject != null)
                        {
                            int i = levelPrefab.position.x;
                            int j = levelPrefab.position.y;

                            Debug.Log("Instantiating object at position: " + i + ", " + j);

                            GameObject newObject = Instantiate(prefabObject, new Vector3(j * cellSize + cellSize / 2f, i * cellSize + cellSize / 2f, 0f), Quaternion.identity);
                            levelGrid[i][j] = newObject;

                            Debug.Log("Object instantiated: " + newObject.name);
                        }
                        else
                        {
                            Debug.LogError("Prefab object not found for GUID: " + levelPrefab.prefabGUID);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Null LevelPrefab found while loading level");
                    }
                }
            }
            EditorUtility.DisplayDialog("Level Loaded", "Level loaded from: " + filePath, "OK");
            Repaint();
        }
        else
        {
            Debug.LogError("Level file not found: " + filePath);
            EditorUtility.DisplayDialog("Error", "Level file not found: " + filePath, "OK");
        }
    }

    #endregion
}
