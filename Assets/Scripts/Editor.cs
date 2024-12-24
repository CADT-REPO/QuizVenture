using UnityEngine;
using UnityEditor;

public class MissingScriptFinder : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptFinder>("Find Missing Scripts");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in Scene"))
        {
            FindMissingScriptsInScene();
        }
        if (GUILayout.Button("Find Missing Scripts in Prefabs"))
        {
            FindMissingScriptsInPrefabs();
        }
    }

    private void FindMissingScriptsInScene()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        int missingCount = 0;

        foreach (GameObject go in objects)
        {
            if (CheckMissingScripts(go))
            {
                missingCount++;
            }
        }

        Debug.Log(missingCount > 0 ? $"{missingCount} GameObjects with missing scripts found in the scene." : "No missing scripts found in the scene.");
    }

    private void FindMissingScriptsInPrefabs()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int missingCount = 0;

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (CheckMissingScripts(prefab))
            {
                missingCount++;
            }
        }

        Debug.Log(missingCount > 0 ? $"{missingCount} prefabs with missing scripts found in the project." : "No missing scripts found in the project.");
    }

    private bool CheckMissingScripts(GameObject go)
    {
        Component[] components = go.GetComponents<Component>();
        bool missingScriptFound = false;

        foreach (Component component in components)
        {
            if (component == null)
            {
                Debug.LogError($"Missing script found on GameObject: {GetGameObjectPath(go)}", go);
                missingScriptFound = true;
            }
        }

        return missingScriptFound;
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;

        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }
}
