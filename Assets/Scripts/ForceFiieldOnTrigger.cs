using UnityEngine;

public class ForceFiieldOnTrigger : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] Transform hapticObject;
    [SerializeField] bool haptocObjectHasMeshColliders = false;
    [SerializeField] bool haptocObjectHasCapsuleColliders = false;
    [SerializeField] private ForceFieldHandler forceFieldHandler;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "HapticCollider_Right" || other.gameObject.tag == "HapticCollider_Left")
        {
            if (!meshRenderer.enabled)
            {
                if (haptocObjectHasMeshColliders)
                {
                    foreach (var collider in hapticObject.GetComponents<MeshCollider>())
                    {
                        collider.enabled = false;
                    }
                }
                else if (haptocObjectHasCapsuleColliders)
                {
                    foreach (var collider in hapticObject.GetComponents<CapsuleCollider>())
                    {
                        collider.enabled = false;
                    }
                }       
                meshRenderer.enabled = true; // Enable the renderer when a collision occurs
            }
            Debug.Log("Collision with haptic collider detected: " + other.gameObject.tag);
        }
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if(forceFieldHandler.isTouched) { return; }
        if (other.gameObject.tag == "HapticCollider_Right" || other.gameObject.tag == "HapticCollider_Left")
        {
            if (meshRenderer.enabled)
            {
                if (haptocObjectHasMeshColliders)
                {
                    foreach (var collider in hapticObject.GetComponents<MeshCollider>())
                    {
                        collider.enabled = true;
                    }
                }
                else if (haptocObjectHasCapsuleColliders)
                {
                    foreach (var collider in hapticObject.GetComponents<CapsuleCollider>())
                    {
                        collider.enabled = true;
                    }
                }
                meshRenderer.enabled = false; // Disable the renderer when the collision ends
            }
            Debug.Log("Collision exit with haptic collider detected: " + other.gameObject.tag);
        }

    }
}
