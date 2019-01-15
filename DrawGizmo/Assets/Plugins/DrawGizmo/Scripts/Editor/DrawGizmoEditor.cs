#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(DrawGizmo))]
public class DrawGizmoEditor : Editor
{

    private SerializedProperty gizmoType;
    private SerializedProperty color;
    private SerializedProperty wire;
    private SerializedProperty offset;
    private SerializedProperty radius;

    private string gizmosFolderPath;
    private List<string> sprites;

    private readonly string[] supportedExtensions = new string[] { "png", "tiff" };

    private int spriteIndex = 0;


	void OnEnable()
	{
        DrawGizmo drawGizmo = (DrawGizmo)target;

        gizmoType = serializedObject.FindProperty("gizmoType");
        color = serializedObject.FindProperty("color");
        wire = serializedObject.FindProperty("wire");
        offset = serializedObject.FindProperty("offset");
        radius = serializedObject.FindProperty("radius");

        gizmosFolderPath = "Assets/Gizmos";
        string[] files = Directory.GetFiles(gizmosFolderPath);

        sprites = new List<string>();

        foreach(string file in files)
        {
            string fileName = Path.GetFileName(file);

            if (HasExtension(fileName))
            {
                sprites.Add(fileName);

                string guid = AssetDatabase.AssetPathToGUID(gizmosFolderPath + "/" + fileName);

                if (guid == drawGizmo.GetSpriteGUID()) // If a sprite was previously set, get back to it
                {
                    spriteIndex = sprites.Count - 1;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawGizmo drawGizmo = (DrawGizmo)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(gizmoType);

        if (drawGizmo.GetGizmoType() == DrawGizmo.GizmoType.Cube || drawGizmo.GetGizmoType() == DrawGizmo.GizmoType.Sphere)
        {
            EditorGUILayout.PropertyField(color);
            EditorGUILayout.PropertyField(wire);
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(radius);
        }
        else if (drawGizmo.GetGizmoType() == DrawGizmo.GizmoType.Sprite)
        {
            spriteIndex = EditorGUILayout.Popup("Sprite", spriteIndex, sprites.ToArray());
            drawGizmo.SetSpriteGUID(AssetDatabase.AssetPathToGUID(gizmosFolderPath + "/" + sprites[spriteIndex]));
            EditorGUILayout.PropertyField(offset);
            string helpBoxTxt = string.Format("Sprites must be in {0} folder.\nSupported file formats are: {1}.", gizmosFolderPath, supportedExtensions.ToStringAll(", "));
            EditorGUILayout.HelpBox(helpBoxTxt, MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private bool HasExtension(string fileName)
    {
        string extension = fileName.Substring(fileName.LastIndexOf('.') + 1); // + 1 to remove '.' character
        return supportedExtensions.Contains(extension);
    }
}
#endif