using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrawGizmo : MonoBehaviour
{

    public enum GizmoType { Cube, Sphere, Sprite }

    #pragma warning disable 414 // To disable the "assigned but never used" warning
    [SerializeField]
    private GizmoType gizmoType = GizmoType.Cube;

    [SerializeField]
    private Color color = new Color(1, 0, 0, 0.5f);

    [SerializeField]
    private bool wire = false;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private string spriteGUID;
    #pragma warning restore 414


    #if !UNITY_EDITOR
    private void Awake()
    { 
        Destroy(this);
    }
    #endif

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 targetPosition = transform.position + offset;

        Gizmos.color = color;
        switch(gizmoType)
        {
            case GizmoType.Cube:
                if (wire) Gizmos.DrawWireCube(targetPosition, Vector3.one * radius);
                else Gizmos.DrawCube(targetPosition, Vector3.one * radius);
                break;
            case GizmoType.Sphere:
                if (wire) Gizmos.DrawWireSphere(targetPosition, radius);
                else Gizmos.DrawSphere(targetPosition, radius);
                break;
            case GizmoType.Sprite:
                Gizmos.DrawIcon(targetPosition, GetSpriteName(), true);
                break;
            default:
                break;
        }
    }

    public GizmoType GetGizmoType()
    {
        return gizmoType;
    }

    public string GetSpriteGUID()
    {
        return spriteGUID;
    }

    public void SetSpriteGUID(string spriteGUID)
    {
        this.spriteGUID = spriteGUID;
        SceneView.RepaintAll(); // To call OnDrawGizmos
    }

    private string GetSpriteName()
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(spriteGUID);

        int lastIndexOfSlash = assetPath.LastIndexOf('/');

        if (lastIndexOfSlash != -1)
        {
            return assetPath.Substring(assetPath.LastIndexOf('/'));
        }

        return string.Empty;
    }
    #endif
}