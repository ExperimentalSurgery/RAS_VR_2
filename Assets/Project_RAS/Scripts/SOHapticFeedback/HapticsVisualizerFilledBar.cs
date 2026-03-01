using UnityEngine;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(HapticPlugin))]
public class HapticsVisualizerFilledBar : MonoBehaviour
{
    [Header("Force Label")]
    [SerializeField] private bool showLabel = true;
    [SerializeField] private TMP_Text labelPrefab; // assign a TextMeshPro (3D) prefab
    [SerializeField] private float labelYOffset = 0.025f; // matches your +0.025f
    [SerializeField] private float labelFontSize = 0.2f;
    //[SerializeField] private Transform labelLookAt; // optional (camera)

    [Header("Runtime Debug Render")]
    [SerializeField] private bool renderInPlayMode = true;

    [Header("Force Bar (Filled)")]
    [SerializeField] private Transform collisionMesh;       // CollisionMesh.transform
    [SerializeField] private float barWidth = 0.01f;        // x/z size
    [SerializeField] private float barHeightScale = 0.05f;  // height = scale * MagForce
    [SerializeField] private float maxForce = 1.0f;         // for color mapping
    [SerializeField, Range(0f, 1f)] private float opacity = 1.0f;

    [Header("Material")]
    [Tooltip("Use an opaque material to prevent see-through. For URP: Lit/Unlit (Opaque).")]
    [SerializeField] private Material barMaterial;

    private HapticPlugin _haptic;

    private GameObject _barGO;
    private Transform _barTf;
    private MeshRenderer _barRenderer;

    private TMP_Text _label;

    private MaterialPropertyBlock _mpb;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP
    private static readonly int ColorId = Shader.PropertyToID("_Color");         // legacy

    private void Awake()
    {
        _haptic = GetComponent<HapticPlugin>();
        _mpb = new MaterialPropertyBlock();

        if (collisionMesh == null && _haptic.CollisionMesh != null)
            collisionMesh = _haptic.CollisionMesh.transform;

        // Create a Unity cube primitive as the filled bar
        _barGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _barGO.name = "ForceBar_Filled";
        _barTf = _barGO.transform;
        _barTf.SetParent(transform, worldPositionStays: true);

        // Remove collider from primitive (we only want visuals)
        var col = _barGO.GetComponent<Collider>();
        if (col) Destroy(col);

        _barRenderer = _barGO.GetComponent<MeshRenderer>();

        if (barMaterial != null)
            _barRenderer.sharedMaterial = barMaterial;

        CreateLabel();
        //if (labelLookAt == null && Camera.main != null) labelLookAt = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying || !renderInPlayMode)
        {
            if (_barGO) _barGO.SetActive(false);
            return;
        }

        if (_haptic.MagForce <= 0f || collisionMesh == null)
        {
            if (_barGO) _barGO.SetActive(false);
            return;
        }

        _barGO.SetActive(true);
        UpdateFilledBar();
        UpdateForceLabel();
    }

    public void ToggleHapticVisualization()
    {
        renderInPlayMode = !renderInPlayMode;
    }

    private void UpdateFilledBar()
    {
        float magForce = _haptic.MagForce;
        float height = barHeightScale * magForce;

        // Same as your Gizmos.DrawCube:
        // position = CollisionMesh.pos + (0, height/2, 0)
        Vector3 center = collisionMesh.position + new Vector3(0f, height * 0.5f, 0f);

        // Size = (0.01, 0.05*MagForce, 0.01)
        _barTf.position = center;
        _barTf.rotation = Quaternion.identity; // matches Gizmos.matrix = identity
        _barTf.localScale = new Vector3(barWidth, height, barWidth);

        ApplyForceColorToMaterial(_haptic.CurrentForce.magnitude, maxForce);
    }

    private void ApplyForceColorToMaterial(float currentForceMagnitude, float maxForceValue)
    {
        // Your exact mapping
        float t = (maxForceValue > 0f) ? (currentForceMagnitude / maxForceValue) : 0f;

        Color c = new Color(
            2.0f * t,
            2.0f * (1.0f - t),
            0.0f,
            opacity
        );

        _barRenderer.GetPropertyBlock(_mpb);

        // Set both for URP + built-in compatibility
        _mpb.SetColor(BaseColorId, c);
        _mpb.SetColor(ColorId, c);

        _barRenderer.SetPropertyBlock(_mpb);
    }

    private void CreateLabel()
    {
        if (!showLabel || _label != null) return;
        if (labelPrefab == null)
        {
            Debug.LogWarning("HapticsVisualizer: labelPrefab not assigned (TMP 3D).");
            return;
        }

        _label = Instantiate(labelPrefab, transform);
        _label.transform.localScale = Vector3.one;
        _label.fontSize = labelFontSize;
    }

    private void UpdateForceLabel()
    {
        if (!showLabel || _label == null)
            return;

        if (_haptic.MagForce <= 0f || collisionMesh == null)
        {
            _label.gameObject.SetActive(false);
            return;
        }

        _label.gameObject.SetActive(true);

        float magForce = _haptic.MagForce;

        // Your gizmo math: 0.05f * MagForce
        float y = barHeightScale * magForce;

        // Exactly matching the two-step offset from Handles.Label:
        Vector3 basePos = collisionMesh.position + new Vector3(0f, y / 2f, 0f);
        Vector3 labelPos = basePos + new Vector3(0f, y / 2f + labelYOffset, 0f);

        _label.transform.position = labelPos;
        _label.text = magForce.ToString("0.###"); // same as "" + MagForce, just formatted

        // Optional: billboard to camera
        //if (labelLookAt != null)
        //{
        //    Vector3 dir = _label.transform.position - labelLookAt.position;
        //    if (dir.sqrMagnitude > 0.0001f)
        //        _label.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        //}
    }

}