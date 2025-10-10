using UnityEngine;

public class ForceFiieldOnTrigger : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] Transform hapticObject;
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
                hapticObject.GetComponent<MeshCollider>().enabled = false;
                hapticObject.GetComponent<CapsuleCollider>().enabled = false;
                foreach (var sphereCollider in hapticObject.GetComponents<SphereCollider>())
                {
                    sphereCollider.enabled = false;
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
                hapticObject.GetComponent<MeshCollider>().enabled = true;
                hapticObject.GetComponent<CapsuleCollider>().enabled = true;
                foreach (var sphereCollider in hapticObject.GetComponents<SphereCollider>())
                {
                    sphereCollider.enabled = true;
                }
                meshRenderer.enabled = false; // Disable the renderer when the collision ends
            }
            Debug.Log("Collision exit with haptic collider detected: " + other.gameObject.tag);
        }

    }
}
