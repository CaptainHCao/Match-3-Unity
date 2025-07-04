using UnityEngine;
using UnityEditor;

public class SpriteChanger : EditorWindow
{
    public TextureSet skinSet;
    public PrefabSet prefabSet;

    [MenuItem("Tools/Texture Set Assigner")]
    public static void ShowWindow()
    {
        GetWindow<SpriteChanger>("Texture Set Assigner");
    }

    private void OnGUI()
    {
        skinSet = (TextureSet)EditorGUILayout.ObjectField("Item Skin Set (SO)", skinSet, typeof(TextureSet), false);
        prefabSet = (PrefabSet)EditorGUILayout.ObjectField("Prefab Set (SO)", prefabSet, typeof(PrefabSet), false);

        EditorGUILayout.Space();

        if (skinSet == null || prefabSet == null)
        {
            EditorGUILayout.HelpBox("Assign both a TextureSet and a PrefabSet.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField($"Number of sprites: {skinSet.normalItemSprites?.Length ?? 0}");
        EditorGUILayout.LabelField($"Number of prefabs: {prefabSet.itemPrefabSet?.Length ?? 0}");

        EditorGUILayout.Space();
        if (GUILayout.Button("Assign Sprites from SO"))
        {
            AssignSpritesFromSO();
        }
    }

    void AssignSpritesFromSO()
    {
        if (skinSet == null || prefabSet == null)
        {
            Debug.LogError("TextureSet or PrefabSet is not assigned!");
            return;
        }

        var sprites = skinSet.normalItemSprites;
        var prefabs = prefabSet.itemPrefabSet;

        if (sprites == null || prefabs == null)
        {
            Debug.LogError("Sprites or Prefabs array is null!");
            return;
        }

        if (sprites.Length != prefabs.Length)
        {
            Debug.LogError("Number of item sprites doesn't match number of prefabs");
            return;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] == null || prefabs[i] == null) continue;

            SpriteRenderer sr = prefabs[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Undo.RecordObject(sr, "Assign Sprite from SO");
                sr.sprite = sprites[i];
                EditorUtility.SetDirty(sr);
                EditorUtility.SetDirty(prefabs[i]);
                Debug.Log($"Assigned sprite {sprites[i].name} to prefab {prefabs[i].name}");
            }
            else
            {
                Debug.LogWarning($"No SpriteRenderer found on prefab {prefabs[i].name}");
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Sprites assigned from Scriptable Object");
    }
}
