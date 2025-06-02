using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteExtractor : CustomWindow<SpriteExtractor>
{
    private Texture2D texture;

    [MenuItem("Tools/Sprite Extractor")]
    static void ShowWindow()
    {
        CreateComstomWindow("Sprite Extractor", new Vector2(290f, 260f), new Vector2(290f, 260f));
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Sprite Sheet", GUILayout.Width(175));
        if (texture != null && GUILayout.Button("Extract Sprites", GUILayout.Height(20)))
        {
            ExtractSprites();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        texture = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void ExtractSprites()
    {
        string path = AssetDatabase.GetAssetPath(texture);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

        string folderPath = EditorUtility.SaveFolderPanel("Select Output Folder", "", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        bool wasReadable = false;
        if (importer != null)
        {
            wasReadable = importer.isReadable;
            if (!importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }
        }
        else
        {
            Debug.LogError("Failed to get TextureImporter.");
            return;
        }

        int index = 0;
        foreach (Object asset in assets)
        {
            if (asset is Sprite sprite)
            {
                Rect rect = sprite.rect;

                Texture2D cropped = new Texture2D((int)rect.width, (int)rect.height);
                Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
                cropped.SetPixels(pixels);
                cropped.Apply();

                byte[] pngData = cropped.EncodeToPNG();
                if (pngData != null)
                {
                    string fileName = Path.Combine(folderPath, sprite.name + ".png");
                    File.WriteAllBytes(fileName, pngData);
                    index++;
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Sprite export complete: " + index + " sprites saved.");

        if (!wasReadable)
        {
            importer.isReadable = false;
            importer.SaveAndReimport();
        }
    }

    protected override void OnSceneGUI(SceneView sceneView)
    {
        
    }
}
