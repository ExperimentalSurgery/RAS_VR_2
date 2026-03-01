using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class HapticsVisualizer : MonoBehaviour
{
    [Header("Runtime Debug Render")]
    [SerializeField] private bool renderInPlayMode = true;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.01f;

    [Header("Line smoothing")]
    [SerializeField, Range(0, 32)] private int numCapVertices = 8;      // round line ends
    [SerializeField, Range(0, 32)] private int numCornerVertices = 8;   // round corners/joins

    [Header("Force Bar")]
    [SerializeField] private Transform collisionMesh;     // assign CollisionMesh.transform here (or auto-pull from plugin if you prefer)
    [SerializeField] private float barWidth = 0.01f;      // x/z size in your gizmo code
    [SerializeField] private float barHeightScale = 0.05f; // the "0.05f * MagForce" factor
    [SerializeField] private float maxForce = 1.0f;       // used for color mapping, replace with your MaxForce

    private HapticPlugin _haptic;
    private LineRenderer _lr;

    // We draw a single rectangle loop with 5 points (closing point repeats first).
    private const int RectPointCount = 5;
    private readonly Vector3[] _pts = new Vector3[RectPointCount];

    private MaterialPropertyBlock _mpb;
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor"); // URP
    private static readonly int ColorIdFallback = Shader.PropertyToID("_Color"); // legacy
    private void Awake()
    {
        _haptic = GetComponent<HapticPlugin>();

        _lr = GetComponent<LineRenderer>();

        _mpb = new MaterialPropertyBlock();

        if (_lr == null) _lr = gameObject.AddComponent<LineRenderer>();

        _lr.useWorldSpace = true;
        _lr.loop = false; // we close manually with point 0 repeated
        _lr.positionCount = RectPointCount;
        _lr.widthMultiplier = lineWidth;

        _lr.numCapVertices = numCapVertices;
        _lr.numCornerVertices = numCornerVertices;

        if (lineMaterial != null)
            _lr.material = lineMaterial;

        _lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lr.receiveShadows = false;

        // If you didn’t assign collisionMesh in inspector, try to find it from plugin.
        if (collisionMesh == null && _haptic.CollisionMesh != null)
            collisionMesh = _haptic.CollisionMesh.transform;

        // If your plugin has MaxForce field, you can read it here instead:
        // maxForce = (float)_haptic.MaxForce;
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying || !renderInPlayMode)
        {
            _lr.enabled = false;
            return;
        }

        // Mirror your condition
        if (_haptic.MagForce <= 0f || collisionMesh == null)
        {
            _lr.enabled = false;
            return;
        }

        _lr.enabled = true;
        DrawForceBarOutline();
    }

    private void DrawForceBarOutline()
    {
        float magForce = _haptic.MagForce;

        // same “height” math as your gizmo:
        // size.y = 0.05f * MagForce
        float height = barHeightScale * magForce;

        // same center offset as your gizmo:
        // position = CollisionMesh.pos + (0, 0.05f * MagForce / 2, 0)
        Vector3 center = collisionMesh.position + new Vector3(0f, height * 0.5f, 0f);

        // half-width in X/Z
        float hx = barWidth * 0.5f;
        float hz = barWidth * 0.5f;

        // We draw an outline rectangle in the X-Y plane at z = center.z
        // (If you want a 3D box outline, say so — I’ll draw all 12 edges like before.)
        _pts[0] = new Vector3(center.x - hx, center.y - height * 0.5f, center.z);
        _pts[1] = new Vector3(center.x + hx, center.y - height * 0.5f, center.z);
        _pts[2] = new Vector3(center.x + hx, center.y + height * 0.5f, center.z);
        _pts[3] = new Vector3(center.x - hx, center.y + height * 0.5f, center.z);
        _pts[4] = _pts[0]; // close

        // Match your color mapping:
        // new Color(2 * ratio, 2*(1-ratio), 0)
        float ratio = (maxForce > 0f) ? Mathf.Clamp01(_haptic.CurrentForce.magnitude / maxForce) : 0f;

        ApplyForceColorToMaterial(_haptic.CurrentForce.magnitude, maxForce);
        _lr.SetPositions(_pts);
    }

    private void ApplyForceColorToMaterial(float currentForceMagnitude, float maxForce)
    {
        // Match your Gizmos calculation exactly
        float t = (maxForce > 0f) ? (currentForceMagnitude / maxForce) : 0f;

        Color c = new Color(
            2.0f * t,
            2.0f * (1.0f - t),
            0.0f
        );

        // Set on material via property block (preferred)
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        _lr.GetPropertyBlock(_mpb);

        // Set both to support URP + legacy shaders
        _mpb.SetColor(ColorId, c);     // "_BaseColor"
        _mpb.SetColor(ColorIdFallback, c);         // "_Color"

        _lr.SetPropertyBlock(_mpb);
    }
}