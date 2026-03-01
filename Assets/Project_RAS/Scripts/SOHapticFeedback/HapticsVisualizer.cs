using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class HapticsVisualizer : MonoBehaviour
{
    [Header("Runtime Debug Render")]
    [SerializeField] private bool renderInPlayMode = true;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.01f;

    [Header("Line smoothing")]
    [SerializeField, Range(0, 32)] private int numCapVertices = 8;
    [SerializeField, Range(0, 32)] private int numCornerVertices = 8;

    [Header("Force Bar (3D Wire Box)")]
    [SerializeField] private Transform collisionMesh;
    [SerializeField] private float barWidth = 0.01f;        // x and z size
    [SerializeField] private float barHeightScale = 0.05f;  // height = scale * MagForce
    [SerializeField] private float maxForce = 1.0f;         // for color mapping

    private HapticPlugin _haptic;
    private LineRenderer _lr;

    private MaterialPropertyBlock _mpb;
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");     // URP
    private static readonly int ColorIdFallback = Shader.PropertyToID("_Color"); // legacy

    // 12 edges * 2 points = 24
    private const int EdgePointCount = 24;

    // Cached corners (world) no allocations per frame
    private readonly Vector3[] _wc = new Vector3[8];

    private void Awake()
    {
        _haptic = GetComponent<HapticPlugin>();
        _lr = GetComponent<LineRenderer>();
        _mpb = new MaterialPropertyBlock();

        _lr.useWorldSpace = true;
        _lr.loop = false;
        _lr.positionCount = EdgePointCount;
        _lr.widthMultiplier = lineWidth;

        _lr.numCapVertices = numCapVertices;
        _lr.numCornerVertices = numCornerVertices;

        if (lineMaterial != null)
            _lr.material = lineMaterial;

        _lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lr.receiveShadows = false;

        if (collisionMesh == null && _haptic.CollisionMesh != null)
            collisionMesh = _haptic.CollisionMesh.transform;
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying || !renderInPlayMode)
        {
            _lr.enabled = false;
            return;
        }

        if (_haptic.MagForce <= 0f || collisionMesh == null)
        {
            _lr.enabled = false;
            return;
        }

        _lr.enabled = true;

        // optional live tweaking
        _lr.widthMultiplier = lineWidth;
        _lr.numCapVertices = numCapVertices;
        _lr.numCornerVertices = numCornerVertices;

        DrawForceBarWireBox3D();
    }

    private void DrawForceBarWireBox3D()
    {
        float magForce = _haptic.MagForce;

        float height = barHeightScale * magForce;

        // Same as your Gizmos cube:
        // position = CollisionMesh.pos + (0, height/2, 0)
        Vector3 center = collisionMesh.position + new Vector3(0f, height * 0.5f, 0f);

        Vector3 size = new Vector3(barWidth, height, barWidth);

        ApplyForceColorToMaterial(_haptic.CurrentForce.magnitude, maxForce);
        SetWireBoxPositions(center, size);
    }

    private void SetWireBoxPositions(Vector3 center, Vector3 size)
    {
        Vector3 e = size * 0.5f; // half extents

        // 8 corners in WORLD space (axis-aligned box)
        // bottom (y - ey)
        _wc[0] = new Vector3(center.x - e.x, center.y - e.y, center.z - e.z);
        _wc[1] = new Vector3(center.x + e.x, center.y - e.y, center.z - e.z);
        _wc[2] = new Vector3(center.x + e.x, center.y - e.y, center.z + e.z);
        _wc[3] = new Vector3(center.x - e.x, center.y - e.y, center.z + e.z);

        // top (y + ey)
        _wc[4] = new Vector3(center.x - e.x, center.y + e.y, center.z - e.z);
        _wc[5] = new Vector3(center.x + e.x, center.y + e.y, center.z - e.z);
        _wc[6] = new Vector3(center.x + e.x, center.y + e.y, center.z + e.z);
        _wc[7] = new Vector3(center.x - e.x, center.y + e.y, center.z + e.z);

        int p = 0;

        // bottom square
        SetEdge(ref p, 0, 1);
        SetEdge(ref p, 1, 2);
        SetEdge(ref p, 2, 3);
        SetEdge(ref p, 3, 0);

        // top square
        SetEdge(ref p, 4, 5);
        SetEdge(ref p, 5, 6);
        SetEdge(ref p, 6, 7);
        SetEdge(ref p, 7, 4);

        // vertical edges
        SetEdge(ref p, 0, 4);
        SetEdge(ref p, 1, 5);
        SetEdge(ref p, 2, 6);
        SetEdge(ref p, 3, 7);

        // Ensure count (in case changed in inspector)
        if (_lr.positionCount != EdgePointCount)
            _lr.positionCount = EdgePointCount;
    }

    private void SetEdge(ref int p, int a, int b)
    {
        _lr.SetPosition(p++, _wc[a]);
        _lr.SetPosition(p++, _wc[b]);
    }

    private void ApplyForceColorToMaterial(float currentForceMagnitude, float maxForceValue)
    {
        // Match your Gizmos calculation exactly
        float t = (maxForceValue > 0f) ? (currentForceMagnitude / maxForceValue) : 0f;

        Color c = new Color(
            2.0f * t,
            2.0f * (1.0f - t),
            0.0f
        );

        _lr.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorId, c);
        _mpb.SetColor(ColorIdFallback, c);
        _lr.SetPropertyBlock(_mpb);
    }
}