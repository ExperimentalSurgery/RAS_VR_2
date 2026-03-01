using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HapticPlugin))]
public class HapticsVisualizerFilledBar : MonoBehaviour
{
    [Header("Runtime Render")]
    private static bool renderInPlayMode = true;
    [SerializeField] private Sprite redneringEnabledSprite;
    [SerializeField] private Sprite redneringDisabledSprite;
    [SerializeField] private Image targetImage;

    [Header("Force Bar (Filled)")]
    [SerializeField] private Transform collisionMesh;        // defaults to _haptic.CollisionMesh.transform if null
    [SerializeField] private float barWidth = 0.01f;         // x/z size (matches Gizmos: 0.01f)
    [SerializeField] private float barHeightScale = 0.05f;   // height = 0.05f * MagForce (matches Gizmos)
    [SerializeField] private float maxForce = 1.0f;          // MaxForce for color mapping
    [SerializeField, Range(0f, 1f)] private float opacity = 1.0f;

    [Header("Materials")]
    [Tooltip("Use an OPAQUE material to avoid see-through. URP: Lit/Unlit (Surface=Opaque).")]
    [SerializeField] private Material barMaterial;

    [Header("Label (TextMeshPro 3D)")]
    [SerializeField] private bool showLabel = true;
    [SerializeField] private TMP_Text labelPrefab;           // assign a TextMeshPro (3D) prefab
    [SerializeField] private float labelYOffset = 0.025f;    // matches Gizmos: +0.025f
    [SerializeField] private float labelFontSize = 0.2f;
    [SerializeField] private Transform labelLookAt;          // optional: camera for billboard

    private HapticPlugin _haptic;

    private GameObject _barGO;
    private Transform _barTf;
    private MeshRenderer _barRenderer;

    private TMP_Text _label;

    private MaterialPropertyBlock _mpb;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP
    private static readonly int ColorId = Shader.PropertyToID("_Color");     // Built-in/legacy

    private void Awake()
    {
        _haptic = GetComponent<HapticPlugin>();
        _mpb = new MaterialPropertyBlock();

        // Auto-assign collision mesh if not set
        if (collisionMesh == null && _haptic.CollisionMesh != null)
            collisionMesh = _haptic.CollisionMesh.transform;

        // Create filled bar cube
        _barGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _barGO.name = "ForceBar_Filled";
        _barTf = _barGO.transform;
        _barTf.SetParent(transform, worldPositionStays: true);

        // Remove collider (visual only)
        var col = _barGO.GetComponent<Collider>();
        if (col) Destroy(col);

        _barRenderer = _barGO.GetComponent<MeshRenderer>();
        if (barMaterial != null)
            _barRenderer.sharedMaterial = barMaterial;

        // Create label (optional)
        if (showLabel)
        {
            if (labelPrefab != null)
            {
                _label = Instantiate(labelPrefab, transform);
                _label.fontSize = labelFontSize;
            }
            else
            {
                Debug.LogWarning("HapticsVisualizer: labelPrefab is not assigned (TMP 3D). Label will not be shown.");
                showLabel = false;
            }

            if (labelLookAt == null && Camera.main != null)
                labelLookAt = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying || !renderInPlayMode)
        {
            SetActiveVisuals(false);
            return;
        }

        if (collisionMesh == null || _haptic.MagForce <= 0f)
        {
            SetActiveVisuals(false);
            return;
        }

        SetActiveVisuals(true);
        UpdateFilledBarAndLabel();
    }

    public void ToogleRendering()
    {
        renderInPlayMode = !renderInPlayMode;
        if(targetImage != null)
        {
            targetImage.sprite = renderInPlayMode ? redneringEnabledSprite : redneringDisabledSprite;
        }
    }
    private void SetActiveVisuals(bool active)
    {
        if (_barGO != null) _barGO.SetActive(active);
        if (_label != null) _label.gameObject.SetActive(active);
    }

    private void UpdateFilledBarAndLabel()
    {
        float magForce = _haptic.MagForce;

        // Same Gizmos math:
        // size.y = 0.05f * MagForce
        float height = barHeightScale * magForce;

        // Same Gizmos placement:
        // DrawCube(CollisionMesh.pos + (0, height/2, 0), size)
        Vector3 center = collisionMesh.position + new Vector3(0f, height * 0.5f, 0f);

        // Filled bar transform (Gizmos.matrix = identity -> no local-to-world mixing)
        _barTf.position = center;
        _barTf.rotation = Quaternion.identity;
        _barTf.localScale = new Vector3(barWidth, height, barWidth);

        // Color mapping exactly like your Gizmos (applied to material)
        ApplyForceColorToMaterial(_haptic.CurrentForce.magnitude, maxForce);

        // Label (same logic as Handles.Label placement)
        if (_label != null)
        {
            // Your original:
            // base = pos + (0, height/2, 0)
            // label = base + (0, height/2 + 0.025, 0)
            Vector3 basePos = collisionMesh.position + new Vector3(0f, height * 0.5f, 0f);
            Vector3 labelPos = basePos + new Vector3(0f, height * 0.5f + labelYOffset, 0f);

            _label.transform.position = labelPos;
            _label.text = magForce.ToString("0.###");

            // Optional billboard to camera
            if (labelLookAt != null)
            {
                Vector3 dir = _label.transform.position - labelLookAt.position;
                if (dir.sqrMagnitude > 0.0001f)
                    _label.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
        }
    }

    private void ApplyForceColorToMaterial(float currentForceMagnitude, float maxForceValue)
    {
        // EXACT Gizmos mapping:
        // new Color(2*t, 2*(1-t), 0) where t = CurrentForce.magnitude / MaxForce
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
}