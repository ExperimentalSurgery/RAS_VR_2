using UnityEngine;

public class ForceFiieldOnTrigger : MonoBehaviour
{
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
                meshRenderer.enabled = true; // Enable the renderer when a collision occurs
            }
            Debug.Log("Collision with haptic collider detected: " + other.gameObject.tag);
        }
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "HapticCollider_Right" || other.gameObject.tag == "HapticCollider_Left")
        {
            if (meshRenderer.enabled)
            {
                meshRenderer.enabled = false; // Disable the renderer when the collision ends
            }
            Debug.Log("Collision exit with haptic collider detected: " + other.gameObject.tag);
        }

    }
}
